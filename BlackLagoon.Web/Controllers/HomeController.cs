using BlackLagoon.Application.Common.Interfaces;
using BlackLagoon.Web.Models;
using Microsoft.AspNetCore.Mvc;
using BlackLagoon.Web.ViewModels;

using System.Diagnostics;

namespace BlackLagoon.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IActionResult Index()
        {
            HomeVM homeVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll(IncludeProperties:"VillaAmenity"),
                Nights = 1,
                CheckInDate = DateOnly.FromDateTime(DateTime.Now),
            };


            return View(homeVM);
        }

        [HttpPost]
        public IActionResult Index(HomeVM homeVM)
        {
            homeVM.VillaList = _unitOfWork.Villa.GetAll(IncludeProperties: "VillaAmenity");
            foreach (var villa in homeVM.VillaList)
            {
                if(villa.Id % 2 == 0)
                {
                    villa.IsAvailable = false;
                }
            }


            return View(homeVM);
        }

        public IActionResult GetVillasByDate(int nights,DateOnly checkInDate)
        {
            Thread.Sleep(2000); // Simulate a delay for demonstration purposes
            var villaList = _unitOfWork.Villa.GetAll(IncludeProperties: "VillaAmenity").ToList();
            foreach (var villa in villaList)
            {
                if (villa.Id % 2 == 0)
                {
                    villa.IsAvailable = false;
                }

            }
            HomeVM homeVM = new()
            {
                VillaList = villaList,
                Nights = nights,
                CheckInDate = checkInDate,
            };
            return PartialView("_VillaList",homeVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
