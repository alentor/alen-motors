using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AlenMotorsDAL {
    public static class UserManager {
        /// <summary>
        /// Add a new user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="password"></param>
        /// <param name="gender"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="birthDate"></param>
        /// <returns>Retuns true if the addition has Succeeded, else returns string with an error message</returns>
        public static UserManagerResult AddUser(string email, string firstName, string lastName, string password, string gender, int phoneNumber,
                                                string birthDate) {
            UserManagerResult userManagerResult = new UserManagerResult();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    string verifyString = Generic.VerifyEmailHash(email);
                    if (alenMotorsDbEntities.Accounts.Any(account => account.Email == email)) {
                        userManagerResult.ErrorMessage = "Email is already in use, please use a different one.";
                        return userManagerResult;
                    }
                    alenMotorsDbEntities.Accounts.Add(new Account {
                        Email = email,
                        FirstName = firstName,
                        LastName = lastName,
                        Password = Generic.EncodePassword(password, email),
                        Gender = gender,
                        BirthDate = birthDate,
                        PhoneNumber = phoneNumber,
                        RegistrationDate = DateTime.Now.ToString("{0:d/M/yyyy HH:mm:ss}"),
                        VerifyString = verifyString
                    });
                    alenMotorsDbEntities.SaveChanges();
                    UserRoleManagerResult addUserToRole = UserRoleManager.AddRoleToUser(email, "User");
                    if (addUserToRole.Success) {
                        //
                        userManagerResult.VerifyString = verifyString;
                        userManagerResult.Success = true;
                        return userManagerResult;
                    }
                    userManagerResult.ErrorMessage = "Some thing went wrong";
                    return userManagerResult;
                }
            }
            catch (Exception ex) {
                userManagerResult.ErrorMessage = ex.Message;
                return userManagerResult;
            }
        }
        /// <summary>
        /// Verifies the account
        /// </summary>
        /// <param name="email"></param>
        /// <param name="verifyString"></param>
        /// <returns>Retuns true if the verification was correct, else returns string with an error message</returns>
        public static UserManagerResult VerifyUser(string email, string verifyString) {
            UserManagerResult userManagerResult = new UserManagerResult();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    foreach (Account account in alenMotorsDbEntities.Accounts.ToList()) {
                        if (account.Email.Replace(" ", String.Empty) == email && account.VerifyString.Replace(" ", String.Empty) == verifyString) {
                            account.Verified = true;
                            alenMotorsDbEntities.SaveChanges();
                            userManagerResult.Success = true;
                            return userManagerResult;
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

        /// <summary>
        /// Removes an existing user
        /// </summary>
        /// <param name="email">The serach parameter at which to remove the user</param>
        /// <returns>Retuns true if the removal has Succeeded, else returns string with an error message</returns>
        public static UserManagerResult RemoveUser(string email) {
            UserManagerResult userManagerResult = new UserManagerResult();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    foreach (
                    Account account in alenMotorsDbEntities.Accounts.ToList().Where(account => account.Email.Replace(" ", String.Empty) == email)) {
                        foreach (AccountInRole accountInRole in account.AccountsInRoles.ToList()) {
                            account.AccountsInRoles.Remove(accountInRole);
                        }
                        foreach (Order order in account.Orders) {
                            account.Orders.Remove(order);
                        }
                        alenMotorsDbEntities.Accounts.Remove(account);
                        alenMotorsDbEntities.SaveChanges();
                        userManagerResult.Success = true;
                        return userManagerResult;
                    }
                    return userManagerResult;
                }
            }
            catch (Exception ex) {
                userManagerResult.ErrorMessage = ex.Message;
                return userManagerResult;
            }
        }

        /// <summary>
        /// Validates if the provided credentials are correct
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="password">Password</param>
        /// <returns>Return true on successful validation, else a string with an error message</returns>
        public static UserManagerResult Login(string email, string password) {
            UserManagerResult userManagerResult = new UserManagerResult();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    foreach (Account account in alenMotorsDbEntities.Accounts.ToList()) {
                        if (account.Email.Replace(" ", String.Empty) == email) {
                            if (account.Verified) {
                                if (account.Password.Replace(" ", string.Empty) == Generic.EncodePassword(password, email)) {
                                    userManagerResult.Success = true;
                                    return userManagerResult;
                                }
                                userManagerResult.ErrorMessage = "Email and Password don't match.";
                                return userManagerResult;
                            }
                            userManagerResult.ErrorMessage = "You haven't verified your account, please check your email and verify your account";
                            return userManagerResult;
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

        /// <summary>
        /// Returns the infrmation that corresponds to the provided email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>Returns all the corresponds information (Account object), else a string with an error message</returns>
        public static UserManagerResult GetUser(string email) {
            UserManagerResult userManagerResult = new UserManagerResult();
            Account user = new Account();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    foreach (Account account in alenMotorsDbEntities.Accounts) {
                        if (account.Email.Replace(" ", string.Empty) != email) {
                            continue;
                        }
                        user.Email = account.Email;
                        user.LastName = account.LastName;
                        user.FirstName = account.FirstName;
                        user.Gender = account.Gender;
                        user.PhoneNumber = account.PhoneNumber;
                        user.BirthDate = account.BirthDate;
                        userManagerResult.User = user;
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
        /// Update user
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="userAccount">User account [Account]</param>
        /// <param name="additionalinformation">New password</param>
        /// <returns>Return true on successful update, else a string with an error message</returns>
        public static UserManagerResult EditUser(string email, Account userAccount, string additionalinformation) {
            UserManagerResult userManagerResult = new UserManagerResult();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    // Allternative 1
                    if (email != userAccount.Email) {
                        foreach (Account account0 in alenMotorsDbEntities.Accounts.ToList()) {
                            if (account0.Email.Replace(" ", string.Empty) == email &&
                                account0.Password.Replace(" ", string.Empty) == Generic.EncodePassword(additionalinformation, email)) {
                                foreach (Account account1 in alenMotorsDbEntities.Accounts.ToList()) {
                                    if (userAccount.Email == account1.Email.Replace(" ", String.Empty)) {
                                        if (userAccount.RegistrationDate != null) {
                                            account1.Email = userAccount.RegistrationDate;
                                        }
                                        account1.LastName = userAccount.LastName;
                                        account1.FirstName = userAccount.FirstName;
                                        account1.BirthDate = userAccount.BirthDate;
                                        account1.Gender = userAccount.Gender;
                                        account1.PhoneNumber = userAccount.PhoneNumber;
                                        if (userAccount.Password != null) {
                                            account1.Password = Generic.EncodePassword(userAccount.Password, userAccount.Email);
                                        }
                                        userManagerResult.Success = true;
                                        alenMotorsDbEntities.SaveChanges();
                                        return userManagerResult;
                                    }
                                }
                            }
                        }
                        userManagerResult.Success = false;
                        return userManagerResult;
                    }

                    // Allternative 2
                    if (userAccount.Password != null) {
                        foreach (Account account in alenMotorsDbEntities.Accounts.ToList()) {
                            if (account.Email.Replace(" ", string.Empty) == email &&
                                account.Password.Replace(" ", string.Empty) == Generic.EncodePassword(userAccount.Password, email)) {
                                account.LastName = userAccount.LastName;
                                account.FirstName = userAccount.FirstName;
                                account.BirthDate = userAccount.BirthDate;
                                account.Gender = userAccount.Gender;
                                account.PhoneNumber = userAccount.PhoneNumber;
                                if (additionalinformation != null) {
                                    account.Password = Generic.EncodePassword(additionalinformation, email);
                                }
                                if (email != account.Email.Replace(" ", String.Empty)) {
                                    account.Email = email;
                                }
                                userManagerResult.Success = true;
                                alenMotorsDbEntities.SaveChanges();
                                return userManagerResult;
                            }
                        }
                    }

                    // Allternative 3
                    foreach (Account account in alenMotorsDbEntities.Accounts.ToList()) {
                        if (account.Email.Replace(" ", string.Empty) == email &&
                            account.Password.Replace(" ", string.Empty) == Generic.EncodePassword(additionalinformation, email)) {
                            account.LastName = userAccount.LastName;
                            account.FirstName = userAccount.FirstName;
                            account.BirthDate = userAccount.BirthDate;
                            account.Gender = userAccount.Gender;
                            account.PhoneNumber = userAccount.PhoneNumber;
                            if (additionalinformation != null) {
                                account.Password = Generic.EncodePassword(additionalinformation, email);
                            }
                            if (email != account.Email.Replace(" ", String.Empty)) {
                                account.Email = email;
                            }
                            userManagerResult.Success = true;
                            alenMotorsDbEntities.SaveChanges();
                            return userManagerResult;
                        }
                    }
                }
                userManagerResult.Success = false;
                return userManagerResult;
            }
            catch (Exception ex) {
                userManagerResult.ErrorMessage = ex.Message;
                return userManagerResult;
            }
        }

        /// <summary>
        /// Returns the list of all users
        /// </summary>
        /// <returns>Return true on successful action as well as List<Account> of all users, else a string with an error message</returns>
        public static UserManagerResult GetUsers() {
            UserManagerResult userManagerResult = new UserManagerResult();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    foreach (Account account in alenMotorsDbEntities.Accounts.ToList()) {
                        userManagerResult.Success = true;
                        userManagerResult.UserList.Add(account);
                    }
                    return userManagerResult;
                }
            }
            catch (Exception ex) {
                userManagerResult.ErrorMessage = ex.Message;
                return userManagerResult;
            }
        }

        public static UserManagerResult GetUserByUserID(int userID) {
            UserManagerResult userManagerResult = new UserManagerResult();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    foreach (Account account in alenMotorsDbEntities.Accounts.ToList()) {
                        if (account.AccountID == userID) {
                            userManagerResult.User = account;
                            userManagerResult.Success = true;
                            return userManagerResult;
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

        public static UserManagerResult GetUserByOrderID(int orderID) {
            UserManagerResult userManagerResult = new UserManagerResult();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    foreach (Account account in alenMotorsDbEntities.Accounts.ToList()) {
                        if (account.Orders.ToList().Any(Order => Order.OrderID == orderID)) {
                            userManagerResult.User = account;
                            userManagerResult.Success = true;
                            return userManagerResult;
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