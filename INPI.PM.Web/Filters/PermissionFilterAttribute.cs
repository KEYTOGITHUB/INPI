using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace INPI.PM.Web.Filters
{
    public class PermissionFilterAttribute : ActionFilterAttribute
    {
        public bool Ignore { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (Ignore)
            {
                return;
            }
            if (filterContext.HttpContext.Session["currentUser"] == null)
            {
                filterContext.Result = new RedirectResult("/Login");
            }
        }
    }
}