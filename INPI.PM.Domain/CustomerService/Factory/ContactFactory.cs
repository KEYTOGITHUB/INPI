using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace INPI.PM.Domain.CustomerService
{
    public class ContactFactory
    {
        private static ContactRepo _customerRepo = new ContactRepo();

        public bool Login(out Contact contact, string userName, string password)
        {
            string cipherText = GetMD5(password);
            contact = _customerRepo.GetContactByUserName(userName);
            if (contact == null) return false;
            return contact.Password == cipherText;
        }

        public Contact AddCcontact(string userName, string password, string realName, string company,
            string position, string phoneNum, string faxNum, string address, string postCode, string customerGuid)
        {
            Contact customer = new Contact()
            {
                GUID = Guid.NewGuid().ToString(),
                UserName = userName,
                Password = GetMD5(password),
                RealName = realName,
                Company = company,
                Position = position,
                PhoneNum = phoneNum,
                FaxNum = faxNum,
                Address = address,
                PostCode = postCode,
                CustomerGuid = customerGuid,
                CreateTime = DateTime.Now
            };
            _customerRepo.AddContact(customer);
            return customer;
        }

        public static Contact GetContact(string guid)
        {
            return _customerRepo.GetContact(guid);
        }

        public List<Contact> GetAllContacts()
        {
            return _customerRepo.GetAllContacts();
        }

        private string GetMD5(string plainText)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] tmp = Encoding.Default.GetBytes(plainText);
            byte[] cipherByte = md5.ComputeHash(tmp);
            string cipherText = BitConverter.ToString(cipherByte);
            return cipherText;
        }

        public bool SaveContact(Contact contact)
        {
            return _customerRepo.SaveContact(contact);
        }

        public bool DeleteContact(string guid)
        {
            return _customerRepo.DeleteContact(guid);
            
        }
    }
}
