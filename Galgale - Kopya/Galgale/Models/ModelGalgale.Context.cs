﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Galgale.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DB090928093827Entities : DbContext
    {
        public DB090928093827Entities()
            : base("name=DB090928093827Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tbl_PortalHaber_Icerik> tbl_PortalHaber_Icerik { get; set; }
        public virtual DbSet<tbl_PortalHaber_kategori> tbl_PortalHaber_kategori { get; set; }
        public virtual DbSet<tbl_PortalHaber_Kaynak> tbl_PortalHaber_Kaynak { get; set; }
        public virtual DbSet<tbl_PortalHaber_OkunmaSayisi> tbl_PortalHaber_OkunmaSayisi { get; set; }
        public virtual DbSet<tbl_PortalHaber_Site> tbl_PortalHaber_Site { get; set; }
        public virtual DbSet<tbl_PortalHaber_Tip> tbl_PortalHaber_Tip { get; set; }
        public virtual DbSet<tbl_PortalHaber_Yazar> tbl_PortalHaber_Yazar { get; set; }
    }
}
