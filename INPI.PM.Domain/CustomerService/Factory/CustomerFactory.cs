using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INPI.PM.Domain.CustomerService
{
    public class CustomerFactory
    {
        private static CustomerRepo _customerRepo = new CustomerRepo();

        public bool AddCustomer(string companyName, string address, string phoneNum, string faxNum, string postCode)
        {
            Customer customer = new Customer(companyName, address, phoneNum, faxNum, postCode);
            return _customerRepo.AddCustomer(customer);
        }

        public bool SaveCustomer(Customer customer)
        {
            return _customerRepo.SaveCustomer(customer);
        }

        public Customer GetCustomer(string guid)
        {
            return _customerRepo.GetCustomer(guid);
        }

        public List<Customer> GetAllCustomers()
        {
            return _customerRepo.GetAllCustomers();
        }

        public bool DeleteCustomer(string guid)
        {
            return _customerRepo.DeleteCustomer(guid);

        }
    }
}
