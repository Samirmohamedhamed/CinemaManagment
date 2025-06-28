using Cinema.Data;
using Cinema.Models;
using Cinema.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Cinema.Models.ViewModel.RelatedFilmsVM;

namespace Cinema.Areas.Spectator.Controllers
{
    [Area("Spectator")]

    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context = new();
        public IActionResult Index(string genre, string NameOfFilm, string NameOfActor)
        {
            IQueryable<Film> films = _context.Films.Include(e => e.Cinemaa).Include(e => e.FilmActors).ThenInclude(e => e.Actor).Include(e=>e.images);
            ViewData["films"] = _context.Films.ToList(); ;
            if(genre is not null )
            {
                films = films.Where(e => e.Genre.Contains(genre));
                ViewBag.genre = genre;
            }
            if(NameOfFilm is not null )
            {
                films = films.Where(e =>e.Title.Contains(NameOfFilm));
                ViewBag.nameOfFilm = NameOfFilm;
            }
            if(NameOfActor is not null )
            {
                films = films.Where(e => e.FilmActors.Any(e=>e.Actor.FullName.Contains(NameOfActor)));
                ViewBag.nameOfActor = NameOfActor;

            }
            return View(films.ToList());
        }

        public IActionResult Details(int id)
        {
            var film = _context.Films.Include(e => e.Cinemaa).Include(e => e.images).Include(e => e.FilmActors)
                .ThenInclude(e => e.Actor).FirstOrDefault(e => e.Id == id);
            if (film is not null)
            {
                var Relatedfilms = _context.Films.Include(e => e.Cinemaa).Include(e => e.images).Include(e => e.FilmActors).ThenInclude(e => e.Actor).Where(e => e.Genre == film.Genre&& e.Id!=film.Id).Skip(0).Take(4);
                var actorId = film.FilmActors.FirstOrDefault()?.ActorId;
                if (actorId == null)
                {
                    return RedirectToAction(nameof(NotFoundPage));

                }
                var SameActorFilms = _context.Films.Include(e=>e.images)
                    .Include(e=>e.Cinemaa)
                    .Include(e => e.FilmActors)
                    .ThenInclude(e => e.Actor)
                    .Where(e => e.FilmActors.Any(fa => fa.ActorId == actorId) && e.Id != film.Id)
                    .Skip(0).Take(4);
                
          
                var films = new RelatedFilmsVM()
                {
                    film = film,
                    Relatedfilms = Relatedfilms.ToList(),
                    SameActorFilms = SameActorFilms.ToList(),
                };
                return View(films);



            }

            return RedirectToAction(nameof(NotFoundPage));

        }
        public IActionResult ActorDetails(int id)
        {
            var actor = _context.Actors
            .Include(a => a.FilmActors)
            .ThenInclude(fa => fa.Film)
            .FirstOrDefault(a => a.Id == id);

            if (actor == null)
            {
                return RedirectToAction(nameof(NotFoundPage));
            }

            return View(actor);

           
        }
        public IActionResult NotFoundPage()
        {
            return View();
        }
      

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
