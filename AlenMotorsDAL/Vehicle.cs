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
    
    public partial class Vehicle
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Vehicle()
        {
            this.Orders = new HashSet<Order>();
        }
    
        public int VehicleID { get; set; }
        public int LicensePlate { get; set; }
        public string VehicleManufacturer { get; set; }
        public string VehicleManufacturYear { get; set; }
        public string VehicleModel { get; set; }
        public int VehicleDistanceTraveled { get; set; }
        public string VehicleTransmission { get; set; }
        public string VehicleStatus { get; set; }
        public int BranchID { get; set; }
    
        public virtual Branch Branch { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}