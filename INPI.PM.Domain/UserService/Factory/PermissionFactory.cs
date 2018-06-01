using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INPI.PM.Domain.UserService
{
    public static class PermissionFactory
    {
        private static List<Permission> _permissions;

        public enum PermissionEnum
        {
            主页 = 1,
            客户管理 = 2,
            项目管理 = 3,
            系统管理 = 4,
            用户管理 = 5,
            角色管理 = 6,
            客户 = 7,
            客户联系人 = 8
        }

        static PermissionFactory()
        {
            _permissions = new List<Permission>();
            _permissions.Add(new Permission(1, "主页"));
            _permissions.Add(new Permission(2, "客户管理"));
            _permissions.Add(new Permission(3, "项目管理"));
            _permissions.Add(new Permission(4, "系统管理"));
            _permissions.Add(new Permission(5, "用户管理"));
            _permissions.Add(new Permission(6, "角色管理"));
            _permissions.Add(new Permission(7, "客户"));
            _permissions.Add(new Permission(8, "客户联系人"));
        }

        public static List<Permission> GetPermissions()
        {
            return _permissions;
        }
    }
}
