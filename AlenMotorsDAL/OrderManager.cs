using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlenMotorsDAL {
    public static class OrderManager {
        /// <summary>
        /// Add a new Order
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="vehicleID">vehicleID can be null</param>
        /// <param name="startDate">startDate</param>
        /// <param name="endDate">endDate</param>
        /// <param name="rentDays">rentDays</param>
        /// <returns>Retuns true if the addition has Succeeded, else returns string with an error message</returns>
        public static OrderManagerResult AddOrder(string email, string vehicleID, string startDate, string endDate, string rentDays) {
            OrderManagerResult orderManagerResult = new OrderManagerResult();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    foreach (Vehicle vehicle in alenMotorsDbEntities.Vehicles.ToList()) {
                        if (vehicle.VehicleID == int.Parse(vehicleID)) {
                            if (vehicle.Status == "Not Avalible") {
                                orderManagerResult.ErrorMessage = "This Vehicle isn't available right now";
                                return orderManagerResult;
                            }
                            // Vehicle = vehicle => maybe instead would've been better to use ID
                            Order newOrder = new Order {
                                StartDate = startDate,
                                EndDate = endDate,
                                Vehicle = vehicle,
                                Status = false,
                                RentDays = int.Parse(rentDays)
                            };
                            foreach (Account account in alenMotorsDbEntities.Accounts.ToList()) {
                                if (account.Email.Replace(" ", String.Empty) == email) {
                                    account.Orders.Add(newOrder);
                                    //alenMotorsDbEntities.Orders.Add(newOrder);
                                    vehicle.Status = "Not Available";
                                    alenMotorsDbEntities.SaveChanges();
                                    orderManagerResult.Success = true;
                                    return orderManagerResult;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                orderManagerResult.ErrorMessage = ex.Message;
                return orderManagerResult;
            }
            orderManagerResult.ErrorMessage = "Something went wrong";
            return orderManagerResult;
        }

        /// <summary>
        /// Gets the orders for the following email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Retuns true if the operation has Succeeded, else returns string with an error message</returns>
        public static OrderManagerResult GetOrders(string email) {
            OrderManagerResult orderManagerResult = new OrderManagerResult();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    foreach (Account account in alenMotorsDbEntities.Accounts.ToList()) {
                        if (account.Email.Replace(" ", String.Empty) == email) {
                            foreach (Order order in account.Orders.ToList()) {
                                orderManagerResult.Orders.Add(order);
                            }
                            orderManagerResult.Success = true;
                            return orderManagerResult;
                        }
                    }
                }
            }
            catch (Exception ex) {
                orderManagerResult.ErrorMessage = ex.Message;
                return orderManagerResult;
            }
            return null;
        }

        /// <summary>
        /// Gets a specific order
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>Retuns true on successful operation, else returns string with an error message</returns>
        public static OrderManagerResult GetOrder(string orderID) {
            OrderManagerResult orderManagerResult = new OrderManagerResult();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    foreach (Order order in alenMotorsDbEntities.Orders.ToList()) {
                        if (order.OrderID == int.Parse(orderID)) {
                            orderManagerResult.Order = order;
                            orderManagerResult.Success = true;
                            return orderManagerResult;
                        }
                    }
                }
            }
            catch (Exception ex) {
                orderManagerResult.ErrorMessage = ex.Message;
                return orderManagerResult;
            }
            return null;
        }


        public static OrderManagerResult GetAllOrdersAndWhicles() {
            return null;
        }

        /// <summary>
        /// returns a List<orders> of all orders 
        /// </summary>
        /// <returns>Retuns true on successful operation, else returns string with an error message</returns>
        public static OrderManagerResult GetAllOrders() {
            OrderManagerResult orderManagerResult = new OrderManagerResult();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    foreach (Order order in alenMotorsDbEntities.Orders.ToList()) {
                        orderManagerResult.Orders.Add(order);
                    }
                    orderManagerResult.Success = true;
                    return orderManagerResult;
                }
            }
            catch (Exception ex) {
                orderManagerResult.ErrorMessage = ex.Message;
                return orderManagerResult;
            }
        }

        /// <summary>
        /// Edits an order
        /// </summary>
        /// <param name="orderToEdit">The order to edit</param>
        /// <param name="newEmail">The new email(owner) for the order</param>
        /// <returns>Retuns true on successful operation, else returns string with an error message</returns>
        public static OrderManagerResult EditOrder(Order orderToEdit, string newEmail) {
            OrderManagerResult orderManagerResult = new OrderManagerResult();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    //accountID is zero
                    //Find to whom the order belongs down below.. 
                    UserManagerResult userManagerResult = UserManager.GetUserByOrderID(orderToEdit.OrderID);
                    if (!userManagerResult.Success) {
                        orderManagerResult.ErrorMessage = "Something went wrong";
                        return orderManagerResult;
                    }
                    foreach (Order order in alenMotorsDbEntities.Orders.ToList()) {
                        if (order.OrderID == orderToEdit.OrderID) {
                            if (orderToEdit.StartDate != null) {
                                order.EndDate = orderToEdit.StartDate;
                            }
                            if (orderToEdit.EndDate != null) {
                                order.EndDate = orderToEdit.EndDate;
                            }
                            if (!order.Status.Equals(orderToEdit.Status)) {
                                foreach (Vehicle vehicle in alenMotorsDbEntities.Vehicles.ToList()) {
                                    if (order.VehicleID == vehicle.VehicleID) {
                                        if (vehicle.Status.Replace(" ", String.Empty) == "Available") {
                                            vehicle.Status = "Not Available";
                                            break;
                                        }
                                        vehicle.Status = "Available";
                                    }
                                }
                            }
                            order.Status = orderToEdit.Status;
                            userManagerResult.User.Orders.Remove(order);
                            if (userManagerResult.User.Email.Replace(" ", String.Empty) != newEmail.Replace(" ", String.Empty)) {
                                foreach (Account account in alenMotorsDbEntities.Accounts.ToList()) {
                                    if (account.Email.Replace(" ", String.Empty) == newEmail.Replace(" ", String.Empty)) {
                                        userManagerResult.User.Orders.Remove(order);
                                        account.Orders.Add(order);
                                        alenMotorsDbEntities.SaveChanges();
                                        orderManagerResult.Success = true;
                                        return orderManagerResult;
                                    }
                                }
                                orderManagerResult.ErrorMessage = "Couldn't find the email";
                                return orderManagerResult;
                            }
                            alenMotorsDbEntities.SaveChanges();
                            orderManagerResult.Success = true;
                            return orderManagerResult;
                        }
                    }
                }
            }
            catch (Exception ex) {
                orderManagerResult.ErrorMessage = ex.Message;
                return orderManagerResult;
            }
            return null;
        }

        public static OrderManagerResult RemoveOrder(int orderID) {
            OrderManagerResult orderManagerResult = new OrderManagerResult();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    foreach (Order order in alenMotorsDbEntities.Orders.ToList()) {
                        if (order.OrderID == orderID) {
                            alenMotorsDbEntities.Orders.Remove(order);
                            alenMotorsDbEntities.SaveChanges();
                            orderManagerResult.Success = true;
                            return orderManagerResult;
                        }
                    }
                }
            }
            catch (Exception ex) {
                orderManagerResult.ErrorMessage = ex.Message;
                return orderManagerResult;
            }
            return null;
        }
    }
}