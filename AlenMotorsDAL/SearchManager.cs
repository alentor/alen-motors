using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlenMotorsDAL {
    public static class SearchManager {
        public static SearchManagerResult ViewOrdersContainsSearch(string searchStr) {
            SearchManagerResult searchManagerResult = new SearchManagerResult();
            List <Order> searchResult = new List <Order>();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    foreach (Order order in alenMotorsDbEntities.Orders.ToList()) {
                        if (order.Account.Email.Replace(" ", String.Empty).IndexOf(searchStr, StringComparison.CurrentCultureIgnoreCase) != -1) {
                            searchResult.Add(order);
                        }
                        if (order.Vehicle.Color.Replace(" ", String.Empty).IndexOf(searchStr, StringComparison.CurrentCultureIgnoreCase) != -1) {
                            searchResult.Add(order);
                        }
                        if (order.Vehicle.Manufacturer.Replace(" ", String.Empty).IndexOf(searchStr, StringComparison.CurrentCultureIgnoreCase) != -1) {
                            searchResult.Add(order);
                        }
                        if (order.Vehicle.Model.Replace(" ", String.Empty).IndexOf(searchStr, StringComparison.CurrentCultureIgnoreCase) != -1) {
                            searchResult.Add(order);
                        }

                        if (
                        order.Vehicle.Branch.BranchName.Replace(" ", String.Empty).IndexOf(searchStr, StringComparison.CurrentCultureIgnoreCase) != -1) {
                            searchResult.Add(order);
                        }
                        if (order.Vehicle.Status.Replace(" ", String.Empty).IndexOf(searchStr, StringComparison.CurrentCultureIgnoreCase) != -1) {
                            searchResult.Add(order);
                        }
                        if (searchStr.IndexOf("Available", StringComparison.CurrentCultureIgnoreCase) != -1) {
                            if (order.Status) {
                                searchResult.Add(order);
                            }
                        }
                        if (searchStr.IndexOf("Not Available", StringComparison.CurrentCultureIgnoreCase) != -1) {
                            if (order.Status) {
                                searchResult.Add(order);
                            }
                        }
                    }
                    searchManagerResult.Orders = searchResult.Distinct().ToList();
                    searchManagerResult.Success = true;
                    return searchManagerResult;
                }
            }
            catch (Exception ex) {
                searchManagerResult.ErrorMessage = ex.Message;
                return searchManagerResult;
            }
        }


        public static SearchManagerResult ViewOrdersOrderIdSearch(string searchStr) {
            SearchManagerResult searchManagerResult = new SearchManagerResult();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    foreach (Order order in alenMotorsDbEntities.Orders.ToList()) {
                        if (order.OrderID == int.Parse(searchStr)) {
                            searchManagerResult.Orders.Add(order);
                            searchManagerResult.Success = true;
                            return searchManagerResult;
                        }
                    }
                }
            }
            catch (Exception ex) {
                searchManagerResult.ErrorMessage = ex.Message;
                return searchManagerResult;
            }
            return null;
        }


        public static SearchManagerResult ViewOrdersOrderIdGreaterThanSearch(string searchStr) {
            SearchManagerResult searchManagerResult = new SearchManagerResult();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    foreach (Order order in alenMotorsDbEntities.Orders.ToList()) {
                        if (order.OrderID > int.Parse(searchStr)) {
                            searchManagerResult.Orders.Add(order);
                        }
                    }
                    searchManagerResult.Success = true;
                    return searchManagerResult;
                }
            }
            catch (Exception ex) {
                searchManagerResult.ErrorMessage = ex.Message;
                return searchManagerResult;
            }
        }


        public static SearchManagerResult ViewOrdersOrderIdLessThanSearch(string searchStr) {
            SearchManagerResult searchManagerResult = new SearchManagerResult();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    foreach (Order order in alenMotorsDbEntities.Orders.ToList()) {
                        if (order.OrderID < int.Parse(searchStr)) {
                            searchManagerResult.Orders.Add(order);
                        }
                    }
                    searchManagerResult.Success = true;
                    return searchManagerResult;
                }
            }
            catch (Exception ex) {
                searchManagerResult.ErrorMessage = ex.Message;
                return searchManagerResult;
            }
        }


        public static SearchManagerResult ViewOrdersEmaildSearch(string searchStr) {
            SearchManagerResult searchManagerResult = new SearchManagerResult();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    foreach (Account account in alenMotorsDbEntities.Accounts.ToList()) {
                        if (account.Email.Replace("  ", String.Empty)== searchStr) {
                            foreach (Order order in account.Orders) {
                                searchManagerResult.Orders.Add(order);
                            }
                            searchManagerResult.Success = true;
                            return searchManagerResult;
                        }
                    }
                }
            }
            catch (Exception ex) {
                searchManagerResult.ErrorMessage = ex.Message;
                return searchManagerResult;
            }
            return null;
        }
    }
}