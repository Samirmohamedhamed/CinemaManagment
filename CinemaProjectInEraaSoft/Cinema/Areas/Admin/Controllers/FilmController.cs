using Cinema.Data;
using Cinema.Models;
using Cinema.Repositories;
using Cinema.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FilmController : Controller
    {
        private readonly IFilmRepository _filmRepository;// = new FilmRepository() ;
        private readonly ApplicationDbContext _context;

        public FilmController(IFilmRepository filmRepository, ApplicationDbContext context)
        {
            _filmRepository = filmRepository;
            _context = context;
        }

        public IActionResult Index()
        {
            var films = _filmRepository.Get();
            return View(films.ToList());
        }
        public IActionResult Create()
        {
            ViewData["cinema"] = _context.Cinemas.ToList();
            ViewData["actor"] = _context.Actors.ToList();

            return View(new Film());
        }
        [HttpPost]
        public async Task<IActionResult> Create(Film film)
        {
           
            ModelState.Remove("Cinemaa");
            if (ModelState.IsValid)
            {
                // تأكد من أن FilmActors تحتوي على ممثلين قبل إضافة الفيلم
                if (film.FilmActors != null && film.FilmActors.Count > 0)
                {
                    // إضافة الفيلم إلى قاعدة البيانات
                  await  _filmRepository.CreateAsync(film);
                  await _filmRepository.CommitAsync();//.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction(nameof(NotFoundPage));

            }
            else
            {
                ViewData["cinema"] = _context.Cinemas.ToList();
                ViewData["actor"] = _context.Actors.ToList();
                return View(film);
            }
        }
        public IActionResult Edit(int id)
        {
            var film = _filmRepository.GetOne(e => e.Id == id);
            if (film is not null)
            {
                ViewData["cinema"] = _context.Cinemas.ToList();
                ViewData["actor"] = _context.Actors.ToList();

                return View(film);

            }
            return RedirectToAction(nameof(NotFoundPage));

        }
        [HttpPost]
        public async Task<IActionResult> Edit(Film film)
        {
           
            if (ModelState.IsValid)
            {
                _filmRepository.UpdateAsync(film);
               await _filmRepository.CommitAsync();//.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            ViewData["cinema"] = _context.Cinemas.ToList();
            ViewData["actor"] = _context.Actors.ToList();

            return View(film);
            }
        public async Task<IActionResult> Delete(int id)
        {
            var film = _filmRepository.GetOne(e => e.Id == id);
            if (film is not null)
            {
                _filmRepository.Remove(film);
              await  _filmRepository.CommitAsync();//.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(NotFoundPage));
        }
        public IActionResult NotFoundPage()
        {
            return View();
        }
    }
}
