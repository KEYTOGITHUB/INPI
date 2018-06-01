using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using INPI.PM.Domain.CustomerService;

namespace INPI.PM.AdminWeb.Controllers
{
    public class CustomerController : Controller
    {
        private ContactFactory _contactFactory = new ContactFactory();
        private CustomerFactory _customerFactory = new CustomerFactory();
        // GET: Customer
        public ActionResult Index()
        {
            ViewBag.customers = _customerFactory.GetAllCustomers() ?? new List<Customer>();
            return View();
        }

        public ActionResult CustomerEdit(string guid = "")
        {
            Customer customer = new Customer();
            if (!string.IsNullOrWhiteSpace(guid)) customer = _customerFactory.GetCustomer(guid);
            return View(customer);
        }

        public JsonResult SaveCustomer(string guid = "")
        {
            string companyName = Request["companyName"].ToString();
            string address = Request["address"].ToString();
            string phoneNum = Request["phoneNum"].ToString();
            string faxNum = Request["faxNum"].ToString();
            string postCode = Request["postCode"].ToString();

            if (string.IsNullOrWhiteSpace(guid))
            {
                _customerFactory.AddCustomer(companyName, address, phoneNum, faxNum, postCode);
            }
            else
            {
                var customer = _customerFactory.GetCustomer(guid);
                customer.CompanyName = companyName;
                customer.PhoneNum = phoneNum;
                customer.FaxNum = faxNum;
                customer.Address = address;
                customer.PostCode = postCode;
                _customerFactory.SaveCustomer(customer);
            }
            return Json(new { result = 1 });
        }

        public ActionResult Contact()
        {
            ViewBag.contacts = _contactFactory.GetAllContacts() ?? new List<Contact>();
            return View();
        }

        public ActionResult ContactEdit(string guid = "")
        {
            ViewBag.customers = _customerFactory.GetAllCustomers() ?? new List<Customer>();
            Contact contact = new Contact();
            if (!string.IsNullOrWhiteSpace(guid)) contact = ContactFactory.GetContact(guid);
            return View(contact);
        }

        public JsonResult SaveContact(string guid = "")
        {
            string realName = Request["realName"].ToString();
            string company = Request["company"].ToString();
            string position = Request["position"].ToString();
            string phoneNum = Request["phoneNum"].ToString();
            string faxNum = Request["faxNum"].ToString();
            string postCode = Request["postCode"].ToString();
            string address = Request["address"].ToString();
            string customerGuid = Request["customerGuid"].ToString();

            if (string.IsNullOrWhiteSpace(guid))
            {
                string userName = Request["userName"].ToString();
                string password = Request["password"].ToString();
                _contactFactory.AddCcontact(userName, password, realName, company, position, phoneNum, faxNum, address, postCode, customerGuid);
            }
            else
            {
                var contact = ContactFactory.GetContact(guid);
                contact.RealName = realName;
                contact.Company = company;
                contact.Position = position;
                contact.PhoneNum = phoneNum;
                contact.FaxNum = faxNum;
                contact.PostCode = postCode;
                contact.Address = address;
                contact.CustomerGuid = customerGuid;
                _contactFactory.SaveContact(contact);
            }

            return Json(new { result = 1 });
        }

        public JsonResult DeleteContact(string guid = "")
        {
            var re = _contactFactory.DeleteContact(guid);
            return Json(new { result = re ? 1 : 0 });
        }

        public JsonResult DeleteCustomer(string guid = "")
        {
            var re = _customerFactory.DeleteCustomer(guid);
            return Json(new { result = re ? 1 : 0 });
        }
    }
}