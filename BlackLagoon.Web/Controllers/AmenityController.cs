using BlackLagoon.Application.Common.Interfaces;
using BlackLagoon.Application.Common.Utility;
using BlackLagoon.Domain.Entities;
using BlackLagoon.Infrastructure.Data;
using BlackLagoon.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlackLagoon.Web.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    public class AmenityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public AmenityController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
      
        private readonly ApplicationDbContext _context;


        public IActionResult Index()
        {
            var amenities = _unitOfWork.Amenity.GetAll(IncludeProperties: "Villa");
            return View(amenities);
        }
        public IActionResult Create()
        {
            /*The view model approach is preferred when you have multiple pieces of data to send to the view.
                         *
             */
            AmenityVM amenityVM = new()
            {
                
                VillaList = _unitOfWork.Villa.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            //View accessed model[amenity.cs] directly which is not a good practice

            /*IEnumerable<SelectListItem>list = _context.Villas.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            *//*
             * If you want to send data to the view, you can use ViewData or ViewBag.
             *//*
            //ViewData["VillaList"] = list;
            ViewBag.VillaList = list;*/
            return View(amenityVM);
        }
        [HttpPost]
        public IActionResult Create(AmenityVM obj)
        {


            if (ModelState.IsValid)
            {
                _unitOfWork.Amenity.Add(obj.Amenity);
                _unitOfWork.Save();
                TempData["success"] = "The Amenity has been created succesfully";

                //return RedirectToAction(nameof(Index));
                return RedirectToAction(nameof(Index));
            }
           
            obj.VillaList = _unitOfWork.Villa.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            return View(obj);
        }
        public IActionResult Update(int amenityId)
        {
            AmenityVM amenityVM = new()
            {

                VillaList = _unitOfWork.Villa.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                Amenity = _unitOfWork.Amenity.Get(u => u.Id == amenityId)
            };
            if (amenityVM.Amenity == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(amenityVM);
        }
        [HttpPost]
        public IActionResult Update(AmenityVM amenityVM)
        {
            //bool roomNumberExists = _context.Amenitys.Any(u => u.Villa_Number == obj.Amenity!.Villa_Number);

            if (ModelState.IsValid)
            {
                _unitOfWork.Amenity.Update(amenityVM.Amenity);
                _unitOfWork.Save();
                TempData["success"] = "The Amenity has been updated succesfully";

                //return RedirectToAction(nameof(Index));
                return RedirectToAction(nameof(Index));
            }


            amenityVM.VillaList = _unitOfWork.Villa.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            return View(amenityVM);




        }
        public IActionResult Delete(int amenityId)
        {
            AmenityVM amenityVM = new()
            {

                VillaList = _unitOfWork.Villa.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                Amenity = _unitOfWork.Amenity.Get(u => u.Id == amenityId)
            };
            if (amenityVM.Amenity == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(amenityVM);
        }
        [HttpPost]
        public IActionResult Delete(AmenityVM amenityVM)
        {

            Amenity? objFromDb = _unitOfWork.Amenity.Get(u => u.Id == amenityVM.Amenity.Id);
            if (objFromDb is not null)
            {
                _unitOfWork.Amenity.Delete(objFromDb);
                _unitOfWork.Save();
                TempData["success"] = "The Amenity has been succesfully deleted";
                //return RedirectToAction(nameof(Index));
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The Amenity could not  be deleted";

            return View();
        }
    }
}
