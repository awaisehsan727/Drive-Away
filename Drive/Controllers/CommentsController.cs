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
    public class CommentsController : Controller
    {
        private Final_yearEntities db = new Final_yearEntities();
        User u = new User();
        ContactUs c = new ContactUs();

        // GET: Comments
        [HttpGet]
        public ActionResult Index(int? page, string Search)
        {
            if(Session["id"] !=null)
            {
                u = db.Users.Find(Session["id"]);
                if (u.RollId == 1 )
                {
                    if(Search != null)
                    {
                        return View(db.ContactUs1.Where(x => x.Name.Contains(Search)).ToList().ToPagedList(page ?? 1, 1));

                    }
                    return View(db.ContactUs1.ToList().ToPagedList(page ?? 1, 3));
                }
            }
            return RedirectToAction("Home","Drive");
        }

        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["id"] != null)
            {
                u = db.Users.Find(Session["id"]);
                if (u.RollId == 1)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    ContactUs ContactU = db.ContactUs1.Find(id);
                    if (ContactU == null)
                    {
                        return HttpNotFound();
                    }
                    return View(ContactU);
                }
                return View("Index");
            }
            return RedirectToAction("Home", "Drive");
        }
        public ActionResult ContactUs()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ContactUs(string name, string email, string Comment)
        {
            if (ModelState.IsValid)                                     // Post Method 
            {
                ContactUs c = new ContactUs();                      // Object of Comment Is use to store Comment in Database
                c.Name = name;
                c.Email = email;
                c.Message = Comment;
                db.ContactUs1.Add(c);
                db.SaveChanges();
                                                  // Save In Database


                MailMessage mm = new MailMessage("driveaway143@gmail.com", c.Email);
                mm.Subject = "Drive Away --- Welcome";
                mm.Body = "Dear Visitor \n Your Message has been recieved.You will be reply Shortly";  // Email Send to The Uer Who 
                mm.IsBodyHtml = false;                                                                  //contact Us

                SmtpClient smptp = new SmtpClient(); // Simple mail Transport Protocol Use to Communicate The Client Between Server
                smptp.Host = "smtp.gmail.com";
                smptp.Port = 587;
                smptp.EnableSsl = true;

                NetworkCredential nc = new NetworkCredential("driveaway143@gmail.com", "smartphone");
                smptp.UseDefaultCredentials = true;
                smptp.Credentials = nc;
                smptp.Send(mm);


            }
            return RedirectToAction("Home", "Drive");

        }
        public ActionResult Reply(string reply, int? id)
        {
            c = db.ContactUs1.Find(id);     //find those person id from table and we will send reply to him.   
            MailMessage mm = new MailMessage("driveaway143@gmail.com", c.Email); //Send Reply to the user who contact our website
            mm.Subject = "Drive Away --- Welcome";
            mm.Body =reply;
            mm.IsBodyHtml = false;

            SmtpClient smptp = new SmtpClient(); // Simple mail Transport Protocol Use to Communicate The Client Between Server
            smptp.Host = "smtp.gmail.com";
            smptp.Port = 587;
            smptp.EnableSsl = true;

            NetworkCredential nc = new NetworkCredential("driveaway143@gmail.com", "smartphone");
            smptp.UseDefaultCredentials = true;
            smptp.Credentials = nc;
            smptp.Send(mm);

            return RedirectToAction("Home", "Drive");
        }

        // GET: Comments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["id"] != null)
            {
                u = db.Users.Find(Session["id"]);
                if (u.RollId == 1 )
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    ContactUs ContactU = db.ContactUs1.Find(id);
                    if (ContactU == null)
                    {
                        return HttpNotFound();
                    }
                    return View(ContactU);
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Drive","Home");
        }
           

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Email,Message")] ContactUs ContactU)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ContactU).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ContactU);
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["id"] != null)
            {
                u = db.Users.Find(Session["id"]);
                if (u.RollId == 1)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    ContactUs ContactU = db.ContactUs1.Find(id);
                    if (ContactU == null)
                    {
                        return HttpNotFound();
                    }
                    return View(ContactU);
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Home", "Drive");
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ContactUs ContactU = db.ContactUs1.Find(id);
            db.ContactUs1.Remove(ContactU);
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
