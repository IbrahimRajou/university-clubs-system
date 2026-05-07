using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UniversityClubs.Models;

namespace UniversityClubs.Controllers
{
    public class AccountController : Controller
    {

        UniClubsDBEntities db = new UniClubsDBEntities();

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(User model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            //var idPart = model.Email.Split('@')[0];
            //string role = "";

            //switch (idPart.Length)
            //{
            //    case 6: role = "Admin"; break;
            //    case 10: role = "Student"; break;
            //    default:
            //        ModelState.AddModelError("", "Invalid ID length for role assignment.");
            //        return View("Index", model);
            //}


            var user = db.Users.FirstOrDefault(u => u.Email == model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Email does not exist.");
                return View("Index", model);
            }
            if (user.Password != model.Password)
            {
                ModelState.AddModelError("", "Incorrect password.");
                return View("Index", model);
            }


            return RedirectToAction("Authentication", "Admin", new { userId = user.UniversityId });

        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}