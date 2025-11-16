using BlackLagoon.Application.Common.Interfaces;
using BlackLagoon.Domain.Entities;
using BlackLagoon.Infrastructure.Data;
using BlackLagoon.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BlackLagoon.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public VillaNumberController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        private readonly ApplicationDbContext _context;

      
        public IActionResult Index()
        {
            var villaNumbers = _unitOfWork.VillaNumber.GetAll(IncludeProperties:"Villa");
            return View(villaNumbers);
        }
        public IActionResult Create()
        {
            /*The view model approach is preferred when you have multiple pieces of data to send to the view.
                         *
             */
            VillaNumberVM villaNumberVM = new()
            {
                VillaNumber = new(),
                VillaList = _unitOfWork.Villa.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            //View accessed model[villaNumber.cs] directly which is not a good practice

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
            return View(villaNumberVM);
        }
        [HttpPost]
        public IActionResult Create(VillaNumberVM obj)
        {
            
            bool roomNumberExists = _unitOfWork.VillaNumber.Any(u => u.Villa_Number == obj.VillaNumber.Villa_Number);

            if (ModelState.IsValid && !roomNumberExists)
            {
               _unitOfWork.VillaNumber.Add(obj.VillaNumber);
                _unitOfWork.Save();
                TempData["success"] = "The villa Number has been created succesfully";

                //return RedirectToAction(nameof(Index));
                return RedirectToAction(nameof(Index));
            }
            if (roomNumberExists)
            {
                TempData["error"] = "Villa Number already exists";
            }
            obj.VillaList = _unitOfWork.Villa.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            return View(obj); 
        }
        public IActionResult Update(int villaNumberId)
        {
            VillaNumberVM villaNumberVM = new()
            {
              
                VillaList = _unitOfWork.Villa.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                VillaNumber = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaNumberId)
            };
            if (villaNumberVM.VillaNumber==null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villaNumberVM);
        }
           [HttpPost]
            public IActionResult Update(VillaNumberVM villaNumberVM)
            {
            //bool roomNumberExists = _context.VillaNumbers.Any(u => u.Villa_Number == obj.VillaNumber!.Villa_Number);

            if (ModelState.IsValid)
            {
                _unitOfWork.VillaNumber.Update(villaNumberVM.VillaNumber);
                _unitOfWork.Save();
                TempData["success"] = "The villa Number has been updated succesfully";

                //return RedirectToAction(nameof(Index));
                return RedirectToAction(nameof(Index));
            }
          
         
            villaNumberVM.VillaList = _unitOfWork.Villa.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            return View(villaNumberVM);


           
               
            }
         public IActionResult Delete(int villaNumberId)
          {
              VillaNumberVM villaNumberVM = new()
          {

              VillaList = _unitOfWork.Villa.GetAll().Select(i => new SelectListItem
              {
                  Text = i.Name,
                  Value = i.Id.ToString()
              }),
              VillaNumber = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaNumberId)
          };
          if (villaNumberVM.VillaNumber==null)
          {
              return RedirectToAction("Error", "Home");
          }
          return View(villaNumberVM);
          }
           [HttpPost]
            public IActionResult Delete(VillaNumberVM villaNumberVM)
            {

                VillaNumber? objFromDb = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);
                if (objFromDb is not null)
                {
                    _unitOfWork.VillaNumber.Delete(objFromDb);
                    _unitOfWork.Save();
                    TempData["success"] = "The villa Number has been succesfully deleted";
                    //return RedirectToAction(nameof(Index));
                    return RedirectToAction(nameof(Index));
                }
                TempData["error"] = "The villa could not  be deleted";

                 return View();
            }
    }
}
