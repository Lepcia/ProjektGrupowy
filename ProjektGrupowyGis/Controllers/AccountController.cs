using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Data.Helper;
using Data.Helper.External.Models;
using ProjektGrupowyGis.Models;
using System.Threading.Tasks;

namespace ProjektGrupowyGis.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        public AccountController()
        { }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid) {
                XMLReader _xmlReader = new XMLReader();
                XMLWriter _xmlWriter = new XMLWriter();

                var loginTaken = _xmlReader.CheckIfLoginTaken(model.Login);
                if (!loginTaken){
                    _xmlWriter.InsertUser(model.Login, model.Name, model.Email, model.Password);
                }
                else {
                    ModelState.AddModelError("LoginTaken", "Login already taken.");
                }
            }
            return View(model);
        }
    }
}