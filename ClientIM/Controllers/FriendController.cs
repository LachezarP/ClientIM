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
        public ActionResult FriendLink()
        {
            return View(db.FriendLinks);
        }

        public ActionResult AllUsers()
        {
            int personId = Int32.Parse(Session["person_id"].ToString());

            Models.Profile theUser = db.Profiles.SingleOrDefault(c => c.person_id == personId);

            IEnumerable<Models.FriendLink> result = db.FriendLinks.Where(p => p.requester == personId);
            ViewBag.IEnumerable = result;

            return View(db.Profiles);
        }

        public ActionResult Approve(int id) {
            int personId = Int32.Parse(Session["person_id"].ToString());
            Models.FriendLink theLink = db.FriendLinks.SingleOrDefault(c => c.requested == id);

            theLink.approved = "true";
            theLink.status = "Friends";

            db.SaveChanges();

            return View("FriendLink", db.FriendLinks);
        }

        public ActionResult Refuse(int id)
        {

            return View(db.FriendLinks);
        }

        public ActionResult CreateFriendLink(int id)
        {
            int personId = Int32.Parse(Session["person_id"].ToString());

            Models.FriendLink theLink = db.FriendLinks.SingleOrDefault(c => c.requester == personId && c.requested == id);

            if (theLink == null) {

                Models.FriendLink newLink = new Models.FriendLink()
                {
                    requester = personId,
                    requested = id,
                    approved = "Pending",
                    status = "Strangers",
                    read = "Not read",
                    timestamp = DateTime.Now.ToString()
                };

                db.FriendLinks.Add(newLink);
                db.SaveChanges();

                ViewBag.person_id = newLink.Profile.person_id;

                return RedirectToAction("AllUsers");
            }
            else
                return RedirectToAction("AllUsers");
        }
    }
}
