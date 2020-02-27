using ClientIM.ActionFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientIM.Controllers
{
    [LoginFilter]
    public class MessageController : Controller
    {
        Models.ClientEntities db = new Models.ClientEntities();
        // GET: Message
        public ActionResult Index()
        {
            int id = Int32.Parse(Session["person_id"].ToString());
            IEnumerable<Models.FriendLink> friend = db.FriendLinks.Where(p => p.requester == id || p.requested == id && p.approved.Equals("true"));
            /*var query = db.Profiles.Join(db.FriendLinks,
                p => p.person_id,
                c => c.requester,
                (p, c) => new { Profile = p, FriendLinks = c }).Where(c => c.FriendLinks.approved.Equals("true"));
            */
            return View("Index", friend);
        }

        // GET: Message/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Message/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Message/Create
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

        // GET: Message/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Message/Edit/5
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

        // GET: Message/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Message/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
