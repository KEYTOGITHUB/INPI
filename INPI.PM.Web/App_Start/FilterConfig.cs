using System.Web;
using System.Web.Mvc;

namespace INPI.PM.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new Filters.PermissionFilterAttribute() { Ignore = false });
        }
    }
}
