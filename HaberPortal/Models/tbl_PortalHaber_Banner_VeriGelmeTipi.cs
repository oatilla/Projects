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
    
    public partial class tbl_PortalHaber_Banner_VeriGelmeTipi
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_PortalHaber_Banner_VeriGelmeTipi()
        {
            this.tbl_PortalHaber_Banner_Site_Relation = new HashSet<tbl_PortalHaber_Banner_Site_Relation>();
        }
    
        public int BT_Id { get; set; }
        public string Adi { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_PortalHaber_Banner_Site_Relation> tbl_PortalHaber_Banner_Site_Relation { get; set; }
    }
}
