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
        /// <param name="roleNameNew">Name of the new role</param>
        /// <returns>Return true on successful operation, else a string with the error message</returns>
        public static UserRoleManagerResult AddRole(string roleNameNew) {
            UserRoleManagerResult userManagerResult = new UserRoleManagerResult();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    UserRoleManagerResult checkRoles = GetRoles();
                    // Check if the role already exists, case senstive. Example  User = user will return as the role already exists
                    if (
                    checkRoles.Roles.Select(roleName => roleName.Equals(roleNameNew, StringComparison.CurrentCultureIgnoreCase)).
                               Any(stringEqual => stringEqual)) {
                        userManagerResult.ErrorMessage = "This Role already exists";
                        return userManagerResult;
                    }
                    alenMotorsDbEntities.Roles.Add(new Role {RoleName = roleNameNew});
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
        /// <returns>Return true on successful operation, else a string with an error message</returns>
        public static UserRoleManagerResult AddRoleToUser(string email, string roleName) {
            UserRoleManagerResult userManagerResult = new UserRoleManagerResult();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    foreach (Account account in alenMotorsDbEntities.Accounts.ToList()) {
                        if (account.Email.Replace(" ", string.Empty) == email) {
                            Role roleToAdd = Enumerable.FirstOrDefault(alenMotorsDbEntities.Roles,
                                                                       role => role.RoleName.Replace(" ", string.Empty) == roleName);
                            UserRoleManagerResult userInRoles = GetUserRoles(email);
                            if (userInRoles.Roles.Any(userInRole => userInRole == roleName)) {
                                userManagerResult.ErrorMessage = "User already in role";
                                return userManagerResult;
                            }
                            account.AccountsInRoles.Add(new AccountInRole {AccountID = account.AccountID, RoleID = roleToAdd.RoleID});
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

        public static UserRoleManagerResult RemoveRole(string roleName) {
            UserRoleManagerResult userManagerResult = new UserRoleManagerResult();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    int roleIdToRemove;
                    foreach (Role role in alenMotorsDbEntities.Roles.ToList().Where(role => role.RoleName.Replace(" ", String.Empty) == roleName)) {
                        roleIdToRemove = role.RoleID;
                        foreach (AccountInRole accountInRole in
                        alenMotorsDbEntities.AccountInRoles.ToList().Where(accountInRolein => accountInRolein.RoleID == roleIdToRemove)) {
                            alenMotorsDbEntities.AccountInRoles.Remove(accountInRole);
                        }
                        alenMotorsDbEntities.SaveChanges();
                        userManagerResult.Success = true;
                        return userManagerResult;
                    }
                }
            }
            catch (Exception ex) {
                userManagerResult.ErrorMessage = ex.Message;
                return userManagerResult;
            }
            return userManagerResult;
        }

        /// <summary>
        /// Removes a role from the user
        /// </summary>
        /// <param name="email">The user's email</param>
        /// <param name="roleName">The role to remove from the user</param>
        /// <returns>Return true on successful operation, else a string with an error message</returns>
        public static UserRoleManagerResult RemoveRoleFromUser(string email, string roleName) {
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

        /// <summary>
        /// Gets all the Roles which the user belongs to, retunrs string[]
        /// </summary>
        /// <param name="email">The users mail</param>
        /// <returns>Returns all the names of roles which the user belongs to (string[]), else a string with an error message</returns>
        public static UserRoleManagerResult GetUserRoles(string email) {
            UserRoleManagerResult userManagerResult = new UserRoleManagerResult();
            List <Role> roles = new List <Role>();
            List <string> userRoles = new List <string>();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    roles.AddRange(alenMotorsDbEntities.Roles);
                    foreach (Account account in alenMotorsDbEntities.Accounts.ToList()) {
                        if (account.Email.Replace(" ", string.Empty) == email) {
                            foreach (AccountInRole accountInRole in account.AccountsInRoles.ToList()) {
                                userRoles.AddRange(from role in roles
                                                   where role.RoleID == accountInRole.RoleID
                                                   select role.RoleName.Replace(" ", string.Empty));
                            }
                            userManagerResult.Roles = userRoles;
                            userManagerResult.Success = true;
                            return userManagerResult;
                        }
                    }
                    return null;
                }
            }
            catch (Exception ex) {
                userManagerResult.ErrorMessage = ex.Message;
                return userManagerResult;
            }
        }

        /// <summary>
        /// Get all roles names List<string>
        /// </summary>
        /// <returns>Returns a List<string> of all roles names, else a string with an error message </string></returns>
        public static UserRoleManagerResult GetRoles() {
            UserRoleManagerResult userManagerResult = new UserRoleManagerResult();
            List <string> roles = new List <string>();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    roles.AddRange(alenMotorsDbEntities.Roles.ToList().Select(role => role.RoleName.Replace(" ", string.Empty)));
                }
                userManagerResult.Success = true;
                userManagerResult.Roles = roles;
                return userManagerResult;
            }
            catch (Exception ex) {
                userManagerResult.ErrorMessage = ex.Message;
                return userManagerResult;
            }
        }
    }
}