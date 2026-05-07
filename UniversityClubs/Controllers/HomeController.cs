using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UniversityClubs.Models;

namespace UniversityClubs.Controllers
{
    public class HomeController : Controller
    {
        UniClubsDBEntities db = new UniClubsDBEntities();
        // GET: Home
        public ActionResult Index()
        {
            // to open the home page
            return View(db.Events.ToList());
        }

    }
}