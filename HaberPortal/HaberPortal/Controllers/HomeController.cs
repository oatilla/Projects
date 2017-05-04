using System;
using System.IO;
using System.Web.Mvc;
using HaberPortal.Models;

namespace HaberPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly DbOperation _dbOperation = new DbOperation();
        public EssayModel EssayModel = new EssayModel();


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View("Login");
        }

        public ActionResult LogOut()
        {

            Session["useremail"] = null;
            return View("Login");
        }

        public ActionResult EssayList()
        {
            var essayList = new EssayListModel();
            essayList.HaberModelList = _dbOperation.GetEssayList();
            return View("EssayList", essayList);
        }

        [HttpPost]
        public ActionResult DeleteNew(int id)
        {
            return View("EssayList");
        }

        [HttpPost]
        public ActionResult UpdateNew()
        {

            return View("UpdateEssay"); // Ajax ile post işleminde yemiyor
            return Json(Url.Action("UpdateEssay", "Home")); // redirect yapmak için  with ajax
        }

        public ActionResult UpdateEssay(int id)
        {

            var useremail = Session["useremail"];
            if (useremail != null)
            {

                // ComboBox'ların doldurulması sağlanır.
                EssayModel.ActionType = "update";
                EssayModel.UpdateId = id;

                EssayModel.DropDownListKategori = _dbOperation.GetKategoriList();
                EssayModel.DropDownListKaynak = _dbOperation.GetKaynakList();
                EssayModel.DropDownListSite = _dbOperation.GetSiteList();
                EssayModel.DropDownListBanner = _dbOperation.GetBannerList();
                EssayModel = _dbOperation.FillREssayModelFordUpdate(EssayModel, id);

                return View("UpdateEssay", EssayModel);
            }
            else
            {
                return View("Login");
            }

        }

        public ActionResult AddEssay()
        {

            var useremail = Session["useremail"];
            if (useremail != null)
            {
                EssayModel.ActionType = "create";
                EssayModel.DropDownListKategori = _dbOperation.GetKategoriList();
                EssayModel.DropDownListKaynak = _dbOperation.GetKaynakList();
                EssayModel.DropDownListSite = _dbOperation.GetSiteList();
                EssayModel.DropDownListBanner = _dbOperation.GetBannerList();

                return View("AddEssay", EssayModel);
            }
            else
            {
                return View("Login");
            }

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddEditorContent(EssayModel model)
        {
            var view = "";
            EssayModel.DropDownListKategori = _dbOperation.GetKategoriList();
            EssayModel.DropDownListKaynak = _dbOperation.GetKaynakList();
            EssayModel.DropDownListSite = _dbOperation.GetSiteList();
            EssayModel.DropDownListBanner = _dbOperation.GetBannerList();

            if (ModelState.IsValid)
            {


                model.UserEmail = Session["useremail"].ToString();
                // Insert Image to Img Folder
                var fileName = Path.GetFileName(model.HaberResimFile.FileName);
                var ext = Path.GetExtension(model.HaberResimFile.FileName);
                string name = Path.GetFileNameWithoutExtension(fileName);
                var guid = Guid.NewGuid().ToString();
                string newsfile = name + "_" + guid + ext;
                var path = Path.Combine(Server.MapPath("~/Img"), newsfile);
                model.HaberResimFile.SaveAs(path);
            }
            else
            {
                if (EssayModel.ActionType == "create")
                {
                    ViewBag.Error = "Resim Seçiniz";
                    view = "AddEssay";
                }
                else if (EssayModel.ActionType == "update")
                {
                    ViewBag.Error = "Resim Seçiniz";
                    view = "UpdateEssay";
                }
                return View(view, EssayModel);
            }

            var result = false;
            // Insert To Icerik ve Icerik2
            if (EssayModel.ActionType == "create")
            {
                result = _dbOperation.InsertEssayToDb(model);
            }
            else
            {
                var useremail = Session["useremail"].ToString();
                var id = EssayModel.UpdateId;
                result = _dbOperation.UpdateEssayToDb(model, id, useremail);

            }



            if (result && EssayModel.ActionType == "create")
            {
                view = "AddEssay";
            }
            else if (result && EssayModel.ActionType == "update")
            {
                view = "UpdateEssay";
            }
            return View(view, EssayModel);



        }

        [HttpPost]
        public ActionResult CheckUser(Login model)
        {
            if (ModelState.IsValid)
            {


                var email = model.Email;
                var password = model.Password;
                var result = _dbOperation.CheckUser(email, password);

                if (result)
                {
                    EssayModel.DropDownListKategori = _dbOperation.GetKategoriList();
                    EssayModel.DropDownListKaynak = _dbOperation.GetKaynakList();
                    EssayModel.DropDownListSite = _dbOperation.GetSiteList();
                    EssayModel.DropDownListBanner = _dbOperation.GetBannerList();


                    ViewBag.result = "success";
                    ViewBag.message = "Giriş Onaylandı";
                    ViewBag.Username = model.Email;
                    ViewBag.NowDate = DateTime.Now;
                    Session["useremail"] = email;
                    return View("Index", EssayModel);
                }
                else
                {
                    ViewBag.result = "failed";
                    ViewBag.message = "Email veya Şifre Yanlış Tekrar Deneyiniz !!!";
                    return View("Login");
                }

            }
            else
            {
                return View("Login");
            }

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public ActionResult ForgotPassword()
        {
            ViewBag.Message = "Forgot Password Page.";
            ViewBag.ForgotMessage = "Lütfen Yeni Şifre için E-mail Adresinizi Yazınız.";
            ViewBag.FoundDate = "2017-2018";
            return View();
        }

        [HttpPost]
        public ActionResult GetPassword(ForgotPassword model)
        {
            var email = model.Email;
            var result = _dbOperation.IsEmailInSystem(email);
            ViewBag.ForgotMessage = result ? "Talebiniz alınmıştır. En Kısa Sürede mailinize şifre gönderilecektir." : "Sistemde girdiğiniz mail kayıtlı değildir. !!!";
            return View("ForgotPassword");
        }
    }
}