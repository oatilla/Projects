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
    
    public partial class tbl_PortalHaber_Icerik
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_PortalHaber_Icerik()
        {
            this.tbl_PortalHaber_IP = new HashSet<tbl_PortalHaber_IP>();
            this.tbl_PortalHaber_OkunmaSayisi = new HashSet<tbl_PortalHaber_OkunmaSayisi>();
        }
    
        public int HaberId { get; set; }
        public string HaberBaslik { get; set; }
        public string HaberAciklama { get; set; }
        public Nullable<int> KategoriId { get; set; }
        public string HaberUrl { get; set; }
        public Nullable<int> KaynakId { get; set; }
        public Nullable<int> Site_Id { get; set; }
        public Nullable<System.DateTime> Onay_Tarih { get; set; }
        public string Haber_Resim { get; set; }
        public Nullable<bool> Onay { get; set; }
        public Nullable<int> Banner_Tip { get; set; }
    
        public virtual tbl_PortalHaber_kategori tbl_PortalHaber_kategori { get; set; }
        public virtual tbl_PortalHaber_Kaynak tbl_PortalHaber_Kaynak { get; set; }
        public virtual tbl_PortalHaber_Site tbl_PortalHaber_Site { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_PortalHaber_IP> tbl_PortalHaber_IP { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_PortalHaber_OkunmaSayisi> tbl_PortalHaber_OkunmaSayisi { get; set; }
    }
}