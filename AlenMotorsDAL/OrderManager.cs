using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlenMotorsDAL {
    public static class OrderManager {
        public static OrderManagerResult AddOrder(string email, string vehicleID, string startDate, string endDate) {
            OrderManagerResult orderManagerResult = new OrderManagerResult();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    foreach (Vehicle vehicle in alenMotorsDbEntities.Vehicles.ToList()) {
                        if (vehicle.VehicleID != int.Parse(vehicleID)) {
                            continue;
                        }
                        if (vehicle.Status != "Avalible") {
                            continue;
                        }
                        Order newOrder = new Order {StartDate = startDate, EndDate = endDate, Vehicle = vehicle, Status = false};
                        alenMotorsDbEntities.Orders.Add(newOrder);
                        alenMotorsDbEntities.SaveChanges();
                        orderManagerResult.Success = true;
                        return orderManagerResult;
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