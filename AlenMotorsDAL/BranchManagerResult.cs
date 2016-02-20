using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlenMotorsDAL {
    public class BranchManagerResult {
        public bool Success{get; set;}
        public List <string> BranchesNames{get; set;}
        public string ErrorMessage{get; set;}
        public Branch Branch{get; set;}
        public List <Branch> Branches{get; set;}


        public BranchManagerResult() {
            Branches = new List <Branch>();
            Success = false;
        }
    }
}