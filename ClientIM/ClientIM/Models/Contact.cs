//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClientIM.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Contact
    {
        public int contact_id { get; set; }
        public int person_id { get; set; }
        public string type_info { get; set; }
        public string info { get; set; }
    
        public virtual Client Client { get; set; }
    }
}
