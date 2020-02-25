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
        public ActionResult Index()
        {
            int id = Int32.Parse(Session["person_id"].ToString());
            Models.Profile theClient = db.Profiles.SingleOrDefault(c => c.person_id == id);
            return View(theClient);
        }

        public ActionResult setProfile(int id ) {
            Models.Picture thePicture = db.Pictures.SingleOrDefault(c => c.picture_id == id);
            Models.Profile theClient = db.Profiles.SingleOrDefault(c => c.person_id == thePicture.person_id);
            theClient.profile_pic = thePicture.path;

            db.SaveChanges();

            return View(theClient);
        }

        public ActionResult Search(String name)
        {
            IEnumerable < Models.Profile > result = db.Profiles.Where(p => (p.first_name + " " + p.last_name).Contains(name));
            return View("Index", result);
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
                newUser.person_id = newClient.person_id;

                db.Profiles.Add(newClient);
                db.SaveChanges();

                Session["person_id"] = newUser.person_id;

                return RedirectToAction("Index");
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
