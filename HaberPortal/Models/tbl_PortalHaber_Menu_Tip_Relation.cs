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
    
    public partial class tbl_PortalHaber_Menu_Tip_Relation
    {
        public int Id { get; set; }
        public Nullable<int> TipId { get; set; }
        public Nullable<int> MenuId { get; set; }
    
        public virtual tbl_PortalHaber_Menu tbl_PortalHaber_Menu { get; set; }
        public virtual tbl_PortalHaber_Tip tbl_PortalHaber_Tip { get; set; }
    }
}
