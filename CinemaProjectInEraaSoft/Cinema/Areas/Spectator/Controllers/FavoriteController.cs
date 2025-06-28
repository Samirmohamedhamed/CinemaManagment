using Cinema.Models;
using Cinema.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Areas.Spectator.Controllers
{
        [Area("Spectator")]
        public class FavoriteController : Controller
        {
        private readonly IFilmRepository filmRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IFavoriteRepository favoriteRepository;

        public FavoriteController(IFilmRepository filmRepository,UserManager<ApplicationUser> userManager,IFavoriteRepository favoriteRepository)
            {
            this.filmRepository = filmRepository;
            this.userManager = userManager;
            this.favoriteRepository = favoriteRepository;
        }
            public async Task<IActionResult> AddToFavorites(int id)
            {
                var user = await userManager.GetUserAsync(User);
                if (user is not null)
                {
                    var film = filmRepository.GetOne(e => e.Id == id);
                var favlist = favoriteRepository.GetFirstOrDefault(e => e.ApplicationId == user.Id&&e.FilmId==id);
                if(film is not null)
                {
                    if (favlist == null)
                    {
                        Favorite favorite = new Favorite()
                        {
                            FilmId = id,
                            ApplicationId = user.Id,
                        };
                        await favoriteRepository.CreateAsync(favorite);
                        await favoriteRepository.CommitAsync();
                        TempData["Notfication"] = "Add Film To Favorites";
                        return RedirectToAction("Index", "Home", new { area = "Spectator" });
                    }
                    TempData["NotficationError"] = "Alredy in Fav List Film";
                    return RedirectToAction("Index", "Home", new { area = "Spectator" });
                }
                return BadRequest();
             

            }
            return BadRequest();
            }
            public async Task<IActionResult> Index()
            {
                var user = await userManager.GetUserAsync(User);
                if (user is not null)
                {
                    var favorite = favoriteRepository.Get(e => e.ApplicationId == user.Id, Includes: [e => e.Film]).ToList();

                    return View(favorite);
                }
                return BadRequest();
            }
            public async Task<IActionResult> DeleteItemFromFavorite(int filmId)
            {
                var user = await userManager.GetUserAsync(User);
                if (user is not null)
                {
                    var favorite = favoriteRepository.GetFirstOrDefault(e => e.ApplicationId == user.Id && e.FilmId == filmId);
                    if (favorite is not null)
                    {
                    favoriteRepository.Remove(favorite);
                        await favoriteRepository.CommitAsync();

                    }

                    return RedirectToAction("Index");
                }
                return BadRequest();
            }
        }
    }

