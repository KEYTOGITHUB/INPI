using INPI.PM.Infrastructure.DataAccess.RedisAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INPI.PM.Domain.CustomerService
{
    public class CustomerRepo
    {
        private static string _key = "customerList";
        private RedisHelper _redisHelper = new RedisHelper();

        public bool AddCustomer(Customer customer)
        {
            return _redisHelper.AddModelToSet<Customer>(_key, customer);
        }

        public List<Customer> GetAllCustomers()
        {
            return _redisHelper.GetAllListFromSet<Customer>(_key);
        }

        public Customer GetCustomer(string guid)
        {
            return _redisHelper.GetModelFromSet<Customer>(_key, p => p.GUID == guid);
        }

        public bool SaveCustomer(Customer customer)
        {
            return _redisHelper.UpdataModelInSet<Customer>(_key, p => p.GUID == customer.GUID, customer);
        }

        public bool DeleteCustomer(string guid)
        {
            return _redisHelper.RemoveModelFromSet<Customer>(_key,p=>p.GUID==guid);
        }
    }
}
