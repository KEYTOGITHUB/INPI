using System;
using System.Collections.Generic;
using System.Text;

namespace INPI.PM.Domain.UserService
{
    public class Role
    {
        public string GUID { get; set; }
        public string RoleName { get; set; }
        public string RoleDesc { get; set; }
        public List<Permission> Permissions { get; set; }

        public Role() { GUID = ""; }

        public Role(string roleName, string roleDesc, List<Permission> permissions)
        {
            GUID = Guid.NewGuid().ToString();
            RoleName = roleName;
            RoleDesc = roleDesc;
            Permissions = permissions;
        }
    }
}
