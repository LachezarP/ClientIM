using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientIM.ActionFilter
{
    public class NotifyFriendRequestFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Models.ClientEntities db = new Models.ClientEntities();

            int personId = Int32.Parse(filterContext.HttpContext.Session["person_id"].ToString());

            int counter = Int32.Parse(filterContext.HttpContext.Session["friend_request"].ToString());

            foreach (Models.FriendLink MF in db.FriendLinks.ToList())
            {
                if (MF.requested == personId && MF.read.Equals("Not read"))
                {
                    counter++;
                    filterContext.HttpContext.Session["friend_request"] = counter;
                }
            }
        }
    }
}