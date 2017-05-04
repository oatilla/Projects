using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HaberPortal.Models
{
    public class EssayModel
    {
        [Required(ErrorMessage = "Lütfen Başlık Giriniz !!!")]
        public string Baslik { get; set; }
        [Required(ErrorMessage = "Lütfen Açıklama Giriniz !!!")]
        public string Aciklama { get; set; }
        public int KategoriId { get; set; }
        public string HaberUrl { get; set; }
        public int KaynakId { get; set; }
        public int SiteId { get; set; }
        public DateTime OnayTarih { get; set; }

        [Required(ErrorMessage = "Dosya Seç .JPG, .JPEG or .PNG file")]
        public HttpPostedFileBase HaberResimFile { get; set; }

        public string HaberResimFileText { get; set; }
        public bool Onay { get; set; }
        public int BannerId { get; set; }
        public string Email { get; set; }
        [Required(ErrorMessage = "Lütfen Habaer İçeriği Giriniz !!!")]
        public string EssayContent { get; set; }
        public string GondermeTarihi { get; set; }
        [Required(ErrorMessage = "En Az Bir Etiket Giriniz !!!")]
        public string Hastags { get; set; }
        public List<SelectListItem> DropDownListKategori { get; set; }
        public List<SelectListItem> DropDownListSite { get; set; }
        public List<SelectListItem> DropDownListKaynak { get; set; }
        public List<SelectListItem> DropDownListBanner { get; set; }
        public string UserEmail { get; set; }
        public  static string ActionType { get; set; }
        public static int UpdateId { get; set; }
    }
}