using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientIM.ActionFilter
{
    public class ReadFriendLinkFilter : ActionFilterAttribute
    {

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Models.ClientEntities db = new Models.ClientEntities();

            int personId = Int32.Parse(filterContext.HttpContext.Session["person_id"].ToString());

            foreach (Models.FriendLink MF in db.FriendLinks.ToList()) {
                if (MF.requested == personId && MF.read.Equals("Not read")) {
                    MF.read = "Read";
                    db.SaveChanges();
                }
            }
        }
    }
}