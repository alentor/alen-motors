using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlenMotorsDAL {
    public static class UserRoleManager {
        /// <summary>
        /// Add a new role to the avalaible roles
        /// </summary>
        /// <param name="role">Name of the new role</param>
        /// <returns>Return true on successful operation, else a string with the error message</returns>
        public static UserRoleManagerResult AddRole(string role) {
            UserRoleManagerResult userManagerResult = new UserRoleManagerResult();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    alenMotorsDbEntities.Roles.Add(new Role {RoleName = role});
                    alenMotorsDbEntities.SaveChanges();
                }
                userManagerResult.Success = true;
                return userManagerResult;
            }
            catch (Exception ex) {
                userManagerResult.ErrorMessage = ex.Message;
                return userManagerResult;
            }
        }

        /// <summary>
        /// Adds a new role to the user
        /// </summary>
        /// <param name="email">The user's email</param>
        /// <param name="roleName">The role to add to the user</param>
        /// <returns>Return true on successful operation, else a string with the error message</returns>
        public static UserRoleManagerResult AddRoleToUser(string email, string roleName) {
            UserRoleManagerResult userManagerResult = new UserRoleManagerResult();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    foreach (Account account in alenMotorsDbEntities.Accounts.ToList().Where(account => account.Email.Replace(" ", string.Empty) == email)) {
                        Role roleToAdd = Enumerable.FirstOrDefault(alenMotorsDbEntities.Roles, role => role.RoleName == roleName);
                        account.AccountsInRoles.Add(new AccountInRole {AccountID = account.AccountID, RoleID = roleToAdd.RoleID});
                        alenMotorsDbEntities.SaveChanges();
                        userManagerResult.Success = true;
                        return userManagerResult;
                    }
                }
                return null;
            }
            catch (Exception ex) {
                userManagerResult.ErrorMessage = ex.Message;
                return userManagerResult;
            }
        }

        /// <summary>
        /// Removes a role from the user
        /// </summary>
        /// <param name="email">The user's email</param>
        /// <param name="roleName">The role to remove from the user</param>
        /// <returns>Return true on successful operation, else a string with the error message</returns>
        public static UserRoleManagerResult RemoveRole(string email, string roleName) {
            UserRoleManagerResult userManagerResult = new UserRoleManagerResult();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    foreach (Account account in alenMotorsDbEntities.Accounts.ToList()) {
                        if (account.Email.Replace(" ", string.Empty) == email) {
                            int roleToRemove = 0;
                            foreach (Role role in alenMotorsDbEntities.Roles.ToList().Where(role => role.RoleName == roleName)) {
                                roleToRemove = role.RoleID;
                            }
                            AccountInRole x = account.AccountsInRoles.FirstOrDefault(accountsInRole => accountsInRole.RoleID == roleToRemove);
                            account.AccountsInRoles.Remove(x);
                            alenMotorsDbEntities.SaveChanges();
                            userManagerResult.Success = true;
                            return userManagerResult;
                        }
                    }
                }
                return null;
            }
            catch (Exception ex) {
                userManagerResult.ErrorMessage = ex.Message;
                return userManagerResult;
            }
        }

        public static UserRoleManagerResult GetUserRoles(string email) {
            UserRoleManagerResult userManagerResult = new UserRoleManagerResult();
            List<Role> roles = new List <Role>();
            List <string> userRoles = new List <string>();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    roles.AddRange(alenMotorsDbEntities.Roles);
                    foreach (Account account in alenMotorsDbEntities.Accounts.ToList()) {
                        if (account.Email.Replace(" ", string.Empty) == email) {
                            foreach (AccountInRole accountInRole in account.AccountsInRoles.ToList()) {
                                userRoles.AddRange(from role in roles where role.RoleID == accountInRole.RoleID select role.RoleName.Replace(" ", string.Empty));
                                userManagerResult.Roles = userRoles.ToArray();
                                return userManagerResult;
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                userManagerResult.ErrorMessage = ex.Message;
                return userManagerResult;
            }
            return null;
        }
    }
}