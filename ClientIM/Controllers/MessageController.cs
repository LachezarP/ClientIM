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
            IEnumerable<Models.FriendLink> friend = db.FriendLinks.Where(p => (p.requester == id || p.requested == id) && p.approved.Equals("true"));
            
            return View("Index", friend);
        }


        // GET: Message/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Message/Create
        public ActionResult Create(int id)
        {
            int counter = 0;
            ViewBag.id = id;
            int user = Int32.Parse(Session["person_id"].ToString());
            int counterMessage = Int32.Parse(Session["new_message"].ToString());
            IEnumerable<Models.Message> theMessage = db.Messages.Where(p => p.receiver == user && p.sender == id);
            foreach(var item in theMessage)
            {         
                if(item.read == "Not read")
                    counter++;
                item.read = "Read";
            }

            counterMessage -= counter;
            Session["new_message"] = counterMessage;
            db.SaveChanges();

            ViewBag.message = db.Messages.Where(p => p.receiver == user || p.sender == user);

            return View();
        }

        // POST: Message/Create
        [HttpPost]
        public ActionResult Create(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                Models.Message newMessage = new Models.Message
                {
                    sender = int.Parse(Session["person_id"].ToString()),
                    receiver = id,
                    message1 = collection["message1"],
                    timestamp = DateTime.Now.ToString(),
                    read = "Not read"
                };

                db.Messages.Add(newMessage);
                db.SaveChanges();

                return RedirectToAction("Index", new { id = id });
            }
            catch
            {
                return View();
            }
        }

    }
}
