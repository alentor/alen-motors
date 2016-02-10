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
        /// <returns>Retuns true if the registration Succeeded, else returns string with the error message</returns>
        public static UserManagerResult Register(string email,
                                                 string firstName,
                                                 string lastName,
                                                 string password,
                                                 string gender,
                                                 int phoneNumber,
                                                 string birthDate) {
            UserManagerResult userManagerResult = new UserManagerResult();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    if (alenMotorsDbEntities.Accounts.Any(account => account.Email == email)) {
                        userManagerResult.ErrorMessage = "Email is already in use, please use a different one.";
                        return userManagerResult;
                    }
                    alenMotorsDbEntities.Accounts.Add(new Account {
                        Email = email, FirstName = firstName, LastName = lastName, Password = UserManagerGeneric.EncodePassword(password, email),
                        Gender = gender, BirthDate = birthDate,PhoneNumber = phoneNumber, RegistrationDate = DateTime.Now.ToString("{0:d/M/yyyy HH:mm:ss}")
                    });
                    alenMotorsDbEntities.SaveChanges();
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
        /// <returns>Return true on successful validation, else a string with the error message</returns>
        public static UserManagerResult Login(string email, string password) {
            UserManagerResult userManagerResult = new UserManagerResult();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    foreach (var account in alenMotorsDbEntities.Accounts) {
                        if (account.Email.Replace(" ", string.Empty) == email && account.Password.Replace(" ", string.Empty) == UserManagerGeneric.EncodePassword(password, email))
                        {
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
    }
}