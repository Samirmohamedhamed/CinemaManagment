using Cinema.Data;
using Cinema.Models;
using Cinema.Repositories;
using Cinema.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Stripe.Checkout;

namespace Cinema.Areas.Spectator.Controllers
{
    [Area("Spectator")]
    public class CheckOutController : Controller
    {
        private readonly IOrderItemsRepository orderItemsRepository;
        private readonly IFilmRepository filmRepository;
        private readonly ICartRepository cartRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IOrderRepository orderRepository;
        private readonly ApplicationDbContext applicationDbContext;

        public CheckOutController(IOrderItemsRepository orderItemsRepository,IFilmRepository filmRepository,ICartRepository cartRepository,UserManager<ApplicationUser> userManager,IOrderRepository orderRepository,ApplicationDbContext applicationDbContext)
        {
            this.orderItemsRepository = orderItemsRepository;
            this.filmRepository = filmRepository;
            this.cartRepository = cartRepository;
            this.userManager = userManager;
            this.orderRepository = orderRepository;
            this.applicationDbContext = applicationDbContext;
        }
        public async Task<IActionResult> Pay()
        {
            var user = await userManager.GetUserAsync(User);
            if (user is not null)
            {
                var cartInDb = cartRepository.Get(e => e.ApplicationId == user.Id, Includes: [e => e.Film]);
                if (cartInDb.Any())
                {
                    Order order = new()
                    {
                        ApplicationId = user.Id,
                        OrderDate = DateTime.UtcNow,
                        OrderStatus = OrderStatus.pending,
                        TotalPrice = cartInDb.Sum(e => e.Film.PriceOfTicket * e.CountOFTicet),
                        TransactionStatus = TransactionStatus.pending
                    };
                    await orderRepository.CreateAsync(order);
                    await orderRepository.CommitAsync();
                    var options = new SessionCreateOptions
                    {
                        PaymentMethodTypes = new List<string> { "card" },// الشكل اللي علي الشمال خليه card 
                        LineItems = new List<SessionLineItemOptions>(),
                        Mode = "payment",
                        SuccessUrl = Url.Action("Success", "Checkout", new { area = "Spectator",id=order.Id }, Request.Scheme),
                        CancelUrl = Url.Action("Cancel", "Checkout", new { area = "Spectator", id = order.Id }, Request.Scheme),
                    };
                    foreach (var item in cartInDb)
                    {
                        options.LineItems.Add(new SessionLineItemOptions
                        {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                Currency = "egp",
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = item.Film.Title,
                                    Description = item.Film.Description,
                                },
                                UnitAmount = (long)(item.Film.PriceOfTicket * 100),
                            },
                            Quantity = item.CountOFTicet,
                        });
                    }
                    var service = new SessionService();
                    var session = service.Create(options);
                    order.SesstionId = session.Id;
                    await orderRepository.CommitAsync();
                    TempData["ValidNotfication"] = Guid.NewGuid().ToString();
                    return Redirect(session.Url);
                }
                return BadRequest();
            }
            return BadRequest();
        }
        public async Task<IActionResult> Success(int id)
        {
            var order1 = orderRepository.GetOne(e => e.Id == id);

            if (order1 == null)
            {
                return NotFound("Order not found");
            }
            if (TempData["ValidNotfication"] is not null)
            {
                var order = orderRepository.GetOne(e => e.Id == id);
                var cartInDb = cartRepository.Get(e => e.ApplicationId == order.ApplicationId, Includes: [e => e.Film]);
                var transaction = await applicationDbContext.Database.BeginTransactionAsync();

                try
                {
                    //Decrement Quantity Of Product
                    foreach (var item in cartInDb)
                    {
                        item.Film.CountOfTicket -= item.CountOFTicet;
                    }
                    await filmRepository.CommitAsync();
                    //Transfer cart => OrderItems
                    foreach (var item in cartInDb)
                    {
                        await orderItemsRepository.CreateAsync(new()
                        {
                            Count = item.CountOFTicet,
                            FilmId = item.FilmId,
                            OrderId = id,
                            ItemPrice = item.Film.PriceOfTicket,

                        });
                    }
                    await orderItemsRepository.CommitAsync();
                    //Remove Cart
                    foreach (var item in cartInDb)
                    {
                        cartRepository.Remove(item);
                    }
                    await cartRepository.CommitAsync();

                    //Update Order Prop
                    order.OrderStatus = OrderStatus.Inprocessing;
                    order.TransactionStatus = TransactionStatus.Completed;
                    var service = new SessionService();
                    var session = service.Get(order.SesstionId);
                    await orderRepository.CommitAsync();
                    transaction.Commit();
                    return View();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }


            }
            return BadRequest();
        }
        


        public IActionResult Cancel()
        {
            return View();
        }
    }
   
}
