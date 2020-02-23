using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientIM.ActionFilter
{
    public class LoginFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.HttpContext.Session["user_id"] == null)
            {
                System.Web.Routing.RouteValueDictionary routeValues = new System.Web.Routing.RouteValueDictionary();

                routeValues.Add("controller", "Home");
                routeValues.Add("action", "Index");

                filterContext.Result = new RedirectToRouteResult("Default", routeValues);
            }
        }
    }
}