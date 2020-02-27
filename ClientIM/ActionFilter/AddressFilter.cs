using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientIM.ActionFilter
{
    public class AddressFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            int id = Int32.Parse(filterContext.HttpContext.Session["person_id"].ToString());
            int AddressId = Int32.Parse(filterContext.HttpContext.Session["Address_person_id"].ToString());

            if (id != AddressId)
            {
                System.Web.Routing.RouteValueDictionary routeValues = new System.Web.Routing.RouteValueDictionary();

                routeValues.Add("controller", "Profile");
                routeValues.Add("action", "Index");

                filterContext.Result = new RedirectToRouteResult("Profile", routeValues);
            }
        }
    }
}