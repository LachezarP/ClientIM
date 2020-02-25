using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientIM.Controllers
{
    public class FriendController : Controller
    {
        Models.ClientEntities db = new Models.ClientEntities();
        // GET: Friend
        public ActionResult Index()
        {
            return View(db.Profiles);
        }

        public ActionResult AllUsers()
        {
            return View(db.Profiles);
        }

        // GET: Friend/Details/5
        public ActionResult FriendLink()
        {
            return View(db.FriendLinks);
        }

        // GET: Friend/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Friend/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Friend/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Friend/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Friend/Delete/5
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

                return RedirectToAction("AllUsers", "Friend");
            }
            catch
            {
                return View();
            }
        }
    }
}
