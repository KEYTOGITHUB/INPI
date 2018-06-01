using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INPI.PM.Infrastructure.DataAccess.RedisAccess;

namespace INPI.PM.Domain.UserService
{
    public class RoleRepo
    {
        private static string _key = "roleList";
        private RedisHelper _redisHelper = new RedisHelper();
        private static List<Role> _roleList = new List<Role>();

        public List<Role> GetAllRoles()
        {
            return _redisHelper.GetAllListFromSet<Role>(_key);
            //return _roleList;
        }

        public bool AddRole(Role role)
        {
            return _redisHelper.AddModelToSet<Role>(_key, role);
            //_roleList.Add(role);
            //return true;
        }

        public bool UpdateRole(Role role)
        {
            return _redisHelper.UpdataModelInSet<Role>(_key, p => p.GUID == role.GUID, role);
            //int index=_roleList.FindIndex(p => p.GUID == role.GUID);
            //_roleList[index] = role;
            //return true;
        }

        public Role GetRole(string guid)
        {
            return _redisHelper.GetModelFromSet<Role>(_key, p => p.GUID == guid);
            //return _roleList.Single<Role>(p => p.GUID == guid);
        }

        public bool DeleteRole(string guid)
        {
            return _redisHelper.RemoveModelFromSet<Role>(_key, p => p.GUID == guid);
        }
    }
}
