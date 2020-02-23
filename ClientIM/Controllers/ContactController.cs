using ClientIM.ActionFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientIM.Controllers
{
    [LoginFilter]
    public class ContactController : Controller
    {
        Models.ClientEntities db = new Models.ClientEntities();

        // GET: Contact
        public ActionResult Index(int id)
        {

            Models.Profile theClient = db.Profiles.SingleOrDefault(c => c.person_id == id);

            return View(theClient);
        }

        // GET: Contact/Details/5
        public ActionResult Details(int id)
        {
            ViewBag.id = id;

            Models.Contact theContact = db.Contacts.SingleOrDefault(c => c.contact_id == id);

            return View(theContact);
        }

        // GET: Contact/Create
        public ActionResult Create(int id)
        {
            ViewBag.id = id;

            return View();
        }

        // POST: Contact/Create
        [HttpPost]
        public ActionResult Create(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                Models.Contact newContact = new Models.Contact()
                {
                    person_id = id,
                    type_info = collection["type_info"],
                    info = collection["info"]
                };

                db.Contacts.Add(newContact);
                db.SaveChanges();

                return RedirectToAction("Index", new { id = id});
            }
            catch
            {
                return View();
            }
        }

        // GET: Contact/Edit/5
        public ActionResult Edit(int id)
        {
            Models.Contact theContact = db.Contacts.SingleOrDefault(c => c.contact_id == id);

            return View(theContact);
        }

        // POST: Contact/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                Models.Contact theContact = db.Contacts.SingleOrDefault(c => c.contact_id == id);

                theContact.type_info = collection["type_info"];
                theContact.info = collection["info"];

                db.SaveChanges();

                return RedirectToAction("Index", new { id = theContact.person_id });
            }
            catch
            {
                return View();
            }
        }

        // GET: Contact/Delete/5
        public ActionResult Delete(int id)
        {
            Models.Contact theContact = db.Contacts.SingleOrDefault(c => c.contact_id == id);

            return View(theContact);
        }

        // POST: Contact/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                Models.Contact theContact = db.Contacts.SingleOrDefault(c => c.contact_id == id);

                db.Contacts.Remove(theContact);
                db.SaveChanges();

                return RedirectToAction("Index", new { id = theContact.person_id});
            }
            catch
            {
                return View();
            }
        }
    }
}
