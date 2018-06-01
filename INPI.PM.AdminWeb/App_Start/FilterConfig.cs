﻿using System.Web;
using System.Web.Mvc;

namespace INPI.PM.AdminWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new PermissionFilterAttribute());
        }
    }
}
