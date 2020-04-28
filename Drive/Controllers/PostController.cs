using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Drive.Models;
using PagedList;
using PagedList.Mvc;
using System.Net.Mail;


namespace Drive.Controllers
{
    public class PostController : HandelController
    {
        User u = new User();
        Post_Tbl p = new Post_Tbl();

        private Final_yearEntities db = new Final_yearEntities();

        [HttpGet]
        public ActionResult Index(string SearchBy, string Search, string user, int? page,int? id )
        {
            if (Session["id"] != null)
            {
                u = db.Users.Find(Session["id"]);   //for Admin
                if (u.RollId == 1)
                {
                    if (id == 1)
                    {
                        return View(db.Post_Tbl.Where(x => x.Category == "Passenger").ToList().ToPagedList(page ?? 1, 1));      // Find Passenger Post
                    }
                    if (id == 2)
                    {
                        return View(db.Post_Tbl.Where(x => x.Category == "Driver").ToList().ToPagedList(page ?? 1, 1));        // Find Driver Post
                    }
                    
                    
                    if (SearchBy != null && Search != null && user != null)
                    {
                        return View(db.Post_Tbl.Where(x => x.From == SearchBy && x.To == Search && x.Category == user).ToList().ToPagedList(page ?? 1,1));
                    }

                    return View(db.Post_Tbl.Where(x => x.Status == "Available").ToList().ToPagedList(page ?? 1, 4));

                }
                else
                    if (u.RollId == 2)
                {
                    if (SearchBy != null && Search != null && user != null)
                    {
                        return View(db.Post_Tbl.Where(x => x.From == SearchBy && x.To == Search && x.Category == user && x.Status != "Booked").ToList());
                    }
                    return RedirectToAction("Blog", "Drive");

                }
            }
            return RedirectToAction("Home", "Drive");
        }


        // GET: Post/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["id"] != null)
            {
                u = db.Users.Find(Session["id"]);
                if (u.RollId == 1 || u.RollId==2)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Post_Tbl user = db.Post_Tbl.Find(id);
                    if (user == null)
                    {
                        return HttpNotFound();
                    }
                    return View(user);
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Home", "Drive");
        }
        // GET: Post/Create
        public ActionResult Create()
        {
            if(Session["id"] !=null)
            {
                return View();
            }
            return RedirectToAction("Data", "Users");
        }
        [HttpPost]
        public ActionResult Create(string To, String From, String Date, String Time, string Description, string Phone, string Seat, string Cat)
        {
            u=db.Users.Find(Session["id"]);
                if (ModelState.IsValid)                       // Add Post Method 
                {
                    Post_Tbl p = new Post_Tbl();              // Object of post_Tbl is use to store data in Database
                    p.userId = u.userId;
                    p.To = To;
                    p.From = From;
                    p.Date = Date;
                    p.Time = Time;
                    p.Description = Description;
                    p.PhoneNo = Phone;
                    p.Seats = Convert.ToInt32(Seat);
                    p.Category = Cat;
                    p.Status = "Available";
                    db.Post_Tbl.Add(p);
                    db.SaveChanges();                  // Save into Database


                MailMessage mm = new MailMessage("driveaway143@gmail.com", u.Email);
                mm.Subject = "Drive Away --- Welcome";
                mm.Body = "Dear " + u.FirstName + ",\n Thank For Creating The Ride , Wait for your Booking You Will be Inform Here";
                mm.IsBodyHtml = false;

                SmtpClient smptp = new SmtpClient();
                smptp.Host = "smtp.gmail.com";
                smptp.Port = 587;
                smptp.EnableSsl = true;

                NetworkCredential nc = new NetworkCredential("driveaway143@gmail.com", "smartphone");
                smptp.UseDefaultCredentials = true;
                smptp.Credentials = nc;
                smptp.Send(mm);
            }
                return RedirectToAction("Blog","Drive");
            }
         

    
  

        // GET: Post/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["id"] != null)
            {
                u = db.Users.Find(Session["id"]);
                p = db.Post_Tbl.Find(id);
                if (p.userId == u.userId )
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Post_Tbl Post_Tbl = db.Post_Tbl.Find(id);
                    if (Post_Tbl == null)
                    {
                        return HttpNotFound();
                    }
                    return View(Post_Tbl);
                }
                return RedirectToAction("Blog","Drive");
            } 
            return RedirectToAction("index");

        }

        // POST: Post/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PostId,userId,From,To,Date,Time,Description,PhoneNo,Seats,Category,Status")] Post_Tbl post_Tbl)
        {
            if (Session["id"] != null)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(post_Tbl).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(post_Tbl);
            }
            return RedirectToAction("Index");
        }

        // GET: Post/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["id"] != null)
            {
                u = db.Users.Find(Session["id"]);
                p = db.Post_Tbl.Find(id);
                if (p.userId == u.userId || u.RollId == 1)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Post_Tbl Post_Tbl = db.Post_Tbl.Find(id);
                    if (Post_Tbl == null)
                    {
                        return HttpNotFound();
                    }
                    return View(Post_Tbl);
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("index");

        }

        // POST: Post_Tbl/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post_Tbl P = db.Post_Tbl.Find(id);
            db.Post_Tbl.Remove(P);
            db.SaveChanges();
            return RedirectToAction("Index");
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
