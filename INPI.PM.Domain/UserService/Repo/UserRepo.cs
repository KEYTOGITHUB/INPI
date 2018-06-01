using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INPI.PM.Infrastructure.DataAccess.RedisAccess;

namespace INPI.PM.Domain.UserService
{
    public class UserRepo
    {
        private static string _key = "userList";
        private RedisHelper _redisHelper = new RedisHelper();
        private static List<User> _userList = new List<User>();

        public List<User> GetAllUsers()
        {
            return _redisHelper.GetAllListFromSet<User>(_key);
            //return _userList;
        }

        public bool AddUser(User user)
        {
            return _redisHelper.AddModelToSet<User>(_key, user);
            //_userList.Add(user);
            //return true;
        }

        public User GetUser(string guid)
        {
            return _redisHelper.GetModelFromSet<User>(_key, p => p.GUID == guid);
            //return _userList.Single(p => p.GUID == guid);
        }

        public bool UpdateUser(User user)
        {
            return _redisHelper.UpdataModelInSet<User>(_key, p => p.GUID == user.GUID, user);
            //int index = _userList.FindIndex(p => p.GUID == user.GUID);
            //_userList[index] = user;
            //return true;
        }

        public User GetUserByUserName(string userName)
        {
            return _redisHelper.GetModelFromSet<User>(_key, p => p.UserName == userName);
            //return _userList.First(p => p.UserName == userName);
        }

        public bool DeleteUser(string guid)
        {
            return _redisHelper.RemoveModelFromSet<User>(_key, p => p.GUID == guid);
        }
    }
}
