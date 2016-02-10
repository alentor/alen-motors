using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlenMotorsDAL {
    public class UserRoleManagerResult
    {
        public bool Success { get; set; }
        public string[] Roles { get; set; }
        public string ErrorMessage { get; set; }
    }
}