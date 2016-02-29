using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlenMotorsDAL {
    public class BranchManager {
        /// <summary>
        /// Add new Branch (string)
        /// </summary>
        /// <param name="branchName">The name of the new branch</param>
        /// <returns>Retuns true if the addition has Succeeded, else returns string with an error message</returns>
        public static BranchManagerResult AddBranch(string branchName) {
            BranchManagerResult branchManagerResult = new BranchManagerResult();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    List <Branch> branchList = alenMotorsDbEntities.Branches.ToList();
                    if (
                    branchList.Any(
                                   branch =>
                                   branchName.Equals(branch.BranchName.Replace(" ", String.Empty), StringComparison.CurrentCultureIgnoreCase))) {
                        branchManagerResult.ErrorMessage = "A branch with this name already exists";
                        return branchManagerResult;
                    }
                    Branch addBranch = new Branch {BranchName = branchName};
                    alenMotorsDbEntities.Branches.Add(addBranch);
                    alenMotorsDbEntities.SaveChanges();
                    branchManagerResult.Success = true;
                    return branchManagerResult;
                }
            }
            catch (Exception ex) {
                branchManagerResult.ErrorMessage = ex.Message;
                return branchManagerResult;
            }
        }

        /// <summary>
        /// Edit an existing branch
        /// </summary>
        /// <param name="branchName"></param>
        /// <param name="branchNewName"></param>
        /// <returns>Retuns true if the addition has Succeeded, else returns string with an error message</returns>
        public static BranchManagerResult EditBranch(string branchName, string branchNewName) {
            BranchManagerResult branchManagerResult = new BranchManagerResult();
            using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                foreach (Branch branch in alenMotorsDbEntities.Branches.ToList().Where(branch => branch.BranchName == branchName)) {
                    branch.BranchName = branchNewName;
                    branchManagerResult.Success = true;
                    return branchManagerResult;
                }
            }
            return null;
        }

        /// <summary>
        /// Remove an existing branch
        /// </summary>
        /// <param name="branchName"></param>
        /// <returns>Retuns true if the addition has Succeeded, else returns string with an error message</returns>
        public static BranchManagerResult RemoveBranch(string branchName) {
            BranchManagerResult branchManagerResult = new BranchManagerResult();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    foreach (Branch branch in
                    alenMotorsDbEntities.Branches.ToList().Where(branch => branch.BranchName.Replace(" ", String.Empty) == branchName)) {
                        alenMotorsDbEntities.Branches.Remove(branch);
                        alenMotorsDbEntities.SaveChanges();
                        branchManagerResult.Success = true;
                        return branchManagerResult;
                    }
                }
            }
            catch (Exception ex) {
                branchManagerResult.ErrorMessage = ex.Message;
                return branchManagerResult;
            }
            return null;
        }

        /// <summary>
        /// Get all branches names into  (string)
        /// </summary>
        /// <returns>Returns a List<string> of all branches names, else a string with an error message </string></returns>
        public static BranchManagerResult GetBranchesNames() {
            BranchManagerResult branchManagerResult = new BranchManagerResult();
            List <string> branches = new List <string>();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    branches.AddRange(alenMotorsDbEntities.Branches.ToList().Select(branch => branch.BranchName.Replace(" ", String.Empty)));
                }
                branchManagerResult.Success = true;
                branchManagerResult.BranchesNames = branches;
                return branchManagerResult;
            }
            catch (Exception ex) {
                branchManagerResult.ErrorMessage = ex.Message;
                return branchManagerResult;
            }
        }

        /// <summary>
        /// Gets the branch name based on branch ID
        /// </summary>
        /// <param name="branchID">BranchID</param>
        /// <returns>Retuns the branch name which is coresponding to the ID</returns>
        public static BranchManagerResult GetBranch(int branchID) {
            BranchManagerResult branchManagerResult = new BranchManagerResult();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    foreach (Branch branch in alenMotorsDbEntities.Branches.ToList().Where(branch => branch.BranchID == branchID)) {
                        branchManagerResult.Branch = branch;
                        branchManagerResult.Success = true;
                        return branchManagerResult;
                    }
                }
            }
            catch (Exception ex) {
                branchManagerResult.ErrorMessage = ex.Message;
                return branchManagerResult;
            }
            return null;
        }

        public static BranchManagerResult GetBrances() {
            BranchManagerResult branchManagerResult = new BranchManagerResult();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    foreach (Branch branch in alenMotorsDbEntities.Branches.ToList()) {
                        branchManagerResult.Branches.Add(branch);
                    }
                    branchManagerResult.Success = true;
                    return branchManagerResult;
                }
            }
            catch (Exception ex) {
                branchManagerResult.ErrorMessage = ex.Message;
                return branchManagerResult;
            }
        }
    }
}