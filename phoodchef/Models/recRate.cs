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
        public int UserId { get; set; }
        public int RecId { get; set; }
        public Nullable<int> Rate { get; set; }
        public Nullable<bool> IsFavorite { get; set; }
        public string Review { get; set; }
    
        public virtual enduser enduser { get; set; }
        public virtual recipe recipe { get; set; }
    }
}
