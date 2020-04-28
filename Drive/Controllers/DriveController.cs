using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Drive.Models;
using PagedList;
using PagedList.Mvc;

namespace Drive.Controllers
{
    public class DriveController : HandelController
    {
        Final_yearEntities db = new Final_yearEntities();
        User u = new User();
        Post_Tbl p = new Post_Tbl();
        Booking b = new Booking();
        public ActionResult Home()
        {
            return View();
        }
        public ActionResult About()
        {
            return View();

        }
        [HttpGet]
        public ActionResult Blog(int? id,string SearchBy, string Search ,string cat, int? page)
        {
            if (Session["id"] != null)
            {
                

                u = db.Users.Find(Session["id"]);   //for Admin
                if (u.RollId == 1 )
                {
                    if(id==1)
                    {
                        return View(db.Post_Tbl.Where(x => x.Category == "Passenger").ToList().ToPagedList(page ?? 1, 3));      // Find Passenger Post
                    }
                    if(id==2)
                    {
                        return View(db.Post_Tbl.Where(x => x.Category == "Driver").ToList().ToPagedList(page ?? 1, 3));        // Find Driver Post
                    }
                    if (SearchBy != null && Search != null && cat!=null)
                    {                                                                                // search To, From Category Post
                        return View(db.Post_Tbl.Where(x => x.From == SearchBy && x.To == Search && x.Category=="Driver" && x.Category==cat ).ToList().ToPagedList(page ?? 1, 3));   
                    }
                    return View(db.Post_Tbl.Where(x=> x.Status=="Available").ToList().ToPagedList(page ?? 1, 3));

                }
                else
                    if (u.RollId == 2)                 // For User
                {
                    if (id == 1)
                    {
                        return View(db.Post_Tbl.Where(x => x.Category == "Passenger").ToList().ToPagedList(page ?? 1, 3));       // Find Passenger Post
                    }
                    if (id == 2)
                    {
                        return View(db.Post_Tbl.Where(x => x.Category == "Driver").ToList().ToPagedList(page ?? 1, 3));        // Find Driver Post
                    }

                    if (SearchBy != null && Search != null && cat!=null)
                    {                                                                                // search To, From Category Post
                        return View(db.Post_Tbl.Where(x => x.From == SearchBy && x.To == Search && x.Category==cat && x.Status != "Booked" ).ToList().ToPagedList(page ?? 1, 3));
                    }
                    return View(db.Post_Tbl.Where(x => x.Status != "Booked").ToList().ToPagedList(page ?? 1, 3));

                }
            }
            return RedirectToAction("Data", "Users");
        }
        public ActionResult MyBook(int? page)    // All Booked Post Displayed
        {

            int a = Convert.ToInt32(Session["id"]);
            if (Session["id"] != null)
             u = db.Users.Find(Session["id"]);
             if(u.Status =="False" && u.RollId==2)               
            {
                    return View(db.Bookings.Where(x =>x.userId == a && x.status=="Booked" ).ToList());

                }
           
            if (u.Status == "True" && u.RollId==2)
            {
                return View(db.Bookings.Where(x => x.userId == a && x.status == "Done").ToList());

            }
            if(u.RollId==1 && u.Status=="True")
            {
                return View(db.Bookings.Where(x =>x.status == "Done").ToList());
            }

            return View("Home");
        }
        public ActionResult Services()
        {
            return View();

        }
        public ActionResult pro()
        {
            if(Session["id"]!=null)
            {
                return RedirectToAction("Details", "users", new { id = Session["id"] });      //  Detail Redirect to Users Detail
            }
            return View("Home");
        }
        public ActionResult hand(int? id)
        {
            if (Session["id"]!=null)
            {
                return RedirectToAction("Details", "Post", new { id = id});                 //  Detail Redirect to post Detail
            }
            return RedirectToAction("Home");
        }
      
        public ActionResult Booking(int? id)
        {
            if (Session["id"] != null)
            {
                return RedirectToAction("BookNow", "Bookings", new { id = id });                  // Redirect To Book Post
            }
            return View("Home");
        }
        public ActionResult cancling(int? id)
        {
            if (Session["id"] != null)
            {
                return RedirectToAction("Cancel", "Bookings", new { id = id });                  // Redirect To Book Post
            }
            return View("Home");
        }
        public ActionResult Done(int? id)
        {
            if (Session["id"] != null)
            {
                return RedirectToAction("Done", "Bookings", new { id = id });                  // Redirect To Book Post
            }
            return View("Home");
        }
        public ActionResult Pas(int? id)
        {
            if (Session["id"] != null)
            {
                return RedirectToAction("Blog", "Drive", new { id = id });                     // Find Passnger/ Driver
            }
            return View("Home");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
} 