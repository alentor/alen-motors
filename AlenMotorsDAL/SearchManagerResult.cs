using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlenMotorsDAL {
    public class SearchManagerResult {
        public bool Success{get; set;}

        public List <Account> users{get; set;}

        public List <Order> Orders{get; set;}
        public List <Vehicle> Vehicles{get; set;}

        public string ErrorMessage{get; set;}

        public SearchManagerResult() {
            Success = false;
            Orders = new List <Order>();
        }
    }
}