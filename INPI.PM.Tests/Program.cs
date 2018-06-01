using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INPI.PM.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            var roles = Domain.UserService.RoleFactory.GetAllRoles();
            foreach(var role in roles)
            {
                Console.WriteLine(role.GUID);
            }
            Console.ReadLine();
        }
    }
}
