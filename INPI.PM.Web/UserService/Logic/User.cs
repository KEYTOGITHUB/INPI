using System;
using System.Collections.Generic;
using System.Text;

namespace INPI.PM.Domain.UserService.Logic
{
    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<Role> Roles { get; set; }
    }
}
