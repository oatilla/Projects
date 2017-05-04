using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using AnnaAndSarahWeb.Models;
using System.IO;
using System.Collections.Generic;

namespace AnnaAndSarahWeb.Controllers
{
    //[Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        AnnaSarahEntities newEntity = new AnnaSarahEntities();

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        //
        // GET: /Manage/RemovePhoneNumber
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        #endregion


        public ActionResult AddProductDetail()
        {
            if (Session["User"] != null)
            {
                ViewBag.Logo = "/images/logo-1.png";
                ProductDetailPage getProductDetailpage = new ProductDetailPage();

                string sayfaId = RouteData.Values["id"].ToString();
                tblCategory getCategory = new tblCategory();
                List<tblProductDetail> getCategoryUpdList = new List<tblProductDetail>();
                tblProductDetail getProductDetail = new tblProductDetail();
                List<tblCategory> getCategoryt = (List<tblCategory>)(from c in newEntity.tblCategories where c.LinkUrl == sayfaId select c).ToList();
                if (getCategoryt.Count > 0)
                {
                    getCategory = getCategoryt.First();
                    ViewBag.Logo = "/images/Categories/" + getCategory.Image;
                    //ViewBag.Ad = getCategory.CatName;

                    ViewBag.Urun = getCategory.CatName;

                    getCategoryUpdList = (List<tblProductDetail>)(from c in newEntity.tblProductDetails where c.CatId == getCategory.Cid select c).ToList();
                }
                if (getCategoryUpdList.Count > 0)
                {
                    getProductDetail = getCategoryUpdList.First();
                    getProductDetailpage.t_productDetail = getProductDetail;
                    return View(getProductDetailpage);
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction("LoginAnnaSarah/2");
            }
        }

        [HttpPost]
        public ActionResult AddProductDetail(ProductDetailPage tpc)
        {

            string sayfaId = RouteData.Values["id"].ToString();
            tblProductDetail newProductDetail = new tblProductDetail();
            tblCategory getCategory = new tblCategory();
            List<tblCategory> getCategoryAllList = (List<tblCategory>)(from c in newEntity.tblCategories where c.LinkUrl == sayfaId select c).ToList();

            if (getCategoryAllList.Count > 0)
            {
                getCategory = getCategoryAllList.First();
            }

            List<tblProductDetail> getProductDetailList = (List<tblProductDetail>)(from c in newEntity.tblProductDetails where c.CatId == getCategory.Cid select c).ToList();
            if (getProductDetailList.Count() > 0)
            {

                tblProductDetail getProductDetailUpdate = (tblProductDetail)(from c in newEntity.tblProductDetails where c.CatId == getCategory.Cid select c).First();
                getProductDetailUpdate.Calcium_Yuzde = tpc.t_productDetail.Calcium_Yuzde;
                getProductDetailUpdate.Calories = tpc.t_productDetail.Calories;
                getProductDetailUpdate.Calories_from_fat = tpc.t_productDetail.Calories_from_fat;
                getProductDetailUpdate.Cholesterol = tpc.t_productDetail.Cholesterol;
                getProductDetailUpdate.Cholesterol_Yuzde = tpc.t_productDetail.Cholesterol_Yuzde;
                getProductDetailUpdate.Company = tpc.t_productDetail.Company;
                getProductDetailUpdate.Country = tpc.t_productDetail.Country;
                getProductDetailUpdate.Dietary_Fiber = tpc.t_productDetail.Dietary_Fiber;
                getProductDetailUpdate.Dietary_Fiber_Yuzde = tpc.t_productDetail.Dietary_Fiber_Yuzde;
                getProductDetailUpdate.Ingredients = tpc.t_productDetail.Ingredients;
                getProductDetailUpdate.Iron_Yuzde = tpc.t_productDetail.Iron_Yuzde;
                getProductDetailUpdate.Link = tpc.t_productDetail.Link;
                getProductDetailUpdate.Protein = tpc.t_productDetail.Protein;
                getProductDetailUpdate.Protein_Yuzde = tpc.t_productDetail.Protein_Yuzde;
                getProductDetailUpdate.Saturated_Fat = tpc.t_productDetail.Saturated_Fat;
                getProductDetailUpdate.Saturated_Fat_Yuzde = tpc.t_productDetail.Saturated_Fat_Yuzde;
                getProductDetailUpdate.ServingSize = tpc.t_productDetail.ServingSize;
                getProductDetailUpdate.SiraNo = tpc.t_productDetail.SiraNo;
                getProductDetailUpdate.Sodium = tpc.t_productDetail.Sodium;
                getProductDetailUpdate.Sodium_Yuzde = tpc.t_productDetail.Sodium_Yuzde;
                getProductDetailUpdate.Stock = tpc.t_productDetail.Stock;
                getProductDetailUpdate.Sugar = tpc.t_productDetail.Sugar;

                getProductDetailUpdate.Sugar_Yuzde = tpc.t_productDetail.Sugar_Yuzde;
                getProductDetailUpdate.Total_Carbonhydrate = tpc.t_productDetail.Total_Carbonhydrate;
                getProductDetailUpdate.Total_Carbonhydrate_Yuzde = tpc.t_productDetail.Total_Carbonhydrate_Yuzde;
                getProductDetailUpdate.Total_Fat = tpc.t_productDetail.Total_Fat;
                getProductDetailUpdate.Total_Fat_Yuzde = tpc.t_productDetail.Total_Fat_Yuzde;
                getProductDetailUpdate.Trans_Fat = tpc.t_productDetail.Trans_Fat;
                getProductDetailUpdate.Trans_Fat_Yuzde = tpc.t_productDetail.Trans_Fat_Yuzde;
                getProductDetailUpdate.VitaminA_Yuzde = tpc.t_productDetail.VitaminA_Yuzde;
                getProductDetailUpdate.VitaminC_Yuzde = tpc.t_productDetail.VitaminC_Yuzde;
                getProductDetailUpdate.Weight = tpc.t_productDetail.Weight;

                getProductDetailUpdate.Column1_Header = tpc.t_productDetail.Column1_Header;
                getProductDetailUpdate.Column2_Header = tpc.t_productDetail.Column2_Header;
                getProductDetailUpdate.Column3_Header = tpc.t_productDetail.Column3_Header;
                getProductDetailUpdate.Column4_Header = tpc.t_productDetail.Column4_Header;

                getProductDetailUpdate.Column1_Value = tpc.t_productDetail.Column1_Value;
                getProductDetailUpdate.Column2_Value = tpc.t_productDetail.Column2_Value;
                getProductDetailUpdate.Column3_Value = tpc.t_productDetail.Column3_Value;
                getProductDetailUpdate.Column4_Value = tpc.t_productDetail.Column4_Value;

                getProductDetailUpdate.CatId = getCategory.Cid;

                //List<vw_kat_detail> vw_category_product_list = (List<vw_kat_detail>)(from c in newEntity.vw_kat_detail where c.CMainId == getCategory.CMainId && c.SiraNo == tpc.t_productDetail.SiraNo select c).ToList();
                //if (vw_category_product_list.Count > 0)
                //{
                //    vw_kat_detail get_vw_category_product = vw_category_product_list.First();
                //    List<tblProductDetail> tblProductDetailList = (List<tblProductDetail>)(from c in newEntity.tblProductDetails where c.Cid == get_vw_category_product.Cid select c).ToList();
                //    if (tblProductDetailList.Count > 0)
                //    {
                //        tblProductDetail getPrDetail = tblProductDetailList.First();
                //        getPrDetail.SiraNo = 0;
                //    }
                //}

                newEntity.SaveChanges();

            }
            else
            {
                newProductDetail = tpc.t_productDetail;
                newProductDetail.CatId = getCategory.Cid;

                //List<vw_kat_detail> vw_category_product_list = (List<vw_kat_detail>)(from c in newEntity.vw_kat_detail where c.CMainId == getCategory.CMainId && c.SiraNo == tpc.t_productDetail.SiraNo select c).ToList();
                //if (vw_category_product_list.Count > 0)
                //{
                //    vw_kat_detail get_vw_category_product = vw_category_product_list.First();
                //    List<tblProductDetail> tblProductDetailList = (List<tblProductDetail>)(from c in newEntity.tblProductDetails where c.Cid == get_vw_category_product.Cid select c).ToList();
                //    if (tblProductDetailList.Count > 0)
                //    {
                //        tblProductDetail getPrDetail = tblProductDetailList.First();
                //        getPrDetail.SiraNo = 0;
                //    }
                //}

                newEntity.tblProductDetails.Add(newProductDetail);
                newEntity.SaveChanges();
            }





            return RedirectToAction("GetMainProductList");
        }


        public ActionResult SubscribeList()
        {
            if (Session["User"] != null)
            {
                ViewBag.Logo = "/images/logo-1.png";
                List<tbl_SubscribeList> getCategoryAllList = (List<tbl_SubscribeList>)(from c in newEntity.tbl_SubscribeList select c).ToList();
                return View(getCategoryAllList);
            }
            else
            {
                return RedirectToAction("LoginAnnaSarah/2");
            }
        }



        public ActionResult ProductList()
        {
            if (Session["User"] != null)
            {
                ViewBag.Logo = "/images/logo-1.png";

                List<tblCategory> getCategoryList = new List<tblCategory>();
                List<tblCategory> getCategoryNoChildList = new List<tblCategory>();
                tblCategory getCategory = new tblCategory();
                List<tblCategory> getCategoryAllList = (List<tblCategory>)(from c in newEntity.tblCategories select c).ToList();
                foreach (var item in getCategoryAllList)
                {
                    getCategoryList = (List<tblCategory>)(from c in getCategoryAllList where c.CMainId == item.Cid select c).ToList();
                    if (getCategoryList.Count == 0)
                    {
                        getCategoryNoChildList.Add(item);
                    }
                }

                return View(getCategoryNoChildList);
            }
            else
            {
                return RedirectToAction("LoginAnnaSarah/2");
            }
        }


        public ActionResult GetMainProductList()
        {
            if (Session["User"] != null)
            {
                ViewBag.Logo = "/images/logo-1.png";
                List<tblCategory> getCategoryAllList = (List<tblCategory>)(from c in newEntity.tblCategories where c.CMainId == 0 select c).ToList();
                return View(getCategoryAllList);
            }
            else
            {
                return RedirectToAction("LoginAnnaSarah/2");
            }
        }

        public ActionResult MainProductDetail()
        {
            if (Session["User"] != null)
            {
                ViewBag.Logo = "/images/logo-1.png";
                int sayfaId = Convert.ToInt32(RouteData.Values["id"].ToString());
                ViewBag.CatId = sayfaId;
                List<tblCategory> getCategoryMiddleList = (List<tblCategory>)(from c in newEntity.tblCategories where c.CMainId == sayfaId select c).ToList();
                return View(getCategoryMiddleList);
            }
            else
            {
                return RedirectToAction("LoginAnnaSarah/2");
            }
        }

        public ActionResult MiddleProductDetail()
        {
            if (Session["User"] != null)
            {
                ViewBag.Logo = "/images/logo-1.png";
                int sayfaId = Convert.ToInt32(RouteData.Values["id"].ToString());
                ViewBag.CatId = sayfaId;
                List<vw_category_product> getCategoryMiddleList = (List<vw_category_product>)(from c in newEntity.vw_category_product where c.CMainId == sayfaId select c).ToList();
                return View(getCategoryMiddleList);
            }
            else
            {
                return RedirectToAction("LoginAnnaSarah/2");
            }
        }


        public ActionResult AddTags()
        {
            if (Session["User"] != null)
            {
                ViewBag.Logo = "/images/logo-1.png";
                //string tagId = RouteData.Values["id"].ToString();
                var tagId = RouteData.Values.ContainsKey("id") ? Convert.ToInt32(RouteData.Values["id"]) : 0;
                if (tagId > 0)
                {
                    int tgID = Convert.ToInt32(tagId.ToString());
                    List<tblTag> getTagList = (List<tblTag>)(from c in newEntity.tblTags where c.TagId == tgID select c).ToList();
                    if (getTagList.Count() > 0)
                    {
                        tblTag getTag = getTagList.First();
                        return View(getTag);
                    }
                    else
                    {
                        return View();
                    }
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction("LoginAnnaSarah/2");
            }
        }


        [HttpPost]
        public ActionResult AddTags(tblTag getTag)
        {
            var tagId = RouteData.Values.ContainsKey("id") ? Convert.ToInt32(RouteData.Values["id"]) : 0;

            string renk = ViewBag.Renk;

            if (tagId > 0)
            {
                int tgID = Convert.ToInt32(tagId.ToString());
                List<tblTag> getTagList = (List<tblTag>)(from c in newEntity.tblTags where c.TagId == tgID select c).ToList();
                if (getTagList.Count() > 0)
                {
                    tblTag getUpdateTag = getTagList.First();
                    getUpdateTag.TagName = getTag.TagName;
                    getUpdateTag.TagColor = getTag.TagColor;
                    newEntity.SaveChanges();
                }
            }
            else
            {
                newEntity.tblTags.Add(getTag);
                newEntity.SaveChanges();
            }
            return RedirectToAction("TagList");
        }

        public ActionResult TagList()
        {
            if (Session["User"] != null)
            {
                ViewBag.Logo = "/images/logo-1.png";
                List<tblTag> getTagler = (List<tblTag>)(from c in newEntity.tblTags select c).ToList();
                return View(getTagler);
            }
            else
            {
                return RedirectToAction("LoginAnnaSarah/2");
            }
        }


        public ActionResult AddCategoryTags()
        {
            if (Session["User"] != null)
            {
                ViewBag.Logo = "/images/logo-1.png";
                List<tblTag> getTagList = (List<tblTag>)(from c in newEntity.tblTags select c).ToList();
                TagPage newTagpage = new TagPage();
                List<TagPage> newTagpageList = new List<TagPage>();
                foreach (var item in getTagList)
                {
                    newTagpage = new TagPage();
                    newTagpage.Tag = item;
                    newTagpage.Onay = true;
                    newTagpageList.Add(newTagpage);
                }

                return View(newTagpageList);
            }
            else
            {
                return RedirectToAction("LoginAnnaSarah/2");
            }
        }

        [HttpPost]
        public ActionResult AddCategoryTags(IEnumerable<TagPage> getTagPageList)
        {
            foreach (var item in getTagPageList)
            {
                if (item.Onay == true)
                {

                }



            }
            return RedirectToAction("AddCategoryTags");
        }

        [HttpGet]
        public ActionResult Index2()
        {
            if (Session["User"] != null)
            {
                var catId = RouteData.Values.ContainsKey("id") ? Convert.ToInt32(RouteData.Values["id"]) : 0;
                ViewBag.Logo = "/images/logo-1.png";
                List<tblTag> getTagList = (List<tblTag>)(from c in newEntity.tblTags select c).ToList();
                TagPage newTagpage = new TagPage();
                List<TagPage> newTagpageList = new List<TagPage>();
                foreach (var item in getTagList)
                {
                    List<tblTagCategory> getTagCatList = (List<tblTagCategory>)(from c in newEntity.tblTagCategories where c.CatId == catId select c).ToList();
                    if (getTagCatList.Count > 0)
                    {
                        newTagpage = new TagPage();
                        newTagpage.Onay = false;
                        foreach (var item2 in getTagCatList)
                        {
                            if (item2.TagId == item.TagId)
                            {
                                newTagpage.Onay = true;
                                break;
                            }
                        }
                        newTagpage.Tag = item;
                        newTagpageList.Add(newTagpage);
                    }
                    else
                    {
                        newTagpage = new TagPage();
                        newTagpage.Onay = false;
                        newTagpage.Tag = item;
                        newTagpageList.Add(newTagpage);
                    }
                }
                return View(newTagpageList);
            }
            else
            {
                return RedirectToAction("LoginAnnaSarah/2");
            }
        }

        [HttpPost]
        public ActionResult Index2(IEnumerable<TagPage> taglar)
        {
            var catId = RouteData.Values.ContainsKey("id") ? Convert.ToInt32(RouteData.Values["id"]) : 0;
            List<tblTagCategory> getTagCatList = (List<tblTagCategory>)(from c in newEntity.tblTagCategories where c.CatId == catId select c).ToList();
            tblTagCategory newTagCategory = new tblTagCategory();
            int sonuc = 0;
            if (catId > 0)
            {
                int ctID = Convert.ToInt32(catId.ToString());

                if (getTagCatList.Count > 0)
                {
                    foreach (var item in getTagCatList)
                    {
                        newEntity.tblTagCategories.Remove(item);
                        newEntity.SaveChanges();
                    }
                }

                foreach (var tag in taglar)
                {
                    if (tag.Onay)
                    {
                        newTagCategory.CatId = ctID;
                        newTagCategory.TagId = tag.Tag.TagId;
                        newEntity.tblTagCategories.Add(newTagCategory);
                        sonuc = newEntity.SaveChanges();
                    }
                }
            }


            if (sonuc > 0)
            {
                return RedirectToAction("Onay/1");
            }
            else
            {
                return RedirectToAction("Onay/2");
            }
        }


        [HttpGet]
        public ActionResult Onay()
        {
            if (Session["User"] != null)
            {
                var onayId = RouteData.Values.ContainsKey("id") ? Convert.ToInt32(RouteData.Values["id"]) : 0;
                ViewBag.Logo = "/images/logo-1.png";
                if (onayId == 1)
                {
                    ViewBag.Class = "callout-info";
                    ViewBag.Sonuc = "Başarılı kayıt oluşturdunuz";
                    ViewBag.Metin = "Ürün";
                    ViewBag.LinkOnay = "GetMainProductList";
                }
                else if (onayId == 2)
                {
                    ViewBag.Class = "callout-danger";
                    ViewBag.Sonuc = "Başarısız";
                    ViewBag.Metin = "Ürün";
                    ViewBag.LinkOnay = "GetMainProductList";
                }
                else if (onayId == 3)
                {
                    ViewBag.Class = "callout-info";
                    ViewBag.Sonuc = "Başarılı kayıt oluşturdunuz";
                    ViewBag.Metin = "Banner";
                    ViewBag.LinkOnay = "BannerList";
                }
                else if (onayId == 4)
                {
                    ViewBag.Class = "callout-danger";
                    ViewBag.Sonuc = "Başarısız";
                    ViewBag.Metin = "Banner";
                    ViewBag.LinkOnay = "BannerList";
                }
                else if (onayId == 5)
                {
                    ViewBag.Class = "callout-info";
                    ViewBag.Sonuc = "Başarılı kayıt oluşturdunuz";
                    ViewBag.Metin = "Blog";
                    ViewBag.LinkOnay = "BlogList";
                }
                else if (onayId == 6)
                {
                    ViewBag.Class = "callout-danger";
                    ViewBag.Sonuc = "Başarısız";
                    ViewBag.Metin = "Blog";
                    ViewBag.LinkOnay = "BlogList";
                }
                else if (onayId == 7)
                {
                    ViewBag.Class = "callout-info";
                    ViewBag.Sonuc = "Başarılı kayıt oluşturdunuz";
                    ViewBag.Metin = "Blog Category";
                    ViewBag.LinkOnay = "BlogCategoryList";
                }
                else if (onayId == 8)
                {
                    ViewBag.Class = "callout-danger";
                    ViewBag.Sonuc = "Başarısız";
                    ViewBag.Metin = "Blog Category";
                    ViewBag.LinkOnay = "BlogCategoryList";
                }
                else if (onayId == 9)
                {
                    ViewBag.Class = "callout-info";
                    ViewBag.Sonuc = "Başarılı kayıt oluşturdunuz";
                    ViewBag.Metin = "Add User";
                    ViewBag.LinkOnay = "UserList";
                }
                else if (onayId == 10)
                {
                    ViewBag.Class = "callout-danger";
                    ViewBag.Sonuc = "Başarısız";
                    ViewBag.Metin = "Add User";
                    ViewBag.LinkOnay = "UserList";
                }

                return View();
            }
            else
            {
                return RedirectToAction("LoginAnnaSarah/2");
            }
        }

        [HttpGet]
        public ActionResult UpdateCategory()
        {
            if (Session["User"] != null)
            {
                ViewBag.Logo = "/images/logo-1.png";
                Picture pc = new Picture();
                int sayfaId = Convert.ToInt32(RouteData.Values["id"].ToString());
                List<tblCategory> getCategoryMiddleList = (List<tblCategory>)(from c in newEntity.tblCategories where c.Cid == sayfaId select c).ToList();
                //MainCategory getMainCategory = new MainCategory();
                if (getCategoryMiddleList.Count > 0)
                {
                    tblCategory getCategory = getCategoryMiddleList.First();
                    ViewBag.Resim = getCategory.Image;
                    //getMainCategory.CategoryName = getCategory.CatName;
                    //getMainCategory.IsEnabled = Convert.ToBoolean(getCategory.IsEnabled.ToString());
                    //getMainCategory.CatId = getCategory.Cid;
                    pc.getCategory = getCategory;
                    return View(pc);
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction("LoginAnnaSarah/2");
            }
        }

        [HttpPost]
        public ActionResult UpdateCategory(Picture mc)
        {
            tblCategory getCategory = new tblCategory();
            int sayfaId = Convert.ToInt32(RouteData.Values["id"].ToString());
            List<tblCategory> getCategoryMiddleList = (List<tblCategory>)(from c in newEntity.tblCategories where c.Cid == sayfaId select c).ToList();
            if (getCategoryMiddleList.Count > 0)
            {
                getCategory = getCategoryMiddleList.First();
                if (mc.file.FileName != null)
                {
                    Random rnd = new Random();
                    string rasgele = rnd.Next(1, 10000).ToString();
                    string dosyaYolu = rasgele + Path.GetFileName(mc.file.FileName);
                    var yuklemeYeri = Path.Combine(Server.MapPath("~/images/Categories"), dosyaYolu);
                    mc.file.SaveAs(yuklemeYeri);
                    getCategory.Image = dosyaYolu;
                }
                getCategory.CatName = mc.getCategory.CatName;
                getCategory.IsEnabled = mc.getCategory.IsEnabled;
                newEntity.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                getCategory = new tblCategory();
                if (mc.file.FileName != null)
                {
                    Random rnd = new Random();
                    string rasgele = rnd.Next(1, 10000).ToString();
                    string dosyaYolu = rasgele + Path.GetFileName(mc.file.FileName);
                    var yuklemeYeri = Path.Combine(Server.MapPath("~/images/Categories"), dosyaYolu);
                    mc.file.SaveAs(yuklemeYeri);
                    getCategory.Image = dosyaYolu;
                }
                getCategory.CatName = mc.getCategory.CatName;
                getCategory.IsEnabled = mc.getCategory.IsEnabled;
                newEntity.tblCategories.Add(getCategory);
                return RedirectToAction("Index");
                //}
            }
        }

        public ActionResult Deneme()
        {
            if (Session["User"] != null)
            {
                ViewBag.Logo = "/images/logo-1.png";
                Picture pc = new Picture();
                int sayfaId = Convert.ToInt32(RouteData.Values["id"].ToString());
                List<tblCategory> getCategoryMiddleList = (List<tblCategory>)(from c in newEntity.tblCategories where c.Cid == sayfaId select c).ToList();
                //MainCategory getMainCategory = new MainCategory();
                if (getCategoryMiddleList.Count > 0)
                {
                    tblCategory getCategory = getCategoryMiddleList.First();
                    ViewBag.Logo = "/images/Categories/" + getCategory.Image;
                    //getMainCategory.CategoryName = getCategory.CatName;
                    //getMainCategory.IsEnabled = Convert.ToBoolean(getCategory.IsEnabled.ToString());
                    //getMainCategory.CatId = getCategory.Cid;
                    pc.getCategory = getCategory;
                    return View(pc);
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction("LoginAnnaSarah/2");
            }
        }

        [HttpPost]
        public ActionResult Deneme(Picture mc)
        {
            tblCategory getCategory = new tblCategory();
            int sayfaId = Convert.ToInt32(RouteData.Values["id"].ToString());
            List<tblCategory> getCategoryMiddleList = (List<tblCategory>)(from c in newEntity.tblCategories where c.Cid == sayfaId select c).ToList();
            tblCategory getCategoryUpdateSiraNo = new tblCategory();
            if (getCategoryMiddleList.Count > 0)
            {
                getCategory = getCategoryMiddleList.First();
                if (mc.file != null)
                {
                    Random rnd = new Random();
                    string rasgele = rnd.Next(1, 10000).ToString();
                    string dosyaYolu = rasgele + Path.GetFileName(mc.file.FileName);
                    var yuklemeYeri = Path.Combine(Server.MapPath("~/images/Categories"), dosyaYolu);
                    mc.file.SaveAs(yuklemeYeri);
                    getCategory.Image = dosyaYolu;
                }
                getCategory.CatName = mc.getCategory.CatName;
                getCategory.IsEnabled = mc.getCategory.IsEnabled;
                getCategory.SiraNo = mc.getCategory.SiraNo;


                List<tblCategory> getCategoryListUpdateSiraNo = (List<tblCategory>)(from c in newEntity.tblCategories where c.CMainId == getCategory.CMainId && c.SiraNo == mc.getCategory.SiraNo select c).ToList();
                if (getCategoryListUpdateSiraNo.Count > 0)
                {
                    getCategoryUpdateSiraNo = getCategoryListUpdateSiraNo.First();
                    getCategoryUpdateSiraNo.SiraNo = 999;
                }



                newEntity.SaveChanges();
                return RedirectToAction("GetMainProductList");
            }
            else
            {
                return RedirectToAction("GetMainProductList");
            }
        }

        public ActionResult NewCategory()
        {
            if (Session["User"] != null)
            {
                ViewBag.Logo = "/images/logo-1.png";
                return View();
            }
            else
            {
                return RedirectToAction("LoginAnnaSarah/2");
            }
        }

        [HttpPost]
        public ActionResult NewCategory(Picture mc)
        {
            int sayfaId = Convert.ToInt32(RouteData.Values["id"].ToString());
            List<tblCategory> gCategoryList = (List<tblCategory>)(from c in newEntity.tblCategories where c.Cid == sayfaId select c).ToList();
            if (gCategoryList.Count > 0 || sayfaId == 0)
            {
                tblCategory getCategory = new tblCategory();
                if (mc.file != null)
                {
                    Random rnd = new Random();
                    string rasgele = rnd.Next(1, 10000).ToString();
                    string dosyaYolu = rasgele + Path.GetFileName(mc.file.FileName);
                    var yuklemeYeri = Path.Combine(Server.MapPath("~/images/Categories"), dosyaYolu);
                    mc.file.SaveAs(yuklemeYeri);
                    getCategory.Image = dosyaYolu;
                }
                if (sayfaId == 0)
                {
                    getCategory.CMainId = 0;
                }
                else
                {
                    tblCategory gCat = gCategoryList.First();
                    getCategory.CMainId = gCat.Cid;
                }

                getCategory.CatName = mc.getCategory.CatName;
                getCategory.IsEnabled = mc.getCategory.IsEnabled;
                if (mc.getCategory.SiraNo != null)
                {
                    getCategory.SiraNo = mc.getCategory.SiraNo;
                }
                else
                {
                    getCategory.SiraNo = 999;
                }

                getCategory.LinkUrl = mc.getCategory.CatName.Replace(" ", "_").Replace("&", "_").Replace("/", "_");

                tblCategory getCategoryUpdateSiraNo = new tblCategory();
                List<tblCategory> getCategoryListUpdateSiraNo = (List<tblCategory>)(from c in newEntity.tblCategories where c.CMainId == getCategory.CMainId && c.SiraNo == mc.getCategory.SiraNo select c).ToList();
                if (getCategoryListUpdateSiraNo.Count > 0)
                {
                    getCategoryUpdateSiraNo = getCategoryListUpdateSiraNo.First();
                    getCategoryUpdateSiraNo.SiraNo = 999;
                }


                newEntity.tblCategories.Add(getCategory);
                newEntity.SaveChanges();
            }
            return RedirectToAction("GetMainProductList");
        }

        public ActionResult DeleteCategory()
        {
            if (Session["User"] != null)
            {
                ViewBag.Logo = "/images/logo-1.png";
                return View();
            }
            else
            {
                return RedirectToAction("LoginAnnaSarah/2");
            }
        }

        [HttpPost]
        public ActionResult DeleteCategory(tblCategory tc)
        {
            int sayfaId = Convert.ToInt32(RouteData.Values["id"].ToString());
            List<tblCategory> gCategoryList = (List<tblCategory>)(from c in newEntity.tblCategories where c.Cid == sayfaId select c).ToList();
            if (gCategoryList.Count > 0)
            {
                tblCategory getCategory = gCategoryList.First();
                newEntity.tblCategories.Remove(getCategory);
                newEntity.SaveChanges();
            }

            return RedirectToAction("GetMainProductList");
        }

        public ActionResult MessageList()
        {
            if (Session["User"] != null)
            {
                ViewBag.Logo = "/images/logo-1.png";
                List<tblContact> gMessageList = (List<tblContact>)(from c in newEntity.tblContacts select c).ToList();
                return View(gMessageList);
            }
            else
            {
                return RedirectToAction("LoginAnnaSarah/2");
            }
        }

        public ActionResult AddBanner()
        {
            if (Session["User"] != null)
            {
                ViewBag.Logo = "/images/logo-1.png";
                return View();
            }
            else
            {
                return RedirectToAction("LoginAnnaSarah/2");
            }
        }

        [HttpPost]
        public ActionResult AddBanner(BannerPage getBanner)
        {
            tblBanner newBanner = new tblBanner();
            newBanner = getBanner.banner;
            List<tblBanner> gBannerList = (List<tblBanner>)(from c in newEntity.tblBanners where c.SiraNo == getBanner.banner.SiraNo select c).ToList();
            if (gBannerList.Count > 0)
            {
                tblBanner getGuncelBanner = gBannerList.First();
                getGuncelBanner.SiraNo = 999;

            }

            if (getBanner.file != null)
            {
                Random rnd = new Random();
                string rasgele = rnd.Next(1, 10000).ToString();
                string dosyaYolu = rasgele + Path.GetFileName(getBanner.file.FileName);
                var yuklemeYeri = Path.Combine(Server.MapPath("~/images/Banner"), dosyaYolu);
                getBanner.file.SaveAs(yuklemeYeri);
                newBanner.BannerImage = dosyaYolu;
            }

            if (getBanner.banner.SiraNo != null)
            {
                newBanner.SiraNo = getBanner.banner.SiraNo;
            }
            else
            {
                newBanner.SiraNo = 999;
            }

            newEntity.tblBanners.Add(newBanner);

            if (newEntity.SaveChanges() > 0)
            {
                return RedirectToAction("Onay/3");
            }
            else
            {
                return RedirectToAction("Onay/4");
            }
        }

        public ActionResult BannerList()
        {
            if (Session["User"] != null)
            {
                ViewBag.Logo = "/images/logo-1.png";
                List<tblBanner> gBannerList = (List<tblBanner>)(from c in newEntity.tblBanners select c).ToList();
                return View(gBannerList);
            }
            else
            {
                return RedirectToAction("LoginAnnaSarah/2");
            }
        }

        [HttpGet]
        public ActionResult UpdateBanner()
        {
            if (Session["User"] != null)
            {
                BannerPage updBanner = new BannerPage();
                ViewBag.Logo = "/images/logo-1.png";
                int sayfaId = Convert.ToInt32(RouteData.Values["id"].ToString());
                List<tblBanner> getBannerList = (List<tblBanner>)(from c in newEntity.tblBanners where c.Bid == sayfaId select c).ToList();
                if (getBannerList.Count > 0)
                {
                    tblBanner getBanner = getBannerList.First();
                    ViewBag.Resim = getBanner.BannerImage;
                    updBanner.banner = getBanner;
                    return View(updBanner);
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction("LoginAnnaSarah/2");
            }
        }

        [HttpPost]
        public ActionResult UpdateBanner(BannerPage updBanner)
        {
            tblBanner getBanner = new tblBanner();
            int sayfaId = Convert.ToInt32(RouteData.Values["id"].ToString());
            List<tblBanner> getBannerList = (List<tblBanner>)(from c in newEntity.tblBanners where c.Bid == sayfaId select c).ToList();
            if (getBannerList.Count > 0)
            {

                List<tblBanner> gBannerList = (List<tblBanner>)(from c in newEntity.tblBanners where c.SiraNo == updBanner.banner.SiraNo select c).ToList();
                if (gBannerList.Count > 0)
                {
                    tblBanner getGuncelBanner = gBannerList.First();
                    getGuncelBanner.SiraNo = 999;
                }

                getBanner = getBannerList.First();
                getBanner.BannerHeader = updBanner.banner.BannerHeader;
                getBanner.BannerText = updBanner.banner.BannerText;
                getBanner.Link = updBanner.banner.Link;

                if (updBanner.banner.SiraNo != null)
                {
                    getBanner.SiraNo = updBanner.banner.SiraNo;
                }
                else
                {
                    getBanner.SiraNo = 999;
                }

                if (updBanner.file != null)
                {
                    Random rnd = new Random();
                    string rasgele = rnd.Next(1, 10000).ToString();
                    string dosyaYolu = rasgele + Path.GetFileName(updBanner.file.FileName);
                    var yuklemeYeri = Path.Combine(Server.MapPath("~/images/Banner"), dosyaYolu);
                    updBanner.file.SaveAs(yuklemeYeri);
                    getBanner.BannerImage = dosyaYolu;

                }

                if (newEntity.SaveChanges() > 0)
                {
                    return RedirectToAction("Onay/3");
                }
                else
                {
                    return RedirectToAction("Onay/4");
                }
            }
            else
            {
                return RedirectToAction("BannerList");
            }
        }


        public ActionResult DeleteBanner()
        {
            if (Session["User"] != null)
            {
                ViewBag.Logo = "/images/logo-1.png";
                return View();
            }
            else
            {
                return RedirectToAction("LoginAnnaSarah/2");
            }
        }

        [HttpPost]
        public ActionResult DeleteBanner(tblBanner tc)
        {
            int sayfaId = Convert.ToInt32(RouteData.Values["id"].ToString());
            List<tblBanner> gBannerList = (List<tblBanner>)(from c in newEntity.tblBanners where c.Bid == sayfaId select c).ToList();
            if (gBannerList.Count > 0)
            {
                tblBanner getBanner = gBannerList.First();
                newEntity.tblBanners.Remove(getBanner);
                newEntity.SaveChanges();
            }

            return RedirectToAction("BannerList");
        }

        public ActionResult DeleteBlog()
        {
            if (Session["User"] != null)
            {
                ViewBag.Logo = "/images/logo-1.png";
                int sayfaId = Convert.ToInt32(RouteData.Values["id"].ToString());
                List<tblBlog> gBlogList = (List<tblBlog>)(from c in newEntity.tblBlogs where c.Bid == sayfaId select c).ToList();
                if (gBlogList.Count > 0)
                {
                    tblBlog getBlog = gBlogList.First();
                    ViewBag.Logo = "/images/Blog/" + getBlog.BResim;
                    ViewBag.Header = getBlog.BAd;
                }
                return View();
            }
            else
            {
                return RedirectToAction("LoginAnnaSarah/2");
            }
        }


        [HttpPost]
        public ActionResult DeleteBlog(tblBlog tb)
        {
            int sayfaId = Convert.ToInt32(RouteData.Values["id"].ToString());
            List<tblBlog> gBlogList = (List<tblBlog>)(from c in newEntity.tblBlogs where c.Bid == sayfaId select c).ToList();
            if (gBlogList.Count > 0)
            {
                tblBlog getBlog = gBlogList.First();
                newEntity.tblBlogs.Remove(getBlog);
                newEntity.SaveChanges();
            }

            return RedirectToAction("BlogList");
        }


        public ActionResult AddBlog()
        {
            if (Session["User"] != null)
            {
                ViewBag.Logo = "/images/logo-1.png";
                List<tblBlogCategory> gBlogCategoryList = (List<tblBlogCategory>)(from c in newEntity.tblBlogCategories select c).ToList();
                List<tblBlogCategory> gBlogCategoryLast = new List<tblBlogCategory>();
                foreach (var item in gBlogCategoryList)
                {
                    tblBlogCategory cc = new tblBlogCategory();
                    cc.CName = item.CName;
                    cc.Cid = item.Cid;
                    gBlogCategoryLast.Add(cc);
                }

                ViewBag.CatList = new SelectList(gBlogCategoryLast, "Cid", "CName");


                var onayId = RouteData.Values.ContainsKey("id") ? Convert.ToInt32(RouteData.Values["id"]) : 0;
                PBlog blogPage = new PBlog();
                if (onayId > 0)
                {
                    List<tblBlog> getBlogList = (List<tblBlog>)(from c in newEntity.tblBlogs where c.Bid == onayId select c).ToList();
                    if (getBlogList.Count > 0)
                    {
                        tblBlog gBlogf = getBlogList.First();
                        blogPage.Blog = gBlogf;
                        blogPage.Cid = Convert.ToInt32(gBlogf.Cid.ToString());
                        ViewBag.Logo = "/images/Blog/" + blogPage.Blog.BResim;
                    }
                }
                else
                {
                    blogPage = new PBlog();
                    blogPage.Cid = 1;
                }


                return View(blogPage);
            }
            else
            {
                return RedirectToAction("LoginAnnaSarah/2");
            }
        }

        [HttpPost]
        public ActionResult AddBlog(PBlog getBlog)
        {
            //Yazar da eklenecek...
            var onayId = RouteData.Values.ContainsKey("id") ? Convert.ToInt32(RouteData.Values["id"]) : 0;
            tblBlog neworgetBlog = new tblBlog();
            if (onayId > 0)
            {
                List<tblBlog> getBlogList = (List<tblBlog>)(from c in newEntity.tblBlogs where c.Bid == onayId select c).ToList();
                if (getBlogList.Count > 0)
                {
                    neworgetBlog = getBlogList.First();
                    neworgetBlog.BAciklama = getBlog.Blog.BAciklama;
                    neworgetBlog.BAd = getBlog.Blog.BAd;
                    neworgetBlog.BText = getBlog.Blog.BText;
                    neworgetBlog.Yazar = getBlog.Blog.Yazar;
                    neworgetBlog.Cid = getBlog.Cid;
                }
            }
            else
            {
                neworgetBlog = getBlog.Blog;
                neworgetBlog.UrlRewrite = getBlog.Blog.BAd.Replace(" ", "_").Replace("&", "_").Replace("/", "_");
                neworgetBlog.Cid = getBlog.Cid;
                neworgetBlog.Tarih = DateTime.Now;
            }


            if (getBlog.file != null)
            {
                Random rnd = new Random();
                string rasgele = rnd.Next(1, 10000).ToString();
                string dosyaYolu = rasgele + Path.GetFileName(getBlog.file.FileName);
                var yuklemeYeri = Path.Combine(Server.MapPath("~/images/Blog"), dosyaYolu);
                getBlog.file.SaveAs(yuklemeYeri);
                neworgetBlog.BResim = dosyaYolu;
            }

            if (onayId == 0)
            {
                newEntity.tblBlogs.Add(neworgetBlog);
            }

            if (newEntity.SaveChanges() > 0)
            {
                return RedirectToAction("Onay/5");
            }
            else
            {
                return RedirectToAction("Onay/6");
            }
        }

        public ActionResult BlogList()
        {
            if (Session["User"] != null)
            {
                ViewBag.Logo = "/images/logo-1.png";
                List<tblBlog> getBlogList = (List<tblBlog>)(from c in newEntity.tblBlogs select c).ToList();
                return View(getBlogList);
            }
            else
            {
                return RedirectToAction("LoginAnnaSarah/2");
            }
        }

        public ActionResult AddBlogCategory()
        {
            if (Session["User"] != null)
            {
                ViewBag.Logo = "/images/logo-1.png";
                var onayId = RouteData.Values.ContainsKey("id") ? Convert.ToInt32(RouteData.Values["id"]) : 0;
                tblBlogCategory newBlogCategory = new tblBlogCategory();
                if (onayId > 0)
                {
                    tblBlogCategory getBlogCategory = new tblBlogCategory();
                    List<tblBlogCategory> getBlogCategoryList = (List<tblBlogCategory>)(from c in newEntity.tblBlogCategories where c.Cid == onayId select c).ToList();
                    if (getBlogCategoryList.Count > 0)
                    {
                        getBlogCategory = getBlogCategoryList.First();
                    }

                    return View(getBlogCategory);
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction("LoginAnnaSarah/2");
            }
        }

        [HttpPost]
        public ActionResult AddBlogCategory(tblBlogCategory tbc)
        {

            var onayId = RouteData.Values.ContainsKey("id") ? Convert.ToInt32(RouteData.Values["id"]) : 0;
            tblBlogCategory newBlogCategory = new tblBlogCategory();
            if (onayId > 0)
            {
                List<tblBlogCategory> getBlogCategoryList = (List<tblBlogCategory>)(from c in newEntity.tblBlogCategories where c.Cid == onayId select c).ToList();
                if (getBlogCategoryList.Count > 0)
                {
                    tblBlogCategory getBlogCategory = getBlogCategoryList.First();
                    getBlogCategory.CName = tbc.CName;
                }
            }
            else
            {
                newBlogCategory = tbc;
                newEntity.tblBlogCategories.Add(newBlogCategory);
            }


            if (newEntity.SaveChanges() > 0)
            {
                return RedirectToAction("Onay/7");
            }
            else
            {
                return RedirectToAction("Onay/8");
            }
        }


        public ActionResult BlogCategoryList()
        {
            if (Session["User"] != null)
            {
                ViewBag.Logo = "/images/logo-1.png";
                List<tblBlogCategory> getBlogCategoryList = (List<tblBlogCategory>)(from c in newEntity.tblBlogCategories select c).ToList();
                return View(getBlogCategoryList);
            }
            else
            {
                return RedirectToAction("LoginAnnaSarah/2");
            }
        }

        [HttpPost]
        public ActionResult DeleteBlogCategory(tblBlogCategory tbc)
        {
            int sayfaId = Convert.ToInt32(RouteData.Values["id"].ToString());
            List<tblBlogCategory> gBlogCatList = (List<tblBlogCategory>)(from c in newEntity.tblBlogCategories where c.Cid == sayfaId select c).ToList();
            if (gBlogCatList.Count > 0)
            {
                tblBlogCategory getBlogCategory = gBlogCatList.First();
                newEntity.tblBlogCategories.Remove(getBlogCategory);
                newEntity.SaveChanges();
            }

            return RedirectToAction("BlogCategoryList");
        }


        public ActionResult DeleteBlogCategory()
        {
            if (Session["User"] != null)
            {
                ViewBag.Logo = "/images/logo-1.png";
                int sayfaId = Convert.ToInt32(RouteData.Values["id"].ToString());
                List<tblBlogCategory> gBlogCatList = (List<tblBlogCategory>)(from c in newEntity.tblBlogCategories where c.Cid == sayfaId select c).ToList();
                if (gBlogCatList.Count > 0)
                {
                    tblBlogCategory getBlogCat = gBlogCatList.First();
                    ViewBag.Header = getBlogCat.CName;
                }
                return View();
            }
            else
            {
                return RedirectToAction("LoginAnnaSarah/2");
            }
        }


        public ActionResult CommentList()
        {
            if (Session["User"] != null)
            {
                ViewBag.Logo = "/images/logo-1.png";
                List<tblComment> getBlogCommentList = (List<tblComment>)(from c in newEntity.tblComments select c).ToList();
                return View(getBlogCommentList);
            }
            else
            {
                return RedirectToAction("LoginAnnaSarah/2");
            }
        }

        public ActionResult AddUser()
        {
            if (Session["User"] != null)
            {
                ViewBag.Logo = "/images/logo-1.png";
                return View();
            }
            else
            {
                return RedirectToAction("LoginAnnaSarah/2");
            }
        }

        [HttpPost]
        public ActionResult AddUser(tblWebUser newUser)
        {
            MD5Sifrele ss = new MD5Sifrele();
            string kk = ss.MD5StringSifrele(newUser.UsPassw);
            newUser.UsPassw = kk;
            newEntity.tblWebUsers.Add(newUser);
            if (newEntity.SaveChanges() > 0)
            {
                return RedirectToAction("Onay/9");
            }
            else
            {
                return RedirectToAction("Onay/10");
            }
        }


        public ActionResult UserList()
        {
            if (Session["User"] != null)
            {
                ViewBag.Logo = "/images/logo-1.png";
                List<tblWebUser> getUserList = (List<tblWebUser>)(from c in newEntity.tblWebUsers select c).ToList();
                return View(getUserList);
            }
            else
            {
                return RedirectToAction("LoginAnnaSarah/2");
            }
        }


        public ActionResult DeleteUser()
        {
            if (Session["User"] != null)
            {
                ViewBag.Logo = "/images/logo-1.png";
                int sayfaId = Convert.ToInt32(RouteData.Values["id"].ToString());
                List<tblWebUser> gUserList = (List<tblWebUser>)(from c in newEntity.tblWebUsers where c.UsId == sayfaId select c).ToList();
                if (gUserList.Count > 0)
                {
                    tblWebUser getUser = gUserList.First();
                    ViewBag.Header = getUser.UsName;
                }
                return View();
            }
            else
            {
                return RedirectToAction("LoginAnnaSarah/2");
            }
        }

        [HttpPost]
        public ActionResult DeleteUser(tblWebUser us1)
        {
            int sayfaId = Convert.ToInt32(RouteData.Values["id"].ToString());
            List<tblWebUser> gUserList = (List<tblWebUser>)(from c in newEntity.tblWebUsers where c.UsId == sayfaId select c).ToList();
            if (gUserList.Count > 0)
            {
                tblWebUser getUser = gUserList.First();
                newEntity.tblWebUsers.Remove(getUser);
                newEntity.SaveChanges();
            }

            return RedirectToAction("UserList");
        }

        public ActionResult LoginAnnaSarah()
        {
            var onayId = RouteData.Values.ContainsKey("id") ? Convert.ToInt32(RouteData.Values["id"]) : 0;
            if (onayId == 1)
            {
                ViewBag.Message = "User Name or Password Wrong";
            }
            if (onayId == 2)
            {
                ViewBag.Message = "You have not autharization this page...";
            }
            return View();
        }

        [HttpPost]
        public ActionResult LoginAnnaSarah(tblWebUser tWu)
        {
            MD5Sifrele ss = new MD5Sifrele();
            string kk = ss.MD5StringSifrele(tWu.UsPassw.Trim());
            List<tblWebUser> gUserList = (List<tblWebUser>)(from c in newEntity.tblWebUsers where c.UsName == tWu.UsName.Trim() && c.UsPassw == kk select c).ToList();
            if (gUserList.Count > 0)
            {
                Session["User"] = gUserList.First();
                return RedirectToAction("SubscribeList");
            }
            else
            {
                return RedirectToAction("LoginAnnaSarah/1");
            }
            //Session[]            
        }

    }
}