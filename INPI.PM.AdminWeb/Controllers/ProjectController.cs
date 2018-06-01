using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using INPI.PM.Domain.CustomerService;
using INPI.PM.Domain.UserService;
using INPI.PM.Domain.ProjectService;

namespace INPI.PM.AdminWeb.Controllers
{
    public class ProjectController : Controller
    {
        private static CustomerFactory _customerFactory = new CustomerFactory();
        private static ProjectFactory _projectFactory = new ProjectFactory();

        // GET: Project
        public ActionResult Index()
        {
            ViewBag.projects = _projectFactory.GetAllProjects() ?? new List<Project>(); ;
            return View();
        }

        public ActionResult ProjectEdit(string guid = "")
        {
            var project = _projectFactory.GetProject(guid);
            ViewBag.customers = _customerFactory.GetAllCustomers() ?? new List<Customer>();
            ViewBag.users = UserFactory.GetAllUsers()??new List<User>();
            return View(project ?? new Project());
        }

        public ActionResult ProjectPlan(string guid)
        {
            var project = _projectFactory.GetProject(guid);
            ViewBag.projectSchedules = project.ProjectSchedules;
            ViewBag.guid = project.GUID;
            ViewBag.createDate = project.CreateDate.ToString("yyyy-MM-dd");
            ViewBag.estimatedDeliveryDate = project.EstimatedDeliveryDate.ToString("yyyy-MM-dd");
            ViewBag.users = UserFactory.GetAllUsers() ?? new List<User>();
            return View();
        }

        public JsonResult SaveProjectPlan(string guid)
        {
            string[] dates = Request["date"].ToString().Split(',');
            string[] guids = Request["guids"].ToString().Split(',');
            var project = _projectFactory.GetProject(guid);
            int index = 0;
            foreach (var step in project.ProjectRoute.Steps)
            {
                int i = project.ProjectSchedules.FindIndex(p => p.Step.StepOrder == step.StepOrder);
                DateTime? date = null;
                if (!string.IsNullOrEmpty(dates[index])) date = Convert.ToDateTime(dates[index]);
                if (i >= 0)
                {
                    project.ProjectSchedules[i].EstimatedCompleteDate = date;
                    if (!string.IsNullOrEmpty(guids[index]))
                    {
                        project.ProjectSchedules[i].PersonInCharge = guids[index];
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(guids[index]))
                    {
                        project.ProjectSchedules.Add(new ProjectSchedule(step, date,guids[index]));
                    }
                    else
                    {
                        project.ProjectSchedules.Add(new ProjectSchedule(step, date));
                    }
                }
                    
                index++;
            }
            _projectFactory.SaveProject(project);
            return Json(new { result = 1 });
        }

        public JsonResult SaveProject(string guid = "")
        {
            string projectCode = Request["projectCode"].ToString();
            string projectName = Request["projectName"].ToString();
            string customerGuid = Request["customerGuid"].ToString();
            string contactGuid = Request["contactGuid"].ToString();
            DateTime createDate = Convert.ToDateTime(Request["createDate"]);
            DateTime estimatedDeliveryDate = Convert.ToDateTime(Request["estimatedDeliveryDate"]);
            string personInCharge = Request["personInCharge"].ToString();
            if (string.IsNullOrWhiteSpace(guid))
            {
                User currentUser = (User)Session["currentUser"];
                _projectFactory.AddProject(projectCode, projectName, customerGuid, contactGuid, createDate, estimatedDeliveryDate, personInCharge, "", currentUser.GUID);
            }
            else
            {
                var project = _projectFactory.GetProject(guid);
                project.ProjectCode = projectCode;
                project.ProjectName = projectName;
                project.CustomerGuid = customerGuid;
                project.ContactGuid = contactGuid;
                project.CreateDate = createDate;
                project.EstimatedDeliveryDate = estimatedDeliveryDate;
                project.PersonInCharge = personInCharge;
                _projectFactory.SaveProject(project);
            }
            return Json(new { result = 1 });
        }

        public JsonResult DeleteProject()
        {
            string guid = Request["guid"].ToString();
            _projectFactory.RemoveProject(guid);
            return Json(new { result = 1 });
        }

        public JsonResult GoToNextStep()
        {
            string guid = Request["guid"].ToString();
            DateTime actualDate = Convert.ToDateTime(Request["actualDate"]);
            var project = _projectFactory.GetProject(guid);
            var personInCharge = Request["personIncharge"].ToString();
            User currentUser = (User)Session["currentUser"];
            project.GoToNextStep(actualDate, personInCharge);
            return Json(new { result = 1 });
        }

        public JsonResult GetProjectContactsByCustomerGuid(string guid)
        {
            var re = new ContactFactory().GetAllContacts().Where(c => c.CustomerGuid.Equals(guid)).ToList();
            return Json(new { result=1,dataList=re},JsonRequestBehavior.AllowGet);
        }
    }
}