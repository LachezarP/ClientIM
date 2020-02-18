using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientIM.Controllers
{
    public class PictureController : Controller
    {
        Models.ClientEntities6 db = new Models.ClientEntities6();
        // GET: Picture
        public ActionResult Index(int id)
        {
            ViewBag.id = id;

            Models.Client theClient = db.Clients.SingleOrDefault(c => c.person_id == id);

            return View(theClient);
        }

        // GET: Picture/Details/5
        public ActionResult Details(int id)
        {
            Models.Picture thePicture = db.Pictures.SingleOrDefault(c => c.picture_id == id);

            return View(thePicture);
        }

        // GET: Picture/Create
        public ActionResult Create(int id)
        {
            ViewBag.id = id;

            return View();
        }

        // POST: Picture/Create
        [HttpPost]
        public ActionResult Create(int id, FormCollection collection, HttpPostedFileBase newPicture)
        {
            try
            {
                // TODO: Add insert logic here

                var type = newPicture.ContentType;

                string[] acceptableTypes = { "image/jpeg", "image/gif", "image/png" };

                if (newPicture != null && newPicture.ContentLength > 0 && acceptableTypes.Contains(type)) {
                    Guid g = Guid.NewGuid();
                    String filename = g.ToString() + Path.GetExtension(newPicture.FileName);
                    String path = Path.Combine(Server.MapPath("~/Images/") + filename);
                    newPicture.SaveAs(path);

                    Models.Picture newPic = new Models.Picture()
                    {
                        caption = collection["caption"],
                        path = filename,
                        loc_info = collection["loc_info"],
                        person_id = id,
                        time_info = collection["time_info"]
                    };

                    db.Pictures.Add(newPic);
                    db.SaveChanges();
                }

                return RedirectToAction("Index", new { id = id });
            }
            catch
            {
                return View();
            }
        }

        // GET: Picture/Edit/5
        public ActionResult Edit(int id)
        {
            Models.Picture thePicture = db.Pictures.SingleOrDefault(c => c.picture_id == id);

            return View(thePicture);
        }

        // POST: Picture/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection, HttpPostedFileBase thePicture)
        {
            try
            {
                // TODO: Add update logic here
                var type = thePicture.ContentType;

                string[] acceptableTypes = { "image/jpeg", "image/gif", "image/png" };

                Models.Picture thePic = db.Pictures.SingleOrDefault(c => c.picture_id == id);

                if (thePicture != null && thePicture.ContentLength > 0 && acceptableTypes.Contains(type)) {
                    Guid g = Guid.NewGuid();
                    String filename = g.ToString() + Path.GetExtension(thePicture.FileName);
                    String path = Path.Combine(Server.MapPath("~/Images/") + filename);
                    thePicture.SaveAs(path);

                    thePic.caption = collection["caption"];
                    thePic.path = filename;
                    thePic.loc_info = collection["loc_info"];
                    thePic.time_info = collection["time_info"];

                    db.SaveChanges();
                }

                return RedirectToAction("Index", new { id = thePic.person_id });
            }
            catch
            {
                return View();
            }
        }

        // GET: Picture/Delete/5
        public ActionResult Delete(int id)
        {
            Models.Picture thePicture = db.Pictures.SingleOrDefault(c => c.picture_id == id);

            return View(thePicture);
        }

        // POST: Picture/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                Models.Picture thePicture = db.Pictures.SingleOrDefault(c => c.picture_id == id);

                Models.Client theClient = db.Clients.SingleOrDefault(c => c.person_id == thePicture.person_id);

                theClient.profile_pic = null;

                db.Pictures.Remove(thePicture);
                db.SaveChanges();

                return RedirectToAction("Index", new { id = thePicture.person_id });
            }
            catch
            {
                return View();
            }
        }


    }
}
