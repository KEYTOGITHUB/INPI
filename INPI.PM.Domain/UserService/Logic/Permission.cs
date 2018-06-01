using System;
using System.Collections.Generic;
using System.Text;

namespace INPI.PM.Domain.UserService
{
    public class Permission
    {
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }

        public Permission(int permissionId, string permissionName)
        {
            PermissionId = permissionId;
            PermissionName = permissionName;
        }
    }
}
