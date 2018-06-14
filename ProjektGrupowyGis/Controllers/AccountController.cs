using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjektGrupowyGis.Models;
using System.Threading.Tasks;
using ProjektGrupowyGis.DAL;
using System.Web.Security;
using ProjektGrupowyGis.API;

namespace ProjektGrupowyGis.Controllers
{
    public class AccountController : Controller
    {
        private AccountSqlExecutor _accountSqlExecutor;

        public AccountController()
        {
            _accountSqlExecutor = new AccountSqlExecutor();
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [AcceptVerbs("POST")]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid) {
                var result = _accountSqlExecutor.CheckIfLoginTaken(model.Login);
                if (result != null) {
                    ModelState.AddModelError("LoginTaken", "Login already taken.");
                }
                User user = new User { Login = model.Login, Name = model.Name, Email = model.Email, Password = model.Password, UserId = null };
                _accountSqlExecutor.AddUser(user);
                EmailService.SendWelcomeEmail(user.Email);
                return RedirectToAction("Login", "Account");
            }
            return View(model);
        }

        [AcceptVerbs("POST")]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _accountSqlExecutor.CheckPassword(model.Login, model.Password);
                if (result != null) {
                    FormsAuthentication.SetAuthCookie(result.Login, false);
                    return RedirectToAction("Index", "Map");
                }
                else {
                    ModelState.AddModelError("WrongLoginData", "Login or password is incorrect.");
                    return View(model);
                }
            }
            return View(model);
        }

        [AcceptVerbs("GET")]
        public ActionResult IsUserAdmin()
        {
            var user = HttpContext.User.Identity; 
            bool isAdmin = user.Name == "Admin" ? true : false;
            var response = new { response = isAdmin };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Map");
        }
    }
}