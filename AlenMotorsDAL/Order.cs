//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AlenMotorsDAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        public int OrderID { get; set; }
        public int AccountID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int VehicleID { get; set; }
        public bool Status { get; set; }
        public int RentDays { get; set; }
    
        public virtual Account Account { get; set; }
        public virtual Vehicle Vehicle { get; set; }
    }
}
