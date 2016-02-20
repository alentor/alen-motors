using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlenMotorsDAL {
    public class VehicleManagerResult {
        public bool Success{get; set;}
        public string ErrorMessage{get; set;}
        public Vehicle Vehicle { get; set;}

        public List <Vehicle> VehicleList{get; set;}

        public VehicleManagerResult() {
            Success = false;
            VehicleList = new List <Vehicle>();
        }
    }
}