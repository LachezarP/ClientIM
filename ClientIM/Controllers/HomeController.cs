using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace ClientIM.Controllers
{
    public class HomeController : Controller
    {
        Models.ClientEntities db = new Models.ClientEntities();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            string username = collection["username"];
            Models.User theUser = db.Users.SingleOrDefault(u => u.username.Equals(username));
            if (theUser != null && 
                Crypto.VerifyHashedPassword(theUser.password_hash, collection["password_hash"]))
            {
                Session["person_id"] = theUser.user_id;
                return RedirectToAction("Index", "Profile");
            }
            else
            {
                ViewBag.error = "Wrong Username/Password combination!";
                return View();
            }
        }

        // GET: Home/Details/5
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index");
        }

        private static Random random = new Random();
        public static string RandomBase32String(int lenght)
        {
            const string chars = "ACBCDEFGHIJKLMNOPQRSTUVWXYZ234567";
            return new string(Enumerable.Repeat(chars, lenght)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // GET: Home/Create
        public ActionResult Register()
        {
            

            return View();
        }

        // POST: Home/Create
        [HttpPost]
        public ActionResult Register(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                string username = collection["username"];
                Models.User theUser = db.Users.SingleOrDefault(u => u.username.Equals(username));

                if (theUser != null)
                    return RedirectToAction("Register");

                Models.User newUser = new Models.User()
                {
                    username = collection["username"],
                    password_hash = Crypto.HashPassword(collection["password_hash"])
                };
                db.Users.Add(newUser);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
