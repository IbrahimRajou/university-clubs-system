using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UniversityClubs.Models;

namespace UniversityClubs.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EventsController : Controller
    {
        UniClubsDBEntities db = new UniClubsDBEntities();

        // return view with all the events
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(db.Events.ToList());
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult SearchByClubName(String clubName)
        {
            var events = db.Events.Where(e => e.Club.Name.Contains(clubName)).ToList();
            return View("Index", events);
        }

        // To show the details of an event
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return Content("Invalid Request");
            }
            Event eve = db.Events.Find(id.Value); 
            return View(eve);
        }

        [AllowAnonymous]
        // To create a new event
        public ActionResult CreateEvent()
        {
            ViewBag.ClubId = new SelectList(db.Clubs, "Id", "Name");
            return View();
        }

        // To add the event to DB
        [AllowAnonymous]
        [HttpPost]
        public ActionResult CreateEvent(Event eve, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    file.SaveAs(Server.MapPath("~/Images/" + file.FileName));
                    eve.ImagePath = file.FileName;
                }
                db.Events.Add(eve);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClubId = new SelectList(db.Clubs, "Id", "Name");
            return View();
        }

        // To edit and event using its ID
        public ActionResult EditEvent(int? id)
        {
            if (id == null)
            {
                return Content("Invalid Request");
            }
            Event eve = db.Events.Find(id.Value);

            if (eve == null)
            {
                return Content("Invalid Request");
            }
            ViewBag.ClubId = new SelectList(db.Clubs, "Id", "Name");
            return View(eve);
        }

        // To reflect the edit process to the DB
        [HttpPost]
        public ActionResult EditEvent(Event eve, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                UniClubsDBEntities Tempdb = new UniClubsDBEntities();
                Event oldEve = Tempdb.Events.Find(eve.Id);

                if (file != null)
                {
                    file.SaveAs(Server.MapPath("~/Images/" + file.FileName));
                    eve.ImagePath = file.FileName;

                    if (oldEve.ImagePath != null)
                    {
                        System.IO.File.Delete(Server.MapPath("~/Images/" + oldEve.ImagePath));
                    }
                }
                else
                {
                    eve.ImagePath = oldEve.ImagePath;
                }
                db.Entry(eve).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClubId = new SelectList(db.Clubs, "Id", "Name");
            return View();
        }

        // To delete an event using its ID
        public ActionResult DeleteEvent(int? id)
        {
            if (id == null)
            {
                return Content("Invalid Request");
            }

            Event eve = db.Events.Find(id.Value);

            if (eve == null)
            {
                return Content("Invalid Request");
            }
            db.Events.Remove(eve);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}