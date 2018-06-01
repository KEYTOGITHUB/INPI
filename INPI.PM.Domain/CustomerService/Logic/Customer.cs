using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INPI.PM.Domain.CustomerService
{
    public class Customer
    {
        public string GUID { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string PhoneNum { get; set; }
        public string FaxNum { get; set; }
        public string PostCode { get; set; }

        public Customer() { }

        public Customer(string companyName, string address, string phoneNum, string faxNum, string postCode)
        {
            GUID = Guid.NewGuid().ToString();
            CompanyName = companyName;
            Address = address;
            PhoneNum = phoneNum;
            FaxNum = faxNum;
            PostCode = postCode;
        }
    }
}
