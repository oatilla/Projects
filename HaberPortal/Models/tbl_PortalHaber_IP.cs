//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HaberPortal.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_PortalHaber_IP
    {
        public int IP_ID { get; set; }
        public Nullable<int> Haber_Id { get; set; }
        public string IP { get; set; }
    
        public virtual tbl_PortalHaber_Icerik tbl_PortalHaber_Icerik { get; set; }
    }
}
