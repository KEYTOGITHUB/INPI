using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INPI.PM.Domain.UserService
{
    public class RoleFactory
    {
        private static RoleRepo _roleRepo = new RoleRepo();

        public static List<Role> GetAllRoles()
        {
            return _roleRepo.GetAllRoles();
        }

        public Role AddRole(string roleName, string roleDesc, string permissions)
        {
            var tmp = permissions.Split(',').Select(p => Convert.ToInt32(p)).ToList();
            List<Permission> list = new List<Permission>();
            var allPermissions = PermissionFactory.GetPermissions();
            tmp.ForEach(p =>
            {
                list.Add(allPermissions.Single(q => q.PermissionId == p));
            });
            var role = new Role(roleName, roleDesc, list);
            _roleRepo.AddRole(role);
            return role;
        }

        public Role GetRole(string guid)
        {
            return _roleRepo.GetRole(guid);
        }

        public bool Save(string guid, string roleName, string roleDesc, string permissions)
        {
            var tmp = permissions.Split(',').Select(p => Convert.ToInt32(p)).ToList();
            List<Permission> list = new List<Permission>();
            var allPermissions = PermissionFactory.GetPermissions();
            tmp.ForEach(p =>
            {
                list.Add(allPermissions.Single(q => q.PermissionId == p));
            });
            var role = GetRole(guid);
            role.RoleName = roleName;
            role.RoleDesc = roleDesc;
            role.Permissions = list;
            return Save(role);
        }

        public bool Save(Role role)
        {
            return _roleRepo.UpdateRole(role);
        }

        public bool DeleteRole(string guid)
        {
            return _roleRepo.DeleteRole(guid);
        }
    }
}
