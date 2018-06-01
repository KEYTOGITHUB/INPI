using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using INPI.PM.Domain.CustomerService;

namespace INPI.PM.Web.Controllers
{
    public class CustomerController : Controller
    {
        private ContactFactory _customerFactory = new ContactFactory();
        // GET: Customer
        public ActionResult Index()
        {
            ViewBag.customers = _customerFactory.GetAllContacts() ?? new List<Contact>();
            return View();
        }
    }
}