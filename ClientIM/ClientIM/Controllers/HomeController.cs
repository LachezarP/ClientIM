using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ClientIM.Controllers
{
    public class HomeController : Controller
    {
        Models.ClientEntities6 db = new Models.ClientEntities6();
        // GET: Home
        public ActionResult Index()
        {
            return View(db.Clients);
        }

        public ActionResult setProfile(int id ) {
            Models.Picture thePicture = db.Pictures.SingleOrDefault(c => c.picture_id == id);
            Models.Client theClient = db.Clients.SingleOrDefault(c => c.person_id == thePicture.person_id);
            theClient.profile_pic = thePicture.path;

            db.SaveChanges();

            return View("Index", db.Clients);
        }

        public ActionResult Search(String name)
        {
            IEnumerable < Models.Client > result = db.Clients.Where(p => (p.first_name + " " + p.last_name).Contains(name));
            return View("Index", result);
        }

        // GET: Home/Details/5
        public ActionResult Details(int id)
        {
            Models.Client theClient = db.Clients.SingleOrDefault(c => c.person_id == id);
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
                Models.Client newClient = new Models.Client();
                newClient.first_name = collection["first_name"];
                newClient.last_name = collection["last_name"];
                newClient.notes= collection["notes"];
                newClient.gender = collection["gender"];

                db.Clients.Add(newClient);
                db.SaveChanges();

                

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
            var aClient = db.Clients.SingleOrDefault(c => c.person_id == id);
            return View(aClient);
        }

        // POST: Home/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                Models.Client theClient = db.Clients.SingleOrDefault(c => c.person_id == id);
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
            Models.Client theClient = db.Clients.SingleOrDefault(c => c.person_id == id);

            return View(theClient);
        }

        // POST: Home/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                Models.Client theClient = db.Clients.SingleOrDefault(c => c.person_id == id);
                
                db.Addresses.RemoveRange(theClient.Addresses);
                db.Contacts.RemoveRange(theClient.Contacts);
                db.Pictures.RemoveRange(theClient.Pictures);
                db.Clients.Remove(theClient);
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
