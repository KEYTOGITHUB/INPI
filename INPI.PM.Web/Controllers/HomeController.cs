using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using INPI.PM.Domain.ProjectService;
using INPI.PM.Domain.CustomerService;

namespace INPI.PM.Web.Controllers
{
    public class HomeController : Controller
    {
        private ProjectFactory _projectFactory = new ProjectFactory();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CustomerHome()
        {
            Contact currentCustomer = (Contact)Session["currentUser"];
            ViewBag.projects = _projectFactory.GetProjectsByCustomer(currentCustomer.GUID).OrderByDescending(p => p.EstimatedDeliveryDate);
            return View("CustomerHome1");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}