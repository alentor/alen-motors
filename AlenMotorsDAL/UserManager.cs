using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AlenMotorsDAL {
    public static class UserManager {
        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="password"></param>
        /// <param name="gender"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="birthDate"></param>
        /// <returns>Retuns true if the registration Succeeded, else returns string with an error message</returns>
        public static UserManagerResult Register(string email, string firstName, string lastName, string password, string gender, int phoneNumber,
                                                 string birthDate) {
            UserManagerResult userManagerResult = new UserManagerResult();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    if (alenMotorsDbEntities.Accounts.Any(account => account.Email == email)) {
                        userManagerResult.ErrorMessage = "Email is already in use, please use a different one.";
                        return userManagerResult;
                    }
                    alenMotorsDbEntities.Accounts.Add(new Account {
                        Email = email,
                        FirstName = firstName,
                        LastName = lastName,
                        Password = UserManagerGeneric.EncodePassword(password, email),
                        Gender = gender,
                        BirthDate = birthDate,
                        PhoneNumber = phoneNumber,
                        RegistrationDate = DateTime.Now.ToString("{0:d/M/yyyy HH:mm:ss}")
                    });
                    //AccountInRole accountInRole = new AccountInRole();
                    //accountInRole.RoleID = 2;
                    //alenMotorsDbEntities.SaveChanges();
                    userManagerResult.Success = true;
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
                    foreach (Account account in alenMotorsDbEntities.Accounts) {
                        if (account.Email.Replace(" ", string.Empty) == email &&
                            account.Password.Replace(" ", string.Empty) == UserManagerGeneric.EncodePassword(password, email)) {
                            userManagerResult.Success = true;
                            return userManagerResult;
                        }
                    }
                    userManagerResult.ErrorMessage = "Email and Password don't match.";
                    return userManagerResult;
                }
            }
            catch (Exception ex) {
                userManagerResult.ErrorMessage = ex.Message;
                return userManagerResult;
            }
        }

        /// <summary>
        /// Returns the infrmation that corresponds to the provided email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>Returns all the corresponds information (Account object), else a string with an error message</returns>
        public static UserManagerResult GetUserInformation(string email) {
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
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="updateUser"></param>
        /// <returns></returns>
        public static UserManagerResult UpdateUser(string email, Account updateUser) {
            UserManagerResult userManagerResult = new UserManagerResult();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    foreach (Account account in alenMotorsDbEntities.Accounts.ToList()) {
                        if (account.Email.Replace(" ", string.Empty) == email &&
                            account.Password.Replace(" ", string.Empty) == UserManagerGeneric.EncodePassword(updateUser.Password, email)) {
                            account.LastName = updateUser.LastName;
                            account.FirstName = updateUser.FirstName;
                            account.BirthDate = updateUser.BirthDate;
                            account.Gender = updateUser.Gender;
                            account.PhoneNumber = updateUser.PhoneNumber;
                            // We are using registration date to pass the new password
                            if (updateUser.RegistrationDate != null) {
                                account.Password = UserManagerGeneric.EncodePassword(updateUser.RegistrationDate, email);
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

            return null;
        }
    }
}