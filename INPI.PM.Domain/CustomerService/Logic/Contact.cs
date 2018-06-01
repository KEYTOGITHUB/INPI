using System;
using System.Collections.Generic;
using System.Text;

namespace INPI.PM.Domain.CustomerService
{
    public class Contact
    {
        public string GUID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RealName { get; set; }
        public string Company { get; set; }
        public string Position { get; set; }
        public string PhoneNum { get; set; }
        public string FaxNum { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public DateTime CreateTime { get; set; }
        public string CustomerGuid { get; set; }
    }
}
