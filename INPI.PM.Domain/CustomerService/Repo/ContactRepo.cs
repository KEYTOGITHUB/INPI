using INPI.PM.Infrastructure.DataAccess.RedisAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INPI.PM.Domain.CustomerService
{
    public class ContactRepo
    {
        private static string _key = "contactList";
        private RedisHelper _redisHelper = new RedisHelper();
        private static List<Contact> _contactList = new List<Contact>();

        public bool AddContact(Contact contact)
        {
            return _redisHelper.AddModelToSet<Contact>(_key, contact);
            //_customerList.Add(customer);
            //return true;
        }

        public List<Contact> GetAllContacts()
        {
            return _redisHelper.GetAllListFromSet<Contact>(_key);
            //return _customerList;
        }

        public Contact GetContact(string guid)
        {
            return _redisHelper.GetModelFromSet<Contact>(_key, p => p.GUID == guid);
            //return _customerList.Single(p => p.GUID == guid);
        }

        public Contact GetContactByUserName(string userName)
        {
            return _redisHelper.GetModelFromSet<Contact>(_key, p => p.UserName == userName);
            //return _customerList.First(p => p.UserName == userName);
        }

        public bool SaveContact(Contact contact)
        {
            return _redisHelper.UpdataModelInSet<Contact>(_key, p => p.GUID == contact.GUID, contact);
        }

        public bool DeleteContact(string guid)
        {
            return _redisHelper.RemoveModelFromSet<Contact>(_key, p => p.GUID == guid);
        }
    }
}
