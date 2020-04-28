using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Drive.Models;
using System.Net.Mail;
using PagedList;
using PagedList.Mvc;

namespace Drive.Controllers
{
    public class BookingsController : HandelController
    {
        private Final_yearEntities db = new Final_yearEntities();
        Booking b = new Booking();
        User u = new User();
        Post_Tbl p = new Post_Tbl();

        // GET: Bookings
        public ActionResult Index(int? page)
        {
            if (Session["id"] != null)
             u = db.Users.Find(Session["id"]);
            if(u.RollId==1)
            {
                return View(db.Bookings.ToList().ToPagedList(page ?? 1, 1));
            }
            return RedirectToAction("Home", "Drive");
        }



        public ActionResult BookNow(int? id)
        {
            if (Session["id"] != null)
                u = db.Users.Find(Session["id"]);
            {
                p = db.Post_Tbl.Find(id);
                b.userId = u.userId;
                b.PostId = Convert.ToInt32(id);
                b.status = "Booked";
                p.Status = "Booked";
                db.Entry(p).State = EntityState.Modified;
                u.Status = "False";
                db.Entry(u).State = EntityState.Modified;
                db.Bookings.Add(b);
                db.SaveChanges();

                User a = new User();
                a = db.Users.Find(p.userId);
                MailMessage mm = new MailMessage("driveaway143@gmail.com", a.Email);
                mm.Subject = "Drive Away --- Welcome";
                mm.Body = "Dear_" + a.FirstName + ",\n Your Post is Booked By_"+ u.FirstName +"His email is "+ u.Email;
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

            return RedirectToAction("MyBook", "Drive");
        }
        public ActionResult Cancel(int? id)
        {
            if (Session["id"] != null)
                u = db.Users.Find(Session["id"]);
            {
                p = db.Post_Tbl.Find(id);
                p.Status = "Available";
                db.Entry(p).State = EntityState.Modified;
                b = db.Bookings.FirstOrDefault(x => x.PostId == id && x.status == "Booked");
                b.status = "Available";
                db.Entry(b).State = EntityState.Modified;
                u = db.Users.FirstOrDefault(x => x.Status == "False");
                u.Status = "True";
                db.Entry(u).State = EntityState.Modified;
                db.SaveChanges();

                User a = new User();
                a = db.Users.Find(p.userId);
                MailMessage mm = new MailMessage("driveaway143@gmail.com", a.Email);
                mm.Subject = "Drive Away --- Welcome";
                mm.Body = "Dear_" + a.FirstName + ",\n Your Post ic Cancel By_"+u.FirstName + "His email is " + u.Email;
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
            return RedirectToAction("Blog", "Drive");

        }
        public ActionResult Done(int? id)
        {
            if (Session["id"] != null)
                u = db.Users.Find(Session["id"]);
            {
                p = db.Post_Tbl.Find(id);
                p.Status = "Done";
                db.Entry(p).State = EntityState.Modified;
                b = db.Bookings.FirstOrDefault(x => x.PostId == id && x.status == "Booked");
                b.status = "Done";
                db.Entry(b).State = EntityState.Modified;
                u = db.Users.FirstOrDefault(x => x.Status == "False");
                u.Status = "True";
                db.Entry(u).State = EntityState.Modified;
                db.SaveChanges();

                User a = new User();
                a = db.Users.Find(p.userId);
                MailMessage mm = new MailMessage("driveaway143@gmail.com", a.Email);
                mm.Subject = "Drive Away --- Welcome";
                mm.Body = "Dear_" + a.FirstName + ",\n Your Post is Done By_"+u.FirstName + "His email is " + u.Email;
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
            return RedirectToAction("Blog", "Drive");

        }

        // GET: Bookings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cancel([Bind(Include = "BookId,userId,PostId,status")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(booking);
        }
        public ActionResult Delete(int? id)
        {
            if (Session["id"] != null)
            {
                u = db.Users.Find(Session["id"]);
                if (u.RollId == 1)

                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                Booking booking = db.Bookings.Find(id);
                if (booking == null)
                {
                    return HttpNotFound();
                }
                return View(booking);
            }
            return RedirectToAction("Home", "Drive");
        }

        // POST: Bookings1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking booking = db.Bookings.Find(id);
            db.Bookings.Remove(booking);
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
