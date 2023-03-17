using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Vinoteca.Models;
using Vinoteca.ViewModels;
using Vinoteca.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Vinoteca.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        //To use dependency injection for manage user
        private readonly ImagesController _imagesController;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context, ImagesController imagesController)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            _context = context;
            _imagesController = imagesController;
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await AuthenticationHttpContextExtensions.SignOutAsync(HttpContext, IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var userName = model.User;
                //Comprobamos si ha introducido email y buscamos nombre de usuario de ese email
                if (IsEmail(model.User))
                {
                    var user = await _userManager.FindByEmailAsync(model.User);
                    if (user != null)
                    {
                        userName = user.UserName;
                    }
                }

                var result = await _signInManager.PasswordSignInAsync(userName, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    if (returnUrl != null)
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Vinos");
                    }

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Nombre de usuario o contraseña no válidos.");
                    return View(model);
                }
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult NewUser()
        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewUser(NewUserViewModel newUser)
        {
            if (ModelState.IsValid)
            {
                var checkUser = await _userManager.FindByNameAsync(newUser.User);
                var checkEmail = await _userManager.FindByEmailAsync(newUser.Email);
                if (checkUser == null && checkEmail == null)
                {
                    

                    var user = new ApplicationUser
                    {
                        UserName = newUser.User,
                        Email = newUser.Email,
                        Nombre = newUser.Nombre,
                        Apellidos = newUser.Apellidos,
                        Direccion = newUser.Direccion,
                        PhoneNumber = newUser.PhoneNumber,
                    };
                    string imagePath="";
                    if (newUser.ImageProfile != null)
                    {
                        //TODO: complete with ChatGPT info and 
                        //https://learn.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads?view=aspnetcore-7.0
                        //This doesn't work with MVC
                        //https://learn.microsoft.com/en-us/aspnet/web-pages/overview/ui-layouts-and-themes/9-working-with-images

                        IActionResult resp = await _imagesController.UploadProfileImage(newUser.ImageProfile, user);
                        if (resp is OkObjectResult result)
                        {
                            imagePath = result.Value as string;
                        }
                    }

                    user.ProfilePicturePath = imagePath;

                    await _userManager.CreateAsync(user, newUser.Password);
                    await _userManager.AddToRoleAsync(user, "User");
                    return RedirectToAction("AccountDetails");
                }
                else if (checkUser != null && checkEmail != null)
                {
                    ModelState.AddModelError("User", "Ya existe un usuario con ese nombre");
                    ModelState.AddModelError("Email", "El correo ya está registrado");
                }
                else if (checkUser != null)
                {
                    ModelState.AddModelError("User", "Ya existe un usuario con ese nombre");
                }
                else
                {
                    ModelState.AddModelError("Email", "El correo ya está registrado");
                }
            }
            return View(newUser);
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User,Bodeguero, Admin")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }




        [Authorize(Roles = "User, Bodeguero, Admin")]
        public async Task<IActionResult> AccountDetails()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            //DO IT WITH VIEWMODEL
            ViewBag.ProfileImage = !String.IsNullOrEmpty(user.ProfilePicturePath) ? "/images" + user.ProfilePicturePath : "/images/Default_profile_image.png";
            //user.ProfilePicturePath != null || user.ProfilePicturePath != "" ? "/images" + user.ProfilePicturePath : "/images/Default_profile_image.png";

            return View(user);
        }

        //GET EDIT USER

        [HttpGet]
        public async Task<IActionResult> AccountEdit(string id)
        {
            var model = await _userManager.FindByIdAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            EditUserViewModel user = new EditUserViewModel();
            user.Email = model.Email;
            user.Id = model.Id;
            user.PhoneNumber = model.PhoneNumber;
            user.Nombre = model.Nombre;
            user.Apellidos = model.Apellidos;
            user.Direccion = model.Direccion;

            user.NombreBodega = model.NombreBodega;
            user.Cif = model.Cif;

            ViewBag.ProfileImage = !String.IsNullOrEmpty(model.ProfilePicturePath) ? "/images"+model.ProfilePicturePath : "/images/Default_profile_image.png";

            return View(user);

        }

        //POST EDIT USER
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User, Bodeguero, Admin")]
        public async Task<IActionResult> AccountEdit(string Id, [Bind("Id,Nombre,Apellidos,Email,Direccion,PhoneNumber,NombreBodega,Cif,ImageProfile")] EditUserViewModel user)
        {

            if (user.Id != Id)
            {
                return NotFound();
            }
            //TODO: MODELSTATE.IsValid return false by nickname that is null check newusercontroller


            if (ModelState.IsValid)
            {
                var checkEmail = await _userManager.FindByEmailAsync(user.Email);
                if (checkEmail == null || checkEmail.Email == user.Email)
                {
                    try
                    {
                        var oldUser = await _userManager.FindByIdAsync(user.Id);
                        string imagePath ="";
                        if (user.ImageProfile != null)
                        {
                            IActionResult resp = await _imagesController.UploadProfileImage(user.ImageProfile, oldUser);
                            //ImagesController image = new ImagesController(_userManager);
                            //IActionResult resp = await image.UploadProfileImage(user.ImageProfile);
                            if (resp is OkObjectResult result)
                            {
                                imagePath = result.Value as string;
                            }
                        }

                        
                        //oldUser.Id = user.Id;
                        
                        oldUser.Nombre = user.Nombre;
                        oldUser.Apellidos = user.Apellidos;
                        oldUser.Email = user.Email;
                        oldUser.Direccion = user.Direccion;
                        oldUser.PhoneNumber = user.PhoneNumber;
                        oldUser.NombreBodega = user.NombreBodega;
                        oldUser.Cif = user.Cif;
                        if(imagePath.Length >0)
                        oldUser.ProfilePicturePath = imagePath;
                        if(oldUser.Bodeguero)
                        {
                            oldUser.NombreBodega = user.NombreBodega;
                            oldUser.Cif = user.Cif;
                        }

                        await _userManager.UpdateAsync(oldUser);

                    }
                    catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException)
                    {
                        if (!UserExists(user.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(AccountDetails));
                }
                else
                {
                    ModelState.AddModelError("Email", "El correo ya está registrado");
                }

            }
            if (!ModelState.IsValid)
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
            }
            return View(user);
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Bodeguero()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return NotFound();
            }
            user.Bodeguero = true;
            await _userManager.UpdateAsync(user);
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Bodeguero(string Id, [Bind("Bodeguero, NombreBodega, Cif")] NewBodegueroViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                user.NombreBodega = model.NombreBodega;
                user.Cif = model.Cif;
                await _userManager.UpdateAsync(user);

                var result = await _userManager.AddToRoleAsync(user, "Bodeguero");
                await _userManager.UpdateSecurityStampAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Ha Habido un error, inténtelo más tarde.");
                    user.Bodeguero = false;
                    user.NombreBodega = null;
                    user.Cif = null;
                    await _userManager.UpdateAsync(user);
                    return View(model);
                }

                await _signInManager.RefreshSignInAsync(user);
                return RedirectToAction("Create", "Vinos");

            }
            return View(model);
        }


        public bool IsEmail(string email)
        {
            try
            {
                var mail = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        private bool UserExists(string Id)
        {
            return _userManager.Users.Any(u => u.Id == Id);
        }


        //// GET: AccountController
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //// GET: AccountController/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: AccountController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: AccountController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: AccountController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: AccountController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: AccountController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: AccountController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
