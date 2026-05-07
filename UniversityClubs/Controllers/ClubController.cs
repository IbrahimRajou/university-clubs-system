using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UniversityClubs.Models;
using System.Data.Entity;


namespace UniversityClubs.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ClubController : Controller
    {

        UniClubsDBEntities db = new UniClubsDBEntities();

        // To return a view with all the clubs stored in DB
        // GET: Club
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(db.Clubs.ToList());
        }

        // To show more details about the club
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return Content("Invalid Request");
            }
            Club club = db.Clubs.Find(id.Value);
            return View(club);
        }

        // To search about a club using its category
        [AllowAnonymous]
        public ActionResult SearchByCategory(int? id)
        {
            if (id == null)
            {
                return Content("Invalid Request");
            }
            var clubs = db.Clubs.Where(x => x.Category.Equals(id.Value)).ToList();
            return View("Index", clubs);

        }

        // Search in the search box for a club by its name
        [AllowAnonymous]
        public ActionResult SearchByName(string SearchValue)
        {
            if (string.IsNullOrEmpty(SearchValue))
            {
                return RedirectToAction("Index");
            }
            var clubs = db.Clubs.Where(x => x.Name.Contains(SearchValue)).ToList();
            return View("Index", clubs);
        }

        [AllowAnonymous]
        // To open the view where you can create a new club
        public ActionResult CreateClub()
        {
            ViewBag.CategoriesList = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        // To save the info of the new club to the DB
        [AllowAnonymous]
        [HttpPost]
        public ActionResult CreateClub(Club club, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    file.SaveAs(Server.MapPath("~/Images/" + file.FileName));
                    club.LogoPath = file.FileName;
                }
                club.Email = club.Name + "@contact.com";
                club.CreatedAt = DateTime.Now.ToString("dd-MM-yyyy");
                db.Clubs.Add(club);
                db.SaveChanges();
                TempData["ClubHasBeenCreated"] = "true";
                return RedirectToAction("CreateClub");
            }
            ViewBag.CategoriesList = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        // To delete a club by its unique ID
        public ActionResult DeleteClub(int? id)
        {
            if (id == null)
            {
                return Content("Invalid Request");
            }

            Club club = db.Clubs.Find(id.Value);

            if (club == null)
            {
                return Content("Invalid Request");
            }
            db.Clubs.Remove(club);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // To edit an existing club by getting the club info 
        // from the DB and returning a view with the current club data
        public ActionResult EditClub(int? id)
        {
            if (id == null)
            {
                return Content("Invalid Request");
            }
            Club club = db.Clubs.Find(id.Value);

            if (club == null)
            {
                return Content("Invalid Request");
            }
            ViewBag.CategoriesList = new SelectList(db.Categories, "Id", "Name");
            return View(club);
        }

        // To reflect (save) your modifications of the club to DB
        [HttpPost]
        public ActionResult EditClub(Club club, HttpPostedFileBase file)
        {
            if(ModelState.IsValid)
            {
                UniClubsDBEntities Tempdb = new UniClubsDBEntities();
                Club oldClub = Tempdb.Clubs.Find(club.Id);

                if(file != null)
                {
                    file.SaveAs(Server.MapPath("~/Images/" + file.FileName));
                    club.LogoPath = file.FileName;

                    if(oldClub.LogoPath != null)
                    {
                        System.IO.File.Delete(Server.MapPath("~/Images/" + oldClub.LogoPath));
                    }
                }
                else
                {
                    club.LogoPath = oldClub.LogoPath;
                }
                club.Email = oldClub.Email;
                club.CreatedAt = oldClub.CreatedAt;
                db.Entry(club).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoriesList = new SelectList(db.Categories, "Id", "Name");
            return View();
        }
    }
}