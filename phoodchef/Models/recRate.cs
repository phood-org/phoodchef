//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace phoodchef.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class recRate
    {
        public int userId { get; set; }
        public int recId { get; set; }
        public Nullable<int> rate { get; set; }
    
        public virtual enduser enduser { get; set; }
        public virtual recipe recipe { get; set; }
    }
}
