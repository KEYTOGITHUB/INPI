using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INPI.PM.Domain.UserService
{
    public class User
    {
        public string GUID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RealName { get; set; }
        public string PhoneNum { get; set; }
        public DateTime CreateTime { get; set; }
        public List<Role> Roles { get; set; }

        public User() { }

        public User(string userName, string password, string realName, string phoneNum, List<Role> roles)
        {
            GUID = Guid.NewGuid().ToString();
            UserName = userName;
            Password = password;
            RealName = realName;
            PhoneNum = phoneNum;
            Roles = roles;
            CreateTime = DateTime.Now;
        }

        public bool IsPermissionAvaliable(int permissionId)
        {
            var permissions = new List<Permission>();
            Roles.ForEach(p =>
            {
                permissions = permissions.Concat(p.Permissions).ToList();
            });
            return permissions.FindIndex(p => p.PermissionId == permissionId) >= 0;
        }
    }
}
