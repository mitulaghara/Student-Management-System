using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using StudentManagementSystem.Filters;
using StudentManagementSystem.Models;
using StudentManagementSystem.Services;

namespace StudentManagementSystem.Controllers
{
    public class AuthController : Controller
    {
        private readonly MongoDbService _mongoDbService;

        public AuthController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(UserLoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool isAuthenticated = false;
            string displayName = model.UserName ?? "admin";

            try
            {
                var user = _mongoDbService.Users
                    .Find(u => u.UserName == model.UserName && u.Password == model.Password)
                    .FirstOrDefault();

                if (user != null)
                {
                    isAuthenticated = true;
                    displayName = user.UserName ?? model.UserName ?? "admin";
                }
            }
            catch (Exception)
            {
                // Fallback authentication
                if (model.UserName == "admin" && model.Password == "admin123")
                {
                    isAuthenticated = true;
                    displayName = "admin";
                }
            }

            if (isAuthenticated)
            {
                HttpContext.Session.SetString("UserID", model.UserName ?? "admin");
                HttpContext.Session.SetString("UserName", displayName);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid Username or Password.");
            return View(model);
        }

        [CheckAccess]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [CheckAccess]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string currentUsername = HttpContext.Session.GetString("UserID") ?? "admin";

            try
            {
                var user = _mongoDbService.Users
                    .Find(u => u.UserName == currentUsername && u.Password == model.CurrentPassword)
                    .FirstOrDefault();

                if (user != null)
                {
                    user.Password = model.NewPassword;
                    _mongoDbService.Users.ReplaceOne(u => u.UserName == currentUsername, user);
                    TempData["SuccessMessage"] = "Password updated successfully!";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("CurrentPassword", "Current password does not match our records.");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error updating password: " + ex.Message;
            }

            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
