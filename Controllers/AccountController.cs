using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projectX.Models;
using System.Security.Claims;

namespace projectX.Controllers
{
    public class AccountController : Controller
    {
        private entitiesDbContext db;

        public AccountController(entitiesDbContext _db)
        {
            db = _db;
        }
        //=================      LOGIN     =======================================
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login (LoginViewModel loginViewModel)
        {
            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                // Check if the user exists in the database
                var user = await db.Users
               .Include (u => u.UserRoles)
               .ThenInclude (ur => ur.Role)
               //returns the first element that matches the specified condition or a default value if no such element is found.
               .FirstOrDefaultAsync (u => u.Email == loginViewModel.UserName);
                // Check if the user exists and the password is correct
                if (user == null || user.Password != loginViewModel.Password)
                {
                    ModelState.AddModelError ("", "Invalid username or password");
                    return View (loginViewModel);
                }
                // Create claims for the user
                //Claims are used in authentication and authorization processes to store and transmit user information securely
                var claims = new List<Claim>
            {
                    //reates a new claim with the type ClaimTypes.Name and assigns the user's email (user.Email) as the value.
                new Claim(ClaimTypes.Name, user.Email),
                // type ClaimTypes.NameIdentifier and assigns the user's ID (user.Id) as the value.
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("FullName", user.Name)

            };

                // Add role claims
                foreach (var userRole in user.UserRoles)
                {
                    claims.Add (new Claim (ClaimTypes.Role, userRole.Role.Name));
                }
                // Create claims identity
                //ClaimsIdentity represents a single identity of a user, which can contain multiple claims.
                //CookieAuthenticationDefaults.AuthenticationScheme: Specifies the authentication scheme, which in this case is cookie-based authentication.
                var claimsIdentity = new ClaimsIdentity (
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);
                // Create authentication properties
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes (30)
                };
                // Sign in the user   using cookie-based authentication 
                await HttpContext.SignInAsync (
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal (claimsIdentity),
                    authProperties);

                return RedirectToAction ("Index", "Home");
            }
            else
            {
                return View (loginViewModel);
            }
   }
        //=================      LOGOUT     =======================================
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout ()
        {
            // Sign out the user using cookie-based authentication
            await HttpContext.SignOutAsync (CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction ("Index", "Home");
        }

        [AllowAnonymous] // Use the custom attribute we created earlier
        [HttpGet]
        public IActionResult LogoutView ()
        {
            return View ();
        }
        //=================      REGISTER     =======================================
        public IActionResult Register ()
        {
            ViewBag.roles = db.Roles.ToList ();
            return View ();
  
        }

        [HttpPost]
        public async Task<IActionResult> Register (RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = db.Roles.ToList ();
                return View (model);
            }

            ViewBag.Roles = db.Roles.ToList ();

            // Check if the email already exists
            if (await db.Users.AnyAsync (u => u.Email == model.Email))
            {
                ModelState.AddModelError ("Email", "Email already registered");
                ViewBag.Roles = db.Roles.ToList ();
                return View (model);
            }

            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password,
                Age = model.Age,
            };

            db.Users.Add (user);
            await db.SaveChangesAsync ();

            // Assign selected role
            if (!string.IsNullOrEmpty (model.Role))
            {
                var role = await db.Roles.FirstOrDefaultAsync (r => r.Name == model.Role);
                if (role != null)
                {
                    var userRole = new UserRole { UserId = user.Id, RoleId = role.Id };
                    db.RoleUsers.Add (userRole);
                    await db.SaveChangesAsync ();
                }
            }

            // Automatically log in after registration
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Email),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim("FullName", user.Name)
    };

            if (!string.IsNullOrEmpty (model.Role))
            {
                claims.Add (new Claim (ClaimTypes.Role, model.Role));
            }

            await HttpContext.SignInAsync (
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal (new ClaimsIdentity (claims, CookieAuthenticationDefaults.AuthenticationScheme))
            );

            return RedirectToAction ("Index", "Home");
        }

        //=================      PROFILE     =======================================

        [Authorize]
        public async Task<IActionResult> Profile ()
        {
            // Get the user ID from the claims
            var userId = int.Parse (User.FindFirstValue (ClaimTypes.NameIdentifier));
            var user = await db.Users
                .Include (u => u.UserRoles)
                .ThenInclude (ur => ur.Role)
                .FirstOrDefaultAsync (u => u.Id == userId);

            if (user == null)
            {
                return NotFound ();
            }

            return View (user);
        }
    }
    }

