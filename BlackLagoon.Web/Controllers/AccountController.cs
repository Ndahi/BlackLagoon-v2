using BlackLagoon.Application.Common.Interfaces;
using BlackLagoon.Application.Common.Utility;
using BlackLagoon.Domain.Entities;
using BlackLagoon.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace BlackLagoon.Web.Controllers
{

    public class AccountController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;

        }

        public IActionResult Login(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            LoginVM loginVM = new()
            {
                RedirectUrl = returnUrl
            };
            return View(loginVM);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.
                    PasswordSignInAsync(loginVM.Email, loginVM.Password, loginVM.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {

                    if (string.IsNullOrEmpty(loginVM.RedirectUrl))
                    {
                        return RedirectToAction("Index", "Home");

                    }
                    else
                    {
                        // Prevent open redirect attacks by ensuring the return URL is local
                        return LocalRedirect(loginVM.RedirectUrl);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                }
            }

            return View(loginVM);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Register(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            //Dotnet version 10 and above support async Main method
            //_roleManager.CreateAsync(new IdentityRole("Admin")).GetAwaiter().GetResult();
            if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).Wait();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).Wait();
            }
            /*
                        RegisterVM registerVM = new()
                        {
                            RoleList = _roleManager.Roles.Select(r => r.Name).Select(r => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                            {
                                Text = r,
                                Value = r
                            })
                        };*/
            RegisterVM registerVM = new()
            {
                RoleList = _roleManager.Roles.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Name
                })
            };
            return View(registerVM);
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new()
                {
                    Name = registerVM.Name,
                    Email = registerVM.Email,
                    PhoneNumber = registerVM.PhoneNumber,
                    NormalizedEmail = registerVM.Email.ToUpper(),
                    EmailConfirmed = true,
                    UserName = registerVM.Email,
                    CreatedAt = DateTime.Now
                };

                var result = await _userManager.CreateAsync(user, registerVM.Password);

                if (result.Succeeded)
                {
                    if (string.IsNullOrEmpty(registerVM.Role))
                    {
                        await _userManager.AddToRoleAsync(user, SD.Role_Customer);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, registerVM.Role);
                    }
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    /* if (!string.IsNullOrEmpty(registerVM.ReturnUrl) && Url.IsLocalUrl(registerVM.ReturnUrl))
                    {
                        return Redirect(registerVM.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }*/

                    if (string.IsNullOrEmpty(registerVM.ReturnUrl))
                    {
                        return RedirectToAction("Index", "Home");

                    }
                    else
                    {
                        // Prevent open redirect attacks by ensuring the return URL is local
                        return LocalRedirect(registerVM.ReturnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            registerVM.RoleList = _roleManager.Roles.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Name
            });

            return View(registerVM);
        }

    }
}
