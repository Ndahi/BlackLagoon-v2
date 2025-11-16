using BlackLagoon.Application.Common.Interfaces;
using BlackLagoon.Domain.Entities;
using BlackLagoon.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlackLagoon.Web.Controllers
{
    [Authorize]
    public class VillaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public VillaController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var villas = _unitOfWork.Villa.GetAll();
            return View(villas);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Villa obj)
        {
            if (obj.Name == obj.Description)
            {
                ModelState.AddModelError("", "The Description cannot exactly match the name");
            }
            if (obj.Occupancy <= 0)
            {
                ModelState.AddModelError("Occupancy", "The number of occupants cannot be less than one ");

            }

            if (ModelState.IsValid)
            {
                if(obj.Image is not null)
                {
                    //string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString()+ Path.GetExtension(obj.Image.FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImages");
                    //var uploads = Path.Combine(wwwRootPath, @"images\Villa Images");

                    var extension = Path.GetExtension(obj.Image.FileName);
                  using (var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create))
                    
                        obj.Image.CopyTo(fileStream);
                    
                    obj.ImageUrl = @"\images\VillaImages\" + fileName;
                }
                else
                {
                    obj.ImageUrl = "https://placehold.co/600x402";
                }
                _unitOfWork.Villa.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "The villa has been created succesfully";

                //return RedirectToAction(nameof(Index));
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }
        public IActionResult Update(int villaId)
        {
            Villa? obj = _unitOfWork.Villa.Get(u => u.Id == villaId);
           /* Villa? obj = _context.Villas.Find(villaId);
            var VillaList = _context.Villas.Where(u => u.Price > 50 && u.Occupancy > 0);*/
            if (obj == null)
            {
                return RedirectToAction("Error","Home");
            }
            return View(obj);
        }
        [HttpPost]
        public IActionResult Update(Villa obj)
        {
           

            if (ModelState.IsValid&&obj.Id>0)
            {
                if (obj.Image is not null)
                {
                    //string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImages");
                    //var uploads = Path.Combine(wwwRootPath, @"images\Villa Images");
                    if (!string.IsNullOrEmpty(obj.ImageUrl)) 
                    { 
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, imagePath.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath)) 
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    var extension = Path.GetExtension(obj.Image.FileName);
                    using (var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create))

                        obj.Image.CopyTo(fileStream);

                    obj.ImageUrl = @"\images\VillaImages\" + fileName;
                }
               

                _unitOfWork.Villa.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "The villa has been succesfully updated";

                //return RedirectToAction(nameof(Index));
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public IActionResult Delete(int villaId)
        {
            Villa? obj = _unitOfWork.Villa.Get(u => u.Id == villaId);
            /* Villa? obj = _context.Villas.Find(villaId);
             var VillaList = _context.Villas.Where(u => u.Price > 50 && u.Occupancy > 0);*/
            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }
        [HttpPost]
        public IActionResult Delete(Villa obj)
        {

            Villa? objFromDb = _unitOfWork.Villa.Get(u => u.Id == obj.Id);
            if (objFromDb is not null)
            {
                if (!string.IsNullOrEmpty(objFromDb.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, objFromDb.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                _unitOfWork.Villa.Delete(objFromDb);
                _unitOfWork.Save();
                TempData["success"] = "The villa has been succesfully deleted";
                //return RedirectToAction(nameof(Index));
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The villa could not  be deleted";

            return View();
        }
    }
}
