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
    
    public partial class tbl_PortalHaber_YazarParaAyar
    {
        public int YPid { get; set; }
        public Nullable<double> Para { get; set; }
        public Nullable<System.DateTime> Tarih { get; set; }
        public Nullable<int> SiteGrupId { get; set; }
    
        public virtual tbl_PortalHaber_SiteGrubu tbl_PortalHaber_SiteGrubu { get; set; }
    }
}