using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaberPortal.Models;
using Microsoft.Owin.Security.DataHandler.Encoder;

namespace HaberPortal.Controllers
{
    public class DbOperation
    {
        DB090928093827Entities context = new DB090928093827Entities();
        public List<HaberModel> GetEssayList()
        {
            //ManageDataContext context = new ManageDataContext();
            //context.Connection.Open();
            
            var list = context.VWYHABERLISTs.ToList();

            var tempList = list.Select(item => new HaberModel
            {
                id = item.id,
                HaberBaslik = item.HaberBaslik,
                EPosta = item.EPosta,
                HaberAciklama = item.HaberAciklama
            }).ToList();

            //context.Connection.Close();
            //context.Connection.Dispose();
            return tempList;
        }

        public EssayModel FillREssayModelFordUpdate(EssayModel essayModel, int id)
        {
            //var context = new ManageDataContext();
            //context.Connection.Open();
            //var icerik = context.tbl_PortalHaber_Iceriks.FirstOrDefault(u => u.HaberId == id);


            var icerikall = (from icerik in context.tbl_PortalHaber_Icerik
                             join icerik2 in context.tbl_PortalHaber_Icerik2
                             on icerik.HaberId equals icerik2.HaberId
                             select new
                             {
                                 icerik.HaberId,
                                 icerik.HaberBaslik,
                                 icerik.HaberAciklama,
                                 icerik.KategoriId,
                                 icerik.HaberUrl,
                                 icerik.KaynakId,
                                 icerik.Site_Id,
                                 icerik.Onay_Tarih,
                                 icerik.Haber_Resim,
                                 icerik.Onay,
                                 icerik.Banner_Tip,
                                 icerik2.HaberMetni,
                                 icerik2.Etiketler,
                                 icerik2.YazarId,
                             }).FirstOrDefault(u => u.HaberId == id);

            //context.Connection.Close();


            //context.Connection.Close();
            if (icerikall != null)
            {
                essayModel.Baslik = icerikall.HaberBaslik;
                essayModel.Aciklama = icerikall.HaberAciklama;
                essayModel.EssayContent = icerikall.HaberMetni;
                essayModel.HaberUrl = icerikall.HaberUrl;
                essayModel.Hastags = icerikall.Etiketler;
                essayModel.KategoriId = (int)icerikall.KategoriId;
                essayModel.KaynakId = (int)icerikall.KaynakId;
                essayModel.SiteId = (int)icerikall.Site_Id;
                essayModel.HaberResimFileText = "~/Img/" + icerikall.Haber_Resim;
            }


            return essayModel;
        }

        public bool CheckUser(string email, string password)
        {
            var isUser = false;
            //ManageDataContext context = new ManageDataContext();
            //context.Connection.Open();
            var result = context.tbl_PortalHaber_Yazar.FirstOrDefault(u => u.EPosta == email && u.Sifre == password);
            //context.Connection.Close();
            //context.Dispose();
            isUser = result != null;
            return isUser;
        }

        public Boolean IsEmailInSystem(string email)
        {
            var isEmail = false;
            //ManageDataContext context = new ManageDataContext();
            //context.Connection.Open();
            var result = context.tbl_PortalHaber_Yazar.FirstOrDefault(u => u.EPosta == email);
            //context.Connection.Close();
            context.Dispose();
            isEmail = result != null;
            return isEmail;
        }

        public bool UpdateEssayToDb(EssayModel model, int id,string useremail)
        {
            var result = false;
            try
            {
                //var context = new ManageDataContext();
                //context.Connection.Open();
                var tblIcerik = context.tbl_PortalHaber_Icerik.FirstOrDefault(u => u.HaberId == id);
                var tblIcerik2 = context.tbl_PortalHaber_Icerik2.FirstOrDefault(u => u.HaberId == id);
                var user = context.tbl_PortalHaber_Yazar.FirstOrDefault(u => u.EPosta == useremail);

                if (tblIcerik != null)
                {
                    tblIcerik.Banner_Tip = model.BannerId;
                    tblIcerik.HaberUrl = model.Baslik.Replace('Ç', 'c').Replace('ç', 'c').Replace(' ', '-').Replace('Ö', 'o').Replace('ö', 'o').Replace('ğ', 'g').Replace('Ğ', 'g').Replace('ü', 'u').Replace('Ü', 'u').Replace('İ', 'i').Replace('Ş', 's').Replace('ş', 's').Replace('ı', 'i').Replace('.', '-').Replace('&', '-').Replace('/', '-');
                    tblIcerik.Haber_Resim = model.HaberResimFile.FileName;
                    tblIcerik.KategoriId = model.KategoriId;
                    tblIcerik.KaynakId = model.KaynakId;
                    tblIcerik.Onay_Tarih = DateTime.Now;
                    tblIcerik.Site_Id = model.SiteId;
                    tblIcerik.HaberAciklama = model.Aciklama;
                    tblIcerik.HaberBaslik = model.Baslik;

                   
                    if (tblIcerik2 != null)
                    {
                        tblIcerik2.Etiketler = model.Hastags;
                        tblIcerik2.HaberMetni = model.EssayContent;
                        if (user != null) tblIcerik2.YazarId = user.YazarId;
                    }
                    context.SaveChanges();
                    //context.SubmitChanges();
                    result = true;

                    //context.Connection.Close();
                    //context.Dispose();
                }

            }
            catch (Exception)
            {
                result = false;
                throw;

            }

            return result;

        }

        public bool InsertEssayToDb(EssayModel model)
        {
            var icerik = new tbl_PortalHaber_Icerik();
            icerik.HaberBaslik = model.Baslik;
            icerik.HaberAciklama = model.Aciklama;
            icerik.KategoriId = model.KategoriId;
            icerik.HaberUrl = model.Baslik.Replace('Ç', 'c').Replace('ç', 'c').Replace(' ', '-').Replace('Ö', 'o').Replace('ö', 'o').Replace('ğ', 'g').Replace('Ğ', 'g').Replace('ü', 'u').Replace('Ü', 'u').Replace('İ', 'i').Replace('Ş', 's').Replace('ş', 's').Replace('ı', 'i').Replace('.', '-').Replace('&', '-').Replace('/', '-');
            icerik.KaynakId = model.KaynakId;
            icerik.Site_Id = model.SiteId;
            icerik.Onay_Tarih = DateTime.Now;
            icerik.Haber_Resim = model.HaberResimFile.FileName;
            icerik.Onay = false;
            icerik.HaberId = model.BannerId;

            //var context = new ManageDataContext();
            //context.Connection.Open();
            context.tbl_PortalHaber_Icerik.Add(icerik);
            context.SaveChanges();
            //context.SubmitChanges();

            var tblPortalHaberIcerik = context.tbl_PortalHaber_Icerik.OrderByDescending(p => p.HaberId).FirstOrDefault();
            if (tblPortalHaberIcerik != null)
            {
                var haberId = tblPortalHaberIcerik.HaberId;
                var icerik2 = new tbl_PortalHaber_Icerik2();
                icerik2.Etiketler = model.Hastags;
                icerik2.HaberMetni = model.EssayContent;
                icerik2.HaberId = haberId;

                var yazar = context.tbl_PortalHaber_Yazar.FirstOrDefault(u => u.EPosta == model.UserEmail);
                icerik2.YazarId = yazar.YazarId;

                context.tbl_PortalHaber_Icerik2.Add(icerik2);
                context.SaveChanges();
                //context.SubmitChanges();
            }

            //context.Connection.Close();
            //context.Connection.Dispose();

            return true;
        }

        public List<SelectListItem> GetKategoriList()
        {
            var list = new List<SelectListItem>();
            //ManageDataContext context = new ManageDataContext();
            //context.Connection.Open();
            var tempList = context.tbl_PortalHaber_kategori.ToList();
            foreach (var item in tempList)
            {
                var one = new SelectListItem() { Text = item.KName, Value = item.Kid.ToString() };
                list.Add(one);


            }
            return list;
        }

        public List<SelectListItem> GetKaynakList()
        {
            var list = new List<SelectListItem>();
            //ManageDataContext context = new ManageDataContext();
            //context.Connection.Open();
            var tempList = context.tbl_PortalHaber_Kaynak.ToList();
            foreach (var item in tempList)
            {
                var one = new SelectListItem() { Text = item.Adi, Value = item.KaynakId.ToString() };
                list.Add(one);

            }
            return list;
        }

        public List<SelectListItem> GetSiteList()
        {
            var list = new List<SelectListItem>();
            //ManageDataContext context = new ManageDataContext();
            //context.Connection.Open();
            var tempList = context.tbl_PortalHaber_Site.ToList();
            foreach (var item in tempList)
            {
                var one = new SelectListItem() { Text = item.Site_Adi, Value = item.Site_Id.ToString() };
                list.Add(one);

            }
            return list;
        }

        public List<SelectListItem> GetBannerList()
        {
            var list = new List<SelectListItem>();
            //ManageDataContext context = new ManageDataContext();
            //context.Connection.Open();
            var tempList = context.VWBANNERs.ToList();
            foreach (var item in tempList)
            {
                var one = new SelectListItem() { Text = item.BannerAdi, Value = item.SbId.ToString() };
                list.Add(one);

            }
            return list;
        }

        //public Boolean InsertEssayContent(Edd model)
        //{
        //    var result = false;
        //    try
        //    {
        //        tbl_PortalHaber_Icerik2 icerik2 = new tbl_PortalHaber_Icerik2();
        //        icerik2.HaberMetni = model.EssayContent;
        //        ManageDataContext context = new ManageDataContext();
        //        context.Connection.Open();
        //        context.tbl_PortalHaber_Icerik2s.InsertOnSubmit(icerik2);
        //        context.SubmitChanges();
        //        context.Connection.Close();
        //        context.Dispose();
        //        result = true;

        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.ToString());
        //        result = false;
        //    }

        //    return result;
        //}
    }
}