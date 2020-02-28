using ClientIM.ActionFilter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientIM.Controllers
{
    [LoginFilter]
    public class PictureController : Controller
    {
        Models.ClientEntities db = new Models.ClientEntities();
        // GET: Picture
        public ActionResult Index(int id)
        {
            ViewBag.id = id;

            Models.Profile theClient = db.Profiles.SingleOrDefault(c => c.person_id == id);

            int personId = Int32.Parse(Session["person_id"].ToString());

            if (id == personId)
            {
                int counter = 0;

                int counterLike = Int32.Parse(Session["new_Likes"].ToString());

                IEnumerable<Models.Picture> yourPics = db.Pictures.Where(c => c.person_id == personId);

                foreach (Models.Like ML in db.Likes.ToList())
                {
                    if (yourPics.Contains(db.Pictures.SingleOrDefault(c => c.picture_id == ML.picture_id)))
                    {
                        if (ML.read == "Not read")
                            counter++;
                        ML.read = "Read";
                    }
                }
                db.SaveChanges();
                counterLike -= counter;
                Session["new_Likes"] = counterLike;
            }
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
        public ActionResult Create( FormCollection collection, HttpPostedFileBase newPicture)
        {
            try
            {
                // TODO: Add insert logic here
                int id = Int32.Parse(Session["person_id"].ToString());
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
                    Models.Profile theUser = db.Profiles.SingleOrDefault(c => c.person_id == id);
                    theUser.profile_pic = filename;
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

                var filePath = Server.MapPath("~/Images/" + thePicture.path);
                System.IO.File.Delete(filePath);

                db.Pictures.Remove(thePicture);
                db.SaveChanges();

                return RedirectToAction("Index", new { id = thePicture.person_id });
            }
            catch
            {
                return View();
            }
        }

        public ActionResult LikePic(int id) {
            int personId = Int32.Parse(Session["person_id"].ToString());

            Models.Like newLike = new Models.Like()
            {
                person_id = personId,
                picture_id = id,
                timestamp = DateTime.Now.ToString(),
                read = "Not read"
            };

            Models.Picture thePerson = db.Pictures.SingleOrDefault(c => c.picture_id == newLike.picture_id);

            db.Likes.Add(newLike);
            db.SaveChanges();

            ViewBag.liked = db.Likes.SingleOrDefault(c => c.person_id == personId && c.picture_id == id);

            return RedirectToAction("Index", new { id =  thePerson.person_id});
        }

        public ActionResult UnlikePic(int id)
        {
            int personId = Int32.Parse(Session["person_id"].ToString());

            Models.Like theLike = db.Likes.SingleOrDefault(c => c.person_id == personId && c.picture_id == id);

            Models.Picture thePerson = db.Pictures.SingleOrDefault(c => c.picture_id == theLike.picture_id);

            db.Likes.Remove(theLike);
            db.SaveChanges();

            return RedirectToAction("Index", new { id = thePerson.person_id });
        }
    }
}
