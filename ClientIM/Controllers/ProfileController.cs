using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using ClientIM.ActionFilter;

namespace ClientIM.Controllers
{
    [LoginFilter]
    public class ProfileController : Controller
    {
        Models.ClientEntities db = new Models.ClientEntities();
        // GET: Home
        [NotifyFriendRequestFilter]
        public ActionResult Index()
        {
            int id = Int32.Parse(Session["person_id"].ToString());
            Models.Profile theClient = db.Profiles.SingleOrDefault(c => c.person_id == id);
            return View(theClient);
        }

        // GET: Home/Details/5
        public ActionResult Details(int id)
        {
            Models.Profile theClient = db.Profiles.SingleOrDefault(c => c.person_id == id);
            return View(theClient);
        }

        // GET: Home/Create
        public ActionResult Create()
        {
            return View();

        }

        // POST: Home/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here          
                int id = Int32.Parse(Session["user_id"].ToString());
                Models.Profile newClient = new Models.Profile();
                Models.User newUser = db.Users.SingleOrDefault(p => p.user_id == id);
                newClient.first_name = collection["first_name"];
                newClient.last_name = collection["last_name"];
                newClient.notes= collection["notes"];
                newClient.gender = collection["gender"];
                newClient.profile_pic = "default.jpg";
                newUser.person_id = newClient.person_id;
               

                Session["person_id"] = newUser.person_id;

                db.Profiles.Add(newClient);
                db.SaveChanges();

                Models.Profile theClient = db.Profiles.SingleOrDefault(c => c.person_id == id);

                return View("Index", theClient);
            }
            catch
            {
                return View();
            }
        }

        // GET: Home/Edit/5
        public ActionResult Edit(int id)
        {
            var aClient = db.Profiles.SingleOrDefault(c => c.person_id == id);
            return View(aClient);
        }

        // POST: Home/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                Models.Profile theClient = db.Profiles.SingleOrDefault(c => c.person_id == id);
                theClient.first_name = collection["first_name"];
                theClient.last_name = collection["last_name"];
                theClient.notes = collection["notes"];
                theClient.gender = collection["gender"];

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Home/Delete/5
        public ActionResult Delete(int id)
        {
            Models.Profile theClient = db.Profiles.SingleOrDefault(c => c.person_id == id);

            return View(theClient);
        }

        // POST: Home/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                Models.Profile theClient = db.Profiles.SingleOrDefault(c => c.person_id == id);
                
                db.Addresses.RemoveRange(theClient.Addresses);
                db.Contacts.RemoveRange(theClient.Contacts);
                db.Pictures.RemoveRange(theClient.Pictures);
                db.Profiles.Remove(theClient);
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
