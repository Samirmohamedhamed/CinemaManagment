using Cinema.Data;
using Cinema.Models;
using Cinema.Repositories;
using Cinema.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CinemaController : Controller
    {
        private readonly ICinemaRepository _cinemaRepository;//= new CinemaRepository();
        private readonly ApplicationDbContext _context;

        public CinemaController(ICinemaRepository cinemaRepository, ApplicationDbContext context)
        {
            _cinemaRepository = cinemaRepository;
            _context = context;
        }

        public IActionResult Index()
        {
            var cinemas = _cinemaRepository.Get();
            return View(cinemas.ToList());
        }
        //public IActionResult CinemaDetails(int id)
        //{
        //    var cinemas = _context.Cinemas;
        //    return View(cinemas.ToList());
        //}
        public IActionResult Create()
        {
           ViewData["films"] = _context.Films.ToList();
            return View(new Cinemaa());
        }
        [HttpPost]
 
        public async Task<IActionResult> Create(Cinemaa cinemaa)
        {
           
            if (ModelState.IsValid)
            {

               await _cinemaRepository.CreateAsync(cinemaa);
               await _cinemaRepository.CommitAsync();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewData["films"] = _context.Films.ToList();
                return View(cinemaa);
            }
        }
        public IActionResult Edit(int id)
        {
           var cinema= _cinemaRepository.GetOne(e=>e.Id==id);
            if(cinema is not null)
            {
                ViewData["films"] = _context.Films.ToList();

                return View(cinema);

            }
            return RedirectToAction(nameof(NotFoundPage));

        }
        [HttpPost]
         public async Task<IActionResult> Edit(Cinemaa cinemaa)
        {
           
            if (ModelState.IsValid)
            {
                _cinemaRepository.UpdateAsync(cinemaa);
              await  _cinemaRepository.CommitAsync();

                return RedirectToAction(nameof(Index));
            }
            else
            {
               ViewData["films"] = _context.Films.ToList();

                return View(cinemaa);
            }
        }
        public async Task<IActionResult> Delete(int id)
        {
            var cinema = _cinemaRepository.GetOne(e => e.Id == id);
            if (cinema is not null)
            {
              _cinemaRepository.Remove(cinema);
              await  _cinemaRepository.CommitAsync();//.SaveChanges();
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
