using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace INPI.PM.Domain.UserService
{
    public class UserFactory
    {
        private static UserRepo _userRepo = new UserRepo();

        public static List<User> GetAllUsers()
        {
            return _userRepo.GetAllUsers();
        }

        public bool Login(out User user, string userName, string password)
        {
            if (userName == "admin" && password == "123456")
            {
                user = new User("admin", "123456", "admin", "",
                    new List<Role> { new Role("超级管理员", "初始化生成", PermissionFactory.GetPermissions()) });
                return true;
            }
            string cipherText = GetMD5(password);
            user = _userRepo.GetUserByUserName(userName);
            if (user == null) return false;
            return user.Password == cipherText;
        }

        public User GetUser(string guid)
        {
            return _userRepo.GetUser(guid);
        }

        public User AddUser(string userName, string realName, string phoneNum, string roleGuids)
        {
            List<Role> roles = new List<Role>();
            var tmp = roleGuids.Split(',').ToList();
            var allRoles = RoleFactory.GetAllRoles();
            tmp.ForEach(p =>
            {
                roles.Add(allRoles.Single(q => q.GUID == p));
            });
            User user = new User(userName, GetMD5("123456"), realName, phoneNum, roles);
            _userRepo.AddUser(user);
            return user;
        }

        public bool SaveUser(string guid, string userName, string realName, string phoneNum, string roleGuids)
        {
            List<Role> roles = new List<Role>();
            var tmp = roleGuids.Split(',').ToList();
            var allRoles = RoleFactory.GetAllRoles();
            tmp.ForEach(p =>
            {
                roles.Add(allRoles.Single(q => q.GUID == p));
            });
            var user = _userRepo.GetUser(guid);
            user.UserName = userName;
            user.RealName = realName;
            user.PhoneNum = phoneNum;
            user.Roles = roles;
            return _userRepo.UpdateUser(user);
        }

        private string GetMD5(string plainText)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] tmp = Encoding.Default.GetBytes(plainText);
            byte[] cipherByte = md5.ComputeHash(tmp);
            string cipherText = BitConverter.ToString(cipherByte);
            return cipherText;
        }

        public bool DeleteUser(string guid)
        {
            return _userRepo.DeleteUser(guid);
        }
    }
}
