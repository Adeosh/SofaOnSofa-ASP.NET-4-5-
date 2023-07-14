using SofaOnSofa.Domain.Entities;
using SofaOnSofa.Domain.Options;
using SofaOnSofa.Web.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SofaOnSofa.Web.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(User user)
        {
            using (DB db = new DB())
            {
                db.Users.Add(user);
                db.SaveChanges();
                ModelState.Clear();
            }
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login login)
        {
            using (DB db = new DB())
            {
                var user = db.Users.Where(x => x.Email == login.Email && x.Password == login.Password).FirstOrDefault();
                if (user != null)
                {
                    var ticket = new FormsAuthenticationTicket(login.Email, true, 3000);
                    string encrypt = FormsAuthentication.Encrypt(ticket);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypt);
                    cookie.Expires = DateTime.Now.AddHours(3000);
                    cookie.HttpOnly = true;
                    Response.Cookies.Add(cookie);
                    if (user.RoleId == 1)
                    {
                        return RedirectToAction("UserArea", "Home");
                    }
                    else
                    {
                        return RedirectToAction("AdminArea", "Home");
                    }
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}