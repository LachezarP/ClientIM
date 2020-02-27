using ClientIM.ActionFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientIM.Controllers
{
    [LoginFilter]
    public class FriendController : Controller
    {
        Models.ClientEntities db = new Models.ClientEntities();

        [ReadFriendLinkFilter]
        public ActionResult FriendLink()
        {
            Session["friend_request"] = 0;
            return View(db.FriendLinks);
        }

        public ActionResult AllUsers(string errorMessage)
        {
            int personId = Int32.Parse(Session["person_id"].ToString());

            Models.Profile theUser = db.Profiles.SingleOrDefault(c => c.person_id == personId);

            
            ViewBag.error = errorMessage;


            return View(db.Profiles);
        }

        public ActionResult Approve(int id) {
            int personId = Int32.Parse(Session["person_id"].ToString());
            Models.FriendLink theLink = db.FriendLinks.SingleOrDefault(c => c.requester == id && c.requested == personId);

            theLink.approved = "true";
            theLink.status = "Friends";

            db.SaveChanges();

            return View("FriendLink", db.FriendLinks);
        }

        public ActionResult Refuse(int id)
        {
            int personId = Int32.Parse(Session["person_id"].ToString());
            Models.FriendLink theLink = db.FriendLinks.SingleOrDefault(c => c.requester == id && c.requested == personId);

            db.FriendLinks.Remove(theLink);
            db.SaveChanges();

            return View("FriendLink", db.FriendLinks);
        }

        public ActionResult RemoveFriend(int requester, int requested)
        {

            int personId = Int32.Parse(Session["person_id"].ToString());

            if (requester == personId)
            {
                Models.FriendLink theLinkRequester = db.FriendLinks.SingleOrDefault(c => c.requester == personId && c.requested == requested);
                IEnumerable<Models.Message> theMessage = db.Messages.Where(c => (c.sender == personId && c.receiver == requested) || (c.sender == requested && c.receiver == personId));
                foreach(var item in theMessage)
                {
                    db.Messages.Remove(item);
                }
                db.FriendLinks.Remove(theLinkRequester);
                db.SaveChanges();
            }
            else
            {
                Models.FriendLink theLinkRequested = db.FriendLinks.SingleOrDefault(c => c.requester == requester && c.requested == personId);
                IEnumerable<Models.Message> theMessage = db.Messages.Where(c => (c.sender == personId && c.receiver == requester) || (c.sender == requester && c.receiver == personId));
                foreach (var item in theMessage)
                {
                    db.Messages.Remove(item);
                }
                db.FriendLinks.Remove(theLinkRequested);
                db.SaveChanges();
            }

            return View("FriendLink", db.FriendLinks);
        }

        public ActionResult CreateFriendLink(int id)
        {
            int personId = Int32.Parse(Session["person_id"].ToString());
            string error = "";
            Models.FriendLink theLinkRequester = db.FriendLinks.SingleOrDefault(c => c.requester == personId && c.requested == id);
            Models.FriendLink theLinkRequested = db.FriendLinks.SingleOrDefault(c => c.requester == id && c.requested == personId);
            if (theLinkRequester == null && theLinkRequested == null)
            {

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

                return RedirectToAction("AllUsers", new { errorMessage = error });
            }
            else
            {
                Models.Profile theProfile = db.Profiles.SingleOrDefault(p => p.person_id == id);
                error = "You have already sent/received a friend request to/from " + theProfile.first_name + " " + theProfile.last_name;

                return RedirectToAction("AllUsers",new {errorMessage = error});
            }
        }

        public ActionResult Search(String name)
        {
            IEnumerable<Models.Profile> result = db.Profiles.Where(p => (p.first_name + " " + p.last_name).Contains(name));
            return View("AllUsers", result);
        }
    }
}
