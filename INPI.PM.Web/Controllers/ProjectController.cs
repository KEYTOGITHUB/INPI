using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using INPI.PM.Domain.CustomerService;
using INPI.PM.Domain.UserService;
using INPI.PM.Domain.ProjectService;

namespace INPI.PM.Web.Controllers
{
    public class ProjectController : Controller
    {
        private static ContactFactory _customerFactory = new ContactFactory();
        private static UserFactory _userFactory = new UserFactory();
        private static ProjectFactory _projectFactory = new ProjectFactory();

        // GET: Project
        public ActionResult Index()
        {
            ViewBag.projects = _projectFactory.GetAllProjects() ?? new List<Project>(); ;
            return View();
        }

        public ActionResult ProjectEdit()
        {
            ViewBag.customers = _customerFactory.GetAllContacts() ?? new List<Contact>();
            ViewBag.users = UserFactory.GetAllUsers();
            return View();
        }

        public JsonResult SaveProject()
        {
            string projectCode = Request["projectCode"].ToString();
            string projectName = Request["projectName"].ToString();
            string customerGuid = Request["customerGuid"].ToString();
            string contactGuid = Request["contactGuid"].ToString();
            DateTime createDate = Convert.ToDateTime(Request["createDate"]);
            DateTime estimatedDeliveryDate = Convert.ToDateTime(Request["estimatedDeliveryDate"]);
            string personInCharge = Request["personInCharge"].ToString();

            User currentUser = (User)Session["currentUser"];
            _projectFactory.AddProject(projectCode, projectName, customerGuid, contactGuid, createDate, estimatedDeliveryDate, personInCharge, "", currentUser.GUID);
            return Json(new { result = 1 });
        }

        public JsonResult GoToNextStep()
        {
            string guid = Request["guid"].ToString();
            //DateTime estimatedDate = Convert.ToDateTime(Request["estimatedDate"]);
            DateTime actualDate = Convert.ToDateTime(Request["actualDate"]);
            var personInCharge = Request["personInCharge"].ToString();
            var project = _projectFactory.GetProject(guid);
            
            project.GoToNextStep(actualDate, personInCharge);
            return Json(new { result = 1 });
        }
    }
}