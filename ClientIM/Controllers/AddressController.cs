using ClientIM.ActionFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientIM.Controllers
{
    [LoginFilter]
    public class AddressController : Controller
    {
        Models.ClientEntities db = new Models.ClientEntities();
        // GET: Address
        [AddressFilter]
        public ActionResult Index(int id)
        {
            ViewBag.id = id;
            Session["Address_person_id"] = id;
            Models.Profile theClient = db.Profiles.SingleOrDefault(c => c.person_id == id);

            return View(theClient);
        }

        // GET: Address/Details/5
        public ActionResult Details(int id)
        {
            Models.Address theAddress = db.Addresses.SingleOrDefault(c => c.address_id == id);
            return View(theAddress);
        }

        // GET: Address/Create
        public ActionResult Create(int id)
        {
            ViewBag.id = id;

            ViewBag.countries =
               db.Countries.Select(c => new SelectListItem() { Value = c.country_id.ToString(), Text = c.country_name });

            return View();
        }

        // POST: Address/Create
        [HttpPost]
        public ActionResult Create(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                Models.Address newAddress = new Models.Address()
                {
                    person_id = id,
                    city = collection["city"],
                    desc = collection["desc"],
                    state = collection["state"],
                    street = collection["street"],
                    zip_code = collection["zip_code"],
                    country_id = Int32.Parse(collection["country_id"])
                };

                db.Addresses.Add(newAddress);
                db.SaveChanges();

                return RedirectToAction("Index", new { id = id });
            }
            catch
            {
                return View();
            }
        }

        // GET: Address/Edit/5
        public ActionResult Edit(int id)
        { 

            ViewBag.countries =
               db.Countries.Select(c => new SelectListItem() { Value = c.country_id.ToString(), Text = c.country_name });

            Models.Address theAddress = db.Addresses.SingleOrDefault(c => c.address_id == id);

            return View(theAddress);
        }

        // POST: Address/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                Models.Address theAddress = db.Addresses.SingleOrDefault(c => c.address_id == id);
                theAddress.city = collection["city"];
                theAddress.desc = collection["desc"];
                theAddress.state = collection["state"];
                theAddress.street = collection["street"];
                theAddress.zip_code = collection["zip_code"];
                theAddress.country_id = Int32.Parse(collection["country_id"]);

                db.SaveChanges();

                return RedirectToAction("Index", new {id = theAddress.person_id});
            }
            catch
            {
                return View();
            }
        }

        // GET: Address/Delete/5
        public ActionResult Delete(int id)
        {
            Models.Address theAddress = db.Addresses.SingleOrDefault(c => c.address_id == id);

            return View(theAddress);
        }

        // POST: Address/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                Models.Address theAddress = db.Addresses.SingleOrDefault(c => c.address_id == id);

                db.Addresses.Remove(theAddress);
                db.SaveChanges();

                return RedirectToAction("Index", new { id = theAddress.person_id });
            }
            catch
            {
                return View();
            }
        }
    }
}
