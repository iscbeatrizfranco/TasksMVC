using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Security.Claims;
using TasksMVC.Models;
using TasksMVC.Services;

namespace TasksMVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly ApplicationDbContext dbContext;

        public UsersController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext dbContext)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.dbContext = dbContext;
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await userManager.CreateAsync(user, password: model.Password);

            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }
        }

        [AllowAnonymous]
        public IActionResult Login(string message = null)
        {
            if (message is not null)
            {
                ViewData["message"] = message;
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var resultado = await signInManager.PasswordSignInAsync(model.Email,
                model.Password, model.RememberMe, lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Nombre de usuario o password incorrectos.");
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        [HttpGet]
        public ChallengeResult ExternLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action("ExternalUserRegister", values: new { provider, returnUrl });
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, returnUrl);
            return new ChallengeResult(provider, properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalUserRegister(string returnUrl = null,
            string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            var message = "";
            if (remoteError != null)
            {
                message = $"Error del proveedor externo: {remoteError}";
                return RedirectToAction("login", routeValues: new { message });
            }

            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                message = "Error cargando la data de login externo";
                return RedirectToAction("login", routeValues: new { message });
            }

            var externalLoginResult = await signInManager.ExternalLoginSignInAsync(
                info.LoginProvider, info.ProviderKey, isPersistent: true, bypassTwoFactor: true);

            //Ya la cuenta existe
            if (externalLoginResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            //sino existe
            string email = "";
            if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
            {
                email = info.Principal.FindFirstValue(ClaimTypes.Email);
            }
            else
            {
                message = "Error leyendo el email del usuario del proveedor";
                return RedirectToAction("login", routeValues: new { message });
            }

            var user = new IdentityUser { Email = email, UserName = email };
            var userCreateResult = await userManager.CreateAsync(user);

            if (!userCreateResult.Succeeded)
            {
                message = userCreateResult.Errors.First().Description;
                return RedirectToAction("login", routeValues: new { message });
            }

            var loginAddResult = await userManager.AddLoginAsync(user, info);

            if (loginAddResult.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: true, info.LoginProvider);
                return LocalRedirect(returnUrl);
            }

            message = "Ha ocurrido un error agregando el login";
            return RedirectToAction("login", routeValues: new { message });

        }

        [HttpGet]
        [Authorize(Roles = Services.Constants.RolAdmin)]
        public async Task<IActionResult> List(string message = null)
        {
            var users = await dbContext.Users.Select(u => new UserViewModel
            {
                Email = u.Email,
            }).ToListAsync();

            var model = new UsersListViewModel();
            model.Users = users;
            model.Message = message;
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = Services.Constants.RolAdmin)]
        public async Task<IActionResult> AdminGrant(string email)
        {
            var user = await dbContext.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
            if (user is null)
            {
                return NotFound();
            }

            await userManager.AddToRoleAsync(user, Services.Constants.RolAdmin);

            return RedirectToAction("List",
                routeValues: new
                {
                    message = "Rol asignado correctamente a " + email
                });
        }

        [HttpPost]
        [Authorize(Roles = Services.Constants.RolAdmin)]
        public async Task<IActionResult> AdminRevoke(string email) 
        {
            var user =  await dbContext.Users.Where(u => u.Email == email).FirstOrDefaultAsync();

            if (user is null)
            {
                return NotFound();
            }

            await userManager.RemoveFromRoleAsync(user, Services.Constants.RolAdmin);

            return RedirectToAction("List",
                routeValues: new
                {
                    message = "Rol removido correctamente a " + email
                });
        }
    }
}
