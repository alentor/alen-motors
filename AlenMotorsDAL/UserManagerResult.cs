using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlenMotorsDAL {
    /// <summary>
    /// A result class for UserManager
    /// </summary>
    public class UserManagerResult {
        public bool Success{get; set;}
        public string ErrorMessage{get; set;}
        public Account User{get; set;}

        public List <Account> UserList{get; set;}

        public UserManagerResult() {
            Success = false;
            UserList = new List <Account>();
        }

    }
}