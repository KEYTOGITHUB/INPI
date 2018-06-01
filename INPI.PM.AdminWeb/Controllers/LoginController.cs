using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using INPI.PM.Domain.UserService;
using INPI.PM.Domain.CustomerService;

namespace INPI.PM.AdminWeb.Controllers
{
    [PermissionFilter(Ignore = true)]
    public class LoginController : Controller
    {
        private UserFactory _userFactory = new UserFactory();
        private ContactFactory _customerFactory = new ContactFactory();

        // GET: Login
        public ActionResult Index()
        {
            ViewBag.isAdmin = 0;
            return View();
        }

        public ActionResult Admin()
        {
            ViewBag.isAdmin = 1;
            return View("Index");
        }

        public JsonResult AdminLogin()
        {
            string userName = Request["userName"].ToString();
            string password = Request["password"].ToString();
            int rememberMe = Convert.ToInt32(Request["rememberMe"]);
            User user;
            bool result = _userFactory.Login(out user, userName, password);
            if (result)
            {
                Session["currentUser"] = user;
                return Json(new { accessGranted = true });
            }
            else
            {
                return Json(new { accessGranted = false, errors = "账号/密码错误" });
            }
        }

        public JsonResult CustomerLogin()
        {
            string userName = Request["userName"].ToString();
            string password = Request["password"].ToString();
            int rememberMe = Convert.ToInt32(Request["rememberMe"]);
            Contact customer;
            bool result = _customerFactory.Login(out customer, userName, password);
            if (result)
            {
                Session["currentUser"] = customer;
                return Json(new { accessGranted = true });
            }
            else
            {
                return Json(new { accessGranted = false, errors = "账号/密码错误" });
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        public JsonResult SaveRegister()
        {
            string userName = Request["userName"].ToString();
            string password = Request["password"].ToString();
            string realName = Request["realName"].ToString();
            string company = Request["company"].ToString();
            string position = Request["position"].ToString();
            string phoneNum = Request["phoneNum"].ToString();
            string faxNum = "";
            string address = "";
            string postCode = "";
            _customerFactory.AddCcontact(userName, password, realName, company, position, phoneNum, faxNum, address, postCode, "");
            return Json(new { result = 1 });
        }

        public ActionResult Logout()
        {
            Session["currentUser"] = null;
            return RedirectToAction("Index");
        }
    }
}