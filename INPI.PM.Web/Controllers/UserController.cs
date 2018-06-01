using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using INPI.PM.Domain.UserService;

namespace INPI.PM.Web.Controllers
{
    public class UserController : Controller
    {
        private RoleFactory _roleFactory = new RoleFactory();
        private UserFactory _userFactory = new UserFactory();
        // GET: User
        public ActionResult Index()
        {
            ViewBag.users = UserFactory.GetAllUsers() ?? new List<User>();
            return View();
        }

        public ActionResult UserEdit(string guid = "")
        {
            ViewBag.roles = RoleFactory.GetAllRoles() ?? new List<Role>();
            User user = new User();
            if (!string.IsNullOrWhiteSpace(guid)) user = _userFactory.GetUser(guid);
            return View(user);
        }

        public JsonResult SaveUser(string guid = "")
        {
            string userName = Request["userName"].ToString();
            string realName = Request["realName"].ToString();
            string phoneNum = Request["phoneNum"].ToString();
            string roles = Request["roles"].ToString();
            if (string.IsNullOrWhiteSpace(guid))
            {
                _userFactory.AddUser(userName, realName, phoneNum, roles);
            }
            else
            {
                _userFactory.SaveUser(guid, userName, realName, phoneNum, roles);
            }
            return Json(new { result = 1 });
        }

        public ActionResult Role()
        {
            ViewBag.roles = RoleFactory.GetAllRoles() ?? new List<Role>();
            return View();
        }

        public ActionResult RoleEdit(string guid = "")
        {
            Role role = new Role();
            if (!string.IsNullOrWhiteSpace(guid)) role = _roleFactory.GetRole(guid);
            ViewBag.permissions = PermissionFactory.GetPermissions();
            return View(role);
        }

        public JsonResult SaveRole(string guid = "")
        {
            string roleName = Request["roleName"].ToString();
            string roleDesc = Request["roleDesc"].ToString();
            string permissions = Request["permissions"].ToString();
            if (string.IsNullOrWhiteSpace(guid))
            {
                _roleFactory.AddRole(roleName, roleDesc, permissions);
            }
            else
            {
                _roleFactory.Save(guid, roleName, roleDesc, permissions);
            }
            return Json(new { result = 1 });
        }
    }
}