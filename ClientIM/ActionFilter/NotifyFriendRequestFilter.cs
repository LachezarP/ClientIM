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

            int counterFriend = Int32.Parse(filterContext.HttpContext.Session["friend_request"].ToString());

            int counterMessage = Int32.Parse(filterContext.HttpContext.Session["new_message"].ToString());
            if (counterFriend == 0)
            {
                foreach (Models.FriendLink MF in db.FriendLinks.ToList())
                {
                    if (MF.requested == personId && MF.read.Equals("Not read"))
                    {
                        counterFriend++;
                        filterContext.HttpContext.Session["friend_request"] = counterFriend;
                    }
                }
            }

            if (counterMessage == 0) { 
                foreach (Models.Message MM in db.Messages.ToList())
                {
                    if (MM.receiver == personId && MM.read.Equals("Not read"))
                    {
                        counterMessage++;
                        filterContext.HttpContext.Session["new_message"] = counterMessage;
                    }
                }
            }
        }
    }
}