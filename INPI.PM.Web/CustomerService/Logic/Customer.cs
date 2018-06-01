using System;
using System.Collections.Generic;
using System.Text;

namespace INPI.PM.Domain.CustomerService.Logic
{
    public class Customer
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RealName { get; set; }
        public string Company { get; set; }
        public string Post { get; set; }
        public string PhoneNum { get; set; }
        public string FaxNum { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
    }
}
