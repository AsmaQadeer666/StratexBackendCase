//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace stratex.Schedule.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class Break
    {
        public decimal id { get; set; }
        public System.DateTime StartDateTime { get; set; }
        public System.DateTime EndDateTime { get; set; }
        public decimal ActivityID { get; set; }
        public decimal EmployeeID { get; set; }
    
        public virtual Activity Activity { get; set; }
        public virtual Employee Employee { get; set; }
    }
}