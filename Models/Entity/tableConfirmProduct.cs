//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace yazilimYapimi.Models.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class tableConfirmProduct
    {
        public int ID { get; set; }
        public string ProductName { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<bool> Confirmed { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<int> Quantity { get; set; }
    
        public virtual tableUser tableUser { get; set; }
    }
}
