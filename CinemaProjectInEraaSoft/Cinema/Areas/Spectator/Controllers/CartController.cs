using Cinema.Models;
using Cinema.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Areas.Spectator.Controllers
{
    [Area("Spectator")]
    public class CartController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IFilmRepository filmRepository;
        private readonly ICartRepository cartRepository;

        public CartController(UserManager<ApplicationUser> userManager,IFilmRepository filmRepository, ICartRepository cartRepository)
        {
            this.userManager = userManager;
            this.filmRepository = filmRepository;
            this.cartRepository = cartRepository;
        }
        public async Task<IActionResult> AddToCart(int filmId, int count)
        {
            var user = await userManager.GetUserAsync(User);
            if (user is not null)
            {
                var film = filmRepository.GetOne(e => e.Id == filmId);
                if (film is not null)
                {
                    var cartInDb = cartRepository.GetFirstOrDefault(e => e.FilmId == filmId && e.ApplicationId == user.Id);
                    if (cartInDb is not null)
                    {
                        cartInDb.CountOFTicet += count;
                    }
                    else
                    {
                        if (count <= film.CountOfTicket && count > 0)
                        {
                            await cartRepository.CreateAsync(new()
                            {
                                FilmId = filmId,
                                ApplicationId = user.Id,
                                CountOFTicet = count,
                            });
                        }
                    }

                    await cartRepository.CommitAsync();
                    TempData["Notfication"] = "Add Film To Cart";

                    return RedirectToAction("Index", "Home", new { area = "Spectator" });
                }
                return BadRequest();
            }
            return BadRequest();

        }
        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);
            if (user != null)
            {
                var carts = cartRepository.Get(expression: e => e.ApplicationId == user.Id, Includes: [e => e.Film]);
                ViewBag.TotalSalary = carts.Sum(e => e.Film.PriceOfTicket * e.CountOFTicet);
                return View(carts.ToList());
            }
            return BadRequest();
        }
        public async Task<IActionResult> IncrementValue(int filmId)
        {
            var user = await userManager.GetUserAsync(User);
            var cartInDb = cartRepository.GetFirstOrDefault(e => e.FilmId == filmId && e.ApplicationId == user.Id);
            if (cartInDb is not null)
            {
                cartInDb.CountOFTicet++;
                await cartRepository.CommitAsync();
                return RedirectToAction("Index");
            }
            return BadRequest();


        }
        public async Task<IActionResult> decrement(int filmId)
        {
            var user = await userManager.GetUserAsync(User);
            if (user is not null)
            {
                var cartInDb = cartRepository.GetFirstOrDefault(e => e.FilmId == filmId && e.ApplicationId == user.Id);
                if (cartInDb is not null)
                {
                    cartInDb.CountOFTicet--;
                    await cartRepository.CommitAsync();
                    return RedirectToAction("Index");

                }
            }
            return BadRequest();


        }
        public async Task<IActionResult> DeleteItemFromCart(int filmId)
        {
            var user = await userManager.GetUserAsync(User);
            var cartInDb = cartRepository.GetFirstOrDefault(e => e.FilmId == filmId && e.ApplicationId == user.Id);
            if (cartInDb is not null)
            {
                cartRepository.Remove(cartInDb);
                await cartRepository.CommitAsync();
                return RedirectToAction("Index");

            }
            return BadRequest();


        }

    }
}
