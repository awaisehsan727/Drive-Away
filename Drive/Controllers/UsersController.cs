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
    public class UsersController : HandelController
    {
        private Final_yearEntities db = new Final_yearEntities();
        User u = new User();
        public ActionResult logout()                   // Logout Method
        {
            if (Session["id"] != null)
            {
                Session["id"] = null;
                Session.Clear();                      // Clear Session
            }
            return RedirectToAction("Login");
        }
        public ActionResult Data()
        {

            ViewData["error"] = "First You Have to Login";
            return View("Login");
        }
        [HttpGet]
        public ActionResult Login()    // Login View
        {
            ViewData["error"] = "";
            return View();
        }
        [HttpPost]
        public ActionResult Login(User us)
        {
            u = db.Users.FirstOrDefault(row => row.Email == us.Email && row.password == us.password);      // Check from database user exists or not
            if (u != null)                                        //if condition u has some value or not 
            {
                Session["id"] = u.userId;                           // Global Session["id"] store userId
                if (u.RollId == 1)                                  // RollId == 1 is for Admin
                {
                    return RedirectToAction("Home", "Drive");      // 
                }
                else                                               // else part
                    if (u.RollId == 2)                              // RollId == 2 is for user
                    return RedirectToAction("Home", "Drive");
            }

            ViewData["Login"] = "Please Enter Correct Information";    // user is not exist in database then show error
            return View("Login");
        }



        [HttpGet]
        public ActionResult Index(string Search, int? page)
        {
            if (Session["id"] != null)
            {
                u = db.Users.Find(Session["id"]);   //for Admin
                if (u.RollId == 1)
                {
                    if (Search != null)
                    {
                        return View(db.Users.Where(x => x.FirstName.Contains(Search)).ToList().ToPagedList(page ?? 1, 3));    // Find Name from User_Table  
                    }

                    return View(db.Users.ToList().ToPagedList(page ?? 1, 3));                                // All Users Detail to list

                }
                else
                    if (u.RollId == 2)           // For User
                {
                    if (Search != null)
                    {
                        return View(db.Users.Where(x => x.FirstName == Search).ToList().ToPagedList(page ?? 1, 3));       // Find Name from User_Table  
                    }

                    return RedirectToAction("Home", "Drive");           // View Only User Profile

                }
            }
            return RedirectToAction("Home","Drive");
        }
        public ActionResult Details(int? id)
        {
            if (Session["id"] != null)
            {
                u = db.Users.Find(Session["id"]);
                if (u.RollId == 1 || u.userId == id)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    User user = db.Users.Find(id);
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
        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(string fname, string Lname, string password, string confirmpass, string Address, string CNIC, string email)
        {
            if (password != confirmpass)                               // Password Validation
            {
                ViewData["error"] = "Both Password Should Be Same";      // Error Show On Front End
                return View("Signup");                                // Redirect To View Signup If password != confirmpass
            }
            else
            {
                try
                {
                    User x = new User();
                    x = db.Users.First(row => row.Email == email);            // Check Email Exist then Show Massge Choose another Email
                    ViewData["error"] = "Choose Another Email Address";
                    return View("Signup");                                   // Redirect To View Signup If Email exits don't make dublicate account on same email
                }
                catch (Exception ex)
                {
                    try
                    {
                        User a = new User();
                        a = db.Users.First(row => row.CNIC == CNIC);            // Check Email Exist then Show Massge Choose another Email
                        ViewData["error"] = "Choose Another CNIC";             // View data is use to display the error on front end
                        return View("Signup");
                    }
                    catch (Exception e)
                    {
                        User Aduser = new User();                     // Object of User is use to store data in Database
                        Aduser.RollId = 2;
                        Aduser.FirstName = fname;
                        Aduser.LastName = Lname;
                        Aduser.password = password;
                        Aduser.Address = Address;
                        Aduser.CNIC = CNIC;
                        Aduser.Email = email;
                        Aduser.Status = "True";
                        db.Users.Add(Aduser);                      // All the data is Add in user Table 
                        db.SaveChanges();                          // Database update and save


                        MailMessage mm = new MailMessage("driveaway143@gmail.com", Aduser.Email);      // mailmessage is bulden libaray Who user is signup gets its email address
                        mm.Subject = "Drive Away --- Welcome";
                        mm.Body = "Dear " + Aduser.FirstName + ",\n Thank For Registerd on Drive Away! You can Contact Us For Any Help"; // Message send to the user who signUp
                        mm.IsBodyHtml = false;

                        SmtpClient smptp = new SmtpClient(); // Simple mail Transport protocol is use to communication between user and client
                        smptp.Host = "smtp.gmail.com";
                        smptp.Port = 587;
                        smptp.EnableSsl = true;

                        NetworkCredential nc = new NetworkCredential("driveaway143@gmail.com", "smartphone");
                        smptp.UseDefaultCredentials = true;
                        smptp.Credentials = nc;
                        smptp.Send(mm);               //mm is the object send to the mail address message by use the simple mail transport protocol
                    }
                }
            }
            return RedirectToAction("Login");
        }


        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["id"] != null)
            {
                u = db.Users.Find(Session["id"]);
                if (u.RollId == 1 || u.userId == id)     // Update Profile Users Him Selfi Admin Every One
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    User user = db.Users.Find(id);
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


        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userId,FirstName,LastName,password,Address,CNIC,Email,RollId,Status")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["id"] != null)
            {
                u = db.Users.Find(Session["id"]);
                if (u.RollId == 1 || u.userId == id)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    User user = db.Users.Find(id);
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

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
  
