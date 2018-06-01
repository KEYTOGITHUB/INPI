using System;
using System.Collections.Generic;
using System.Text;

namespace INPI.PM.Domain.UserService.Logic
{
    public class Role
    {
        public string RoleName { get; set; }
        public List<Permission> Permissions { get; set; }
    }
}
