using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Drive.Models;

namespace Drive.Controllers
{
    public class HandelController : Controller
    {
        public User ux
        {
            set
            {
                Session["id"] = value;                // Global Sessions 
            }
            get
            {                                     // Object of User Table
                User user = new User();
                user = Session["id"] as User;              // Store Session["id"] in User 
                return user;
            }
        }
        public int achivedLocalId
        {
            get
            {
                int id = 0;
                User u = ux;
                if(u!=null)
                {
                    id = u.userId;                  //  id  store in user tabel
                }
                return id;
            }
        }

        public Post_Tbl us
        {
            set
            {
                Session["cat"] = value;
            }
            get
            {
                Post_Tbl a = new Post_Tbl();
                a = Session["cat"] as Post_Tbl;
                return a;
            }
        }
        public int achivedLoad
        {
            get
            {
                int i = 0;
                Post_Tbl p = us;
                if (us != null)
                {
                    i = us.PostId;
                }
                return i;
            }
        }

    }
}