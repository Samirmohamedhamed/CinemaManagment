using Cinema.Data;
using Cinema.Models;
using Cinema.Repositories;
using Cinema.Repositories.IRepositories;
using Cinema.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ActorController : Controller
    {
        private readonly IActorRepository _actorRepository;//= new ActorRepository();
        public ActorController(IActorRepository actorRepository)
        {
            _actorRepository = actorRepository;
        }
        public IActionResult Index()
        {
            var actors = _actorRepository.Get();
            return View(actors.ToList());
        }
        public IActionResult Create()
        {

            return View(new Actor());
        }
        [HttpPost]

        public async Task<IActionResult> Create(Actor actor, IFormFile MainImage)
        {

            if (ModelState.IsValid && MainImage != null && MainImage.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(MainImage.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\image", fileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    MainImage.CopyTo(stream);
                }
                actor.ImageUrl = fileName;
               await _actorRepository.CreateAsync(actor);
              await _actorRepository.CommitAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(actor);

        }
        public IActionResult Edit(int id)
        {
            var actor = _actorRepository.GetOne(e=>e.Id == id);
            if (actor is not null)
            {
                return View(actor);

            }
            return RedirectToAction(nameof(NotFoundPage));

        }
        [HttpPost]
        public async Task<IActionResult> Edit(Actor actor, IFormFile MainImage)
        {
            ModelState.Remove("MainImage");
            var ActorInDb = _actorRepository.GetOne(e => e.Id == actor.Id, traked: false);// _context.Actors.AsNoTracking().FirstOrDefault(e => e.Id == actor.Id);
            if (ModelState.IsValid && ActorInDb is not null)
            {
                if (MainImage != null && MainImage.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(MainImage.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\image", fileName);
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await MainImage.CopyToAsync(stream);

                    }
                    //Delete Old Image
                    if (actor.ImageUrl!= "DefualtImage.png")
                    {
                        var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\image", ActorInDb.ImageUrl);
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                    }
                  
                    actor.ImageUrl = fileName;
                }
                else
                {
                    actor.ImageUrl = ActorInDb.ImageUrl;
                }
                _actorRepository.UpdateAsync(actor);
               await _actorRepository.CommitAsync();
             

                return RedirectToAction(nameof(Index));
            }
            return View(actor);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var actor = _actorRepository.GetOne(e => e.Id == id); //_context.Actors.Find(id);
            if (actor is not null )
            {
                if (actor.ImageUrl != "DefualtImage.png")
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\image", actor.ImageUrl);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }
                _actorRepository.Remove(actor);
               await _actorRepository.CommitAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(NotFoundPage));
        }
        public IActionResult NotFoundPage()
        {
            return View();
        }
        public async Task<IActionResult> DeleteImage(int id)
        {
            var actor = _actorRepository.GetOne(e=>e.Id==id);//_context.Actors.FirstOrDefault(e => e.Id == id);
            if (actor is not null &&actor.ImageUrl!= "DefualtImage.png")
            {
                var OldFileName = actor.ImageUrl;
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\image", OldFileName);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }
                actor.ImageUrl = "DefualtImage.png";
                await _actorRepository.CommitAsync();
                return RedirectToAction(nameof(Edit), new { id = id });
            }
            return RedirectToAction(nameof(NotFoundPage));


        }
        //public IActionResult ReplaceImage(int id, IFormFile MainImage)
        //{
        //    var actor = _context.Actors.FirstOrDefault(e => e.Id == id);
        //    if (actor is not null)
        //    {

        //        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(MainImage.FileName);
        //        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\image", fileName);
        //        using (var stream = System.IO.File.Create(filePath))
        //        {
        //            MainImage.CopyTo(stream);
        //        }
        //        //Delete Old Image
        //        var OldFileName = actor.ImageUrl;
        //        var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\image", OldFileName);
        //        if (System.IO.File.Exists(oldPath))
        //        {
        //            System.IO.File.Delete(oldPath);
        //        }
        //        actor.ImageUrl = fileName;
        //        _context.SaveChanges();
        //        return RedirectToAction(nameof(Edit), new { id = id });
        //    }

        //    return RedirectToAction(nameof(NotFoundPage));


        //}
    }
}
