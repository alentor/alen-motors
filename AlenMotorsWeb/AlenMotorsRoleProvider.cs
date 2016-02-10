using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using AlenMotorsDAL;

namespace alenMotorsWeb {
    public class AlenMotorsRoleProvider: RoleProvider {
        public override bool IsUserInRole(string username, string roleName) {
            if (username == "b") {
                return true;
            }
            if (username == "c") {
                return true;
            }
            return false;
            //throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string email) {
            UserRoleManagerResult userRoleManagerResult =  UserRoleManager.GetUserRoles(email);
            return userRoleManagerResult?.Roles;

        }

        public override void CreateRole(string roleName) {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole) {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName) {
            throw new NotImplementedException();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames) {
            //throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames) {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName) {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles() {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch) {
            throw new NotImplementedException();
        }

        public override string ApplicationName{get; set;}
    }
}