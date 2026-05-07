using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UniversityClubs.Models;
using static System.Web.Razor.Parser.SyntaxConstants;

namespace UniversityClubs.Controllers
{
    public class UserController : Controller
    {
        UniClubsDBEntities db = new UniClubsDBEntities();
        // GET: User
        public ActionResult Index(string email, string password)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
                {
                    var user = db.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
                    if (user == null)
                    {
                        return RedirectToAction("Index", "Account");
                    }

                    int id = user.UniversityId;

                    var isAdmin = db.ClubMemberships.Any(r => r.UniversityId == id && r.Role == "Admin");

                    var role = isAdmin ? "Admin" : "Student";
                    Session["Role"] = role;
                    Session["UserEmail"] = user.Email;

                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Index", "Account");
        }
    }
}
