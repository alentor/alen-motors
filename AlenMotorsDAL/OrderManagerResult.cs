using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlenMotorsDAL {
    public class OrderManagerResult {
        public bool Success{get; set;}
        public string ErrorMessage{get; set;}
        public Order Order{get; set;}

        public List <Order> VehicleList{get; set;}

        public OrderManagerResult() {
            Success = false;
            VehicleList = new List <Order>();
        }
    }
}