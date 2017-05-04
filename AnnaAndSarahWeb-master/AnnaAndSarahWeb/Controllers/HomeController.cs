using AnnaAndSarahWeb.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnnaAndSarah.Controllers
{
    public class HomeController : Controller
    {
        AnnaSarahEntities newEntity = new AnnaSarahEntities();
        public ActionResult Index()
        {
            List<tblTag> getTagAllList = (List<tblTag>)(from c in newEntity.tblTags select c).ToList();
            List<tblCategory> getCategoryAllList = (List<tblCategory>)(from c in newEntity.tblCategories where c.CMainId == 177 || c.CMainId == 178 || c.CMainId == 179 || c.CMainId == 180 || c.CMainId == 181 || c.Cid == 177 || c.Cid == 178 || c.Cid == 179 || c.Cid == 180 || c.Cid == 181 select c).ToList();
            List<tblCategory> getCategoryNuts = (List<tblCategory>)(from c in getCategoryAllList where c.CMainId == 177 orderby c.SiraNo select c).Take(4).ToList();
            List<tblCategory> getCategoryDriedFruits = (List<tblCategory>)(from c in getCategoryAllList where c.CMainId == 178 orderby c.SiraNo select c).Take(4).ToList();
            List<tblCategory> getCategorySeeds = (List<tblCategory>)(from c in getCategoryAllList where c.CMainId == 179 orderby c.SiraNo select c).Take(4).ToList();
            List<tblCategory> getCategorySnacks = (List<tblCategory>)(from c in getCategoryAllList where c.CMainId == 180 orderby c.SiraNo select c).Take(4).ToList();
            List<tblCategory> getCategoryGifts = (List<tblCategory>)(from c in getCategoryAllList where c.CMainId == 181 orderby c.SiraNo select c).Take(4).ToList();

            List<tblBanner> getBanners = (List<tblBanner>)(from c in newEntity.tblBanners orderby c.SiraNo select c).ToList();

            List<tblCategory> getNuts = (List<tblCategory>)(from c in getCategoryAllList where c.Cid == 177 select c).ToList();
            if (getNuts.Count > 0)
            {
                tblCategory getNut = getNuts.First();
                ViewBag.Nuts = getNut.Image;
            }

            List<tblCategory> getDriedFruits = (List<tblCategory>)(from c in getCategoryAllList where c.Cid == 178 select c).ToList();
            if (getDriedFruits.Count > 0)
            {
                tblCategory getDriedFruit = getDriedFruits.First();
                ViewBag.DriedFruits = getDriedFruit.Image;
            }
            List<tblCategory> getSeeds_Grains = (List<tblCategory>)(from c in getCategoryAllList where c.Cid == 179 select c).ToList();
            if (getSeeds_Grains.Count > 0)
            {
                tblCategory getSeeds_Grain = getSeeds_Grains.First();
                ViewBag.Seeds = getSeeds_Grain.Image;
            }
            List<tblCategory> getSnacks = (List<tblCategory>)(from c in getCategoryAllList where c.Cid == 180 select c).ToList();
            if (getSnacks.Count > 0)
            {
                tblCategory getSnack = getSnacks.First();
                ViewBag.Snacks = getSnack.Image;
            }
            List<tblCategory> getGifts = (List<tblCategory>)(from c in getCategoryAllList where c.Cid == 181 select c).ToList();
            if (getGifts.Count > 0)
            {
                tblCategory getGift = getGifts.First();
                ViewBag.Gifts = getGift.Image;
            }







            AnaSayfaPage newAnasayfa = new AnaSayfaPage();
            newAnasayfa.TagList = getTagAllList;
            newAnasayfa.CategoryDriedFruits = getCategoryDriedFruits;
            newAnasayfa.CategoryGifts = getCategoryGifts;
            newAnasayfa.CategoryNuts = getCategoryNuts;
            newAnasayfa.CategorySeeds = getCategorySeeds;
            newAnasayfa.CategorySnacks = getCategorySnacks;

            //tblBanner newBanner = new tblBanner();
            //List<tblBanner> lastNewBanner = new List<tblBanner>();
            //if (getBanners.Count > 0)
            //{                
            //    newBanner = (tblBanner)(from c in getBanners orderby c.SiraNo select c).First();
            //    newBanner.SiraNo = 1;
            //}
            

            //foreach (var item in getBanners)
            //{
            //    if (newBanner.Bid == item.Bid)
            //    {
            //        lastNewBanner.Add(newBanner);
            //    }
            //    else
            //    {
            //        lastNewBanner.Add(item);
            //    }
            //}
            newAnasayfa.Banners = getBanners;

            return View(newAnasayfa);
        }

        [HttpPost]
        public ActionResult Index(AnaSayfaPage getSubscribe, string command)
        {
            if (command == null)
            {
                string message = "";
                tbl_SubscribeList newSubscribe = new tbl_SubscribeList();
                newSubscribe.MailAddress = getSubscribe.MailAdress;
                newSubscribe.Tarih = DateTime.Now;
                if (ModelState.IsValid)
                {
                    try
                    {
                        newEntity.tbl_SubscribeList.Add(newSubscribe);
                        newEntity.SaveChanges();

                        message = "Successfully Saved!";
                    }
                    catch (Exception ex)
                    {
                        message = "Error! Please try again.";
                    }
                }
                else
                {
                    message = "Please provide required fields value.";
                }
                if (Request.IsAjaxRequest())
                {
                    return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    ViewBag.Message = message;
                    return View(getSubscribe);
                }
            }
            else
            {
                if (command.Equals("deneme1"))
                {
                    List<tblCategory> getCategorySearchList = (List<tblCategory>)(from c in newEntity.tblCategories where c.CatName.ToLower().Contains(getSubscribe.Search.ToLower()) select c).ToList();
                    if (getCategorySearchList.Count > 0)
                    {                        
                        tblCategory getCategory = getCategorySearchList.First();
                        List<tblCategory> getCategoryCocukVarmiList = (List<tblCategory>)(from c in newEntity.tblCategories where c.CMainId == getCategory.Cid select c).ToList();
                        //Aranan ürün, en alt ürün ise, direk ürün detaya yönlendir.
                        if (getCategoryCocukVarmiList.Count > 0)
                        {
                            return RedirectToAction("GetSearchList/" + getSubscribe.Search.Replace(" ","-").ToLower());
                        }
                        else
                        {
                            return RedirectToAction("Product/" + getCategory.LinkUrl);
                        }
                    }
                    else
                    {
                        return RedirectToAction("GetSearchList/" + getSubscribe.Search.Replace(" ", "-").ToLower());
                        //return RedirectToAction("Product/" + getCategory.LinkUrl);
                    }
                }
                else
                {
                    return View();
                }
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

        public ActionResult Category()
        {
            string sayfaId = RouteData.Values["id"].ToString(); //Hata çıkabilir, buraya dikkat...

            CategoryTree getCategoryTree = new CategoryTree();

            List<tblCategory> getCategoryList = new List<tblCategory>();
            List<tblCategory> getCategorySubList = new List<tblCategory>();

            List<tblCategory> getCategoryAllList = (List<tblCategory>)(from c in newEntity.tblCategories select c).ToList();

            List<tblCategory> getCategoryt = (List<tblCategory>)(from c in getCategoryAllList where c.LinkUrl == sayfaId select c).ToList();
            if (getCategoryt.Count > 0)
            {
                tblCategory getCategory = getCategoryt.First();
                ViewBag.Category = getCategory.CatName;
                getCategoryList = (List<tblCategory>)(from c in getCategoryAllList where c.CMainId == getCategory.Cid orderby c.SiraNo select c).ToList();
                foreach (tblCategory item in getCategoryList)
                {
                    if ((from c in getCategoryAllList where c.CMainId == item.Cid select c).ToList().Count() > 0)
                    {
                        List<tblCategory> getCategoryChildList = (List<tblCategory>)(from c in getCategoryAllList where c.CMainId == item.Cid orderby c.SiraNo select c).Take(4).ToList();
                        getCategorySubList.AddRange(getCategoryChildList);
                    }
                }
            }

            getCategoryTree.CategoryListChild = getCategorySubList;
            getCategoryTree.CategoryListMain = getCategoryList;


            UstMenu ss = new UstMenu();

            ViewBag.DriedFruits1 = ss.getDriedFruits();
            ViewBag.Seeds1 = ss.getSeeds_Grains();
            ViewBag.Snacks1 = ss.getSnacks();
            ViewBag.Gifts1 = ss.getGifts();
            ViewBag.Nuts1 = ss.getNut();





            return View(getCategoryTree);
        }

        public ActionResult Product()
        {
            //ViewBag.Message = "Your contact page.";
            string productId = RouteData.Values["id"].ToString();
            //        public IEnumerable<tblCategory> CategoryListSameCategory { get; set; }
            //public IEnumerable<tblCategory> CategoryListParentCategory { get; set; }

            ProductPage newProductPage = new ProductPage();

            tblCategory newProductDede = new tblCategory();

            List<tblCategory> newProductDedeList = new List<tblCategory>();

            tblCategory getCategoryProduct;
            List<tblCategory> getSameCategoryList = new List<tblCategory>();
            List<tblCategory> getParentCategoryList = new List<tblCategory>();
            List<tblCategory> getCategorySubList = new List<tblCategory>();

            List<tblCategory> getCategoryAllList = (List<tblCategory>)(from c in newEntity.tblCategories select c).ToList();
            List<tblCategory> getCategoryList = (List<tblCategory>)(from c in getCategoryAllList where c.LinkUrl == productId select c).ToList();
            if (getCategoryList.Count > 0)
            {
                getCategoryProduct = getCategoryList.First();
                getSameCategoryList = (List<tblCategory>)(from c in getCategoryAllList where c.CMainId == getCategoryProduct.CMainId orderby c.SiraNo select c).Take(9).ToList();
                getSameCategoryList.Remove(getCategoryProduct);

                newProductDedeList = (List<tblCategory>)(from c in getCategoryAllList where c.Cid == getCategoryProduct.CMainId select c).ToList();
                if (newProductDedeList.Count > 0)
                {
                    newProductDede = newProductDedeList.First();
                }

                getParentCategoryList = (List<tblCategory>)(from c in getCategoryAllList where c.CMainId == newProductDede.CMainId orderby c.SiraNo select c).ToList();

                newProductPage.Category = getCategoryProduct;
                newProductPage.CategoryListSameCategory = getSameCategoryList;
                newProductPage.CategoryListParentCategory = getParentCategoryList;

                foreach (tblCategory item in getParentCategoryList)
                {
                    if ((from c in getCategoryAllList where c.CMainId == item.Cid select c).ToList().Count() > 0)
                    {
                        List<tblCategory> getCategoryChildList = (List<tblCategory>)(from c in getCategoryAllList where c.CMainId == item.Cid orderby c.SiraNo select c).Take(4).ToList();
                        getCategorySubList.AddRange(getCategoryChildList);
                    }
                }
                newProductPage.CategoryListParentChildCategory = getCategorySubList;


                //Ürün özelliklerini bağla...
                //ProductDetail
                List<tblProductDetail> getProductDetailList = (List<tblProductDetail>)(from c in newEntity.tblProductDetails where c.CatId == getCategoryProduct.Cid select c).ToList();
                if (getProductDetailList.Count > 0)
                {
                    tblProductDetail getProductDetailFirst = getProductDetailList.First();
                    newProductPage.ProductDetail = getProductDetailFirst;
                }
                else
                {
                    tblProductDetail getProductDetailFirst = new tblProductDetail();
                    newProductPage.ProductDetail = getProductDetailFirst;
                }
            }

            UstMenu ss = new UstMenu();

            ViewBag.DriedFruits1 = ss.getDriedFruits();
            ViewBag.Seeds1 = ss.getSeeds_Grains();
            ViewBag.Snacks1 = ss.getSnacks();
            ViewBag.Gifts1 = ss.getGifts();
            ViewBag.Nuts1 = ss.getNut();

            return View(newProductPage);
        }


        public ActionResult TagProduct()
        {
            var tagId = RouteData.Values.ContainsKey("id") ? Convert.ToInt32(RouteData.Values["id"]) : 0;
            List<vw_tag_cat_product> getProductDetailList = (List<vw_tag_cat_product>)(from c in newEntity.vw_tag_cat_product where c.TagId == tagId select c).ToList();

            UstMenu ss = new UstMenu();

            ViewBag.DriedFruits1 = ss.getDriedFruits();
            ViewBag.Seeds1 = ss.getSeeds_Grains();
            ViewBag.Snacks1 = ss.getSnacks();
            ViewBag.Gifts1 = ss.getGifts();
            ViewBag.Nuts1 = ss.getNut();

            if (getProductDetailList.Count > 0)
            {
                vw_tag_cat_product getTagProduct = getProductDetailList.First();
                ViewBag.Category = getTagProduct.TagName;
                return View(getProductDetailList);
            }
            else
            {
                List<vw_tag_cat_product> getProductDetailList2 = new List<vw_tag_cat_product>();
                return View(getProductDetailList2);
            }
        }

        public ActionResult Conditions()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult WhoWeAre()
        {
            //ViewBag.Message = "Your contact page.";
            UstMenu ss = new UstMenu();
            ViewBag.Category = "Who We Are";
            ViewBag.DriedFruits1 = ss.getDriedFruits();
            ViewBag.Seeds1 = ss.getSeeds_Grains();
            ViewBag.Snacks1 = ss.getSnacks();
            ViewBag.Gifts1 = ss.getGifts();
            ViewBag.Nuts1 = ss.getNut();
            return View();
        }


        public ActionResult BlogList(int? page)
        {
            BlogFrontList pageSon = new BlogFrontList();

            UstMenu ss = new UstMenu();

            ViewBag.DriedFruits1 = ss.getDriedFruits();
            ViewBag.Seeds1 = ss.getSeeds_Grains();
            ViewBag.Snacks1 = ss.getSnacks();
            ViewBag.Gifts1 = ss.getGifts();
            ViewBag.Nuts1 = ss.getNut();

            int pageIndex = page ?? 1;
            int dataCount = 10;
            var catId = RouteData.Values.ContainsKey("id") ? Convert.ToInt32(RouteData.Values["id"]) : 0;
            ViewBag.Category = "Anna and Sarah's Health Corner";
            List<vw_blog_category> getBlogCategoryList = new List<vw_blog_category>();
            if (catId > 0)
            {
                getBlogCategoryList = (List<vw_blog_category>)(from c in newEntity.vw_blog_category where c.Cid == catId orderby c.Bid descending select c).ToList();
            }
            else
            {
                getBlogCategoryList = (List<vw_blog_category>)(from c in newEntity.vw_blog_category orderby c.Bid descending select c).ToList();
            }

            BlogPaket bp = new BlogPaket();
            List<tblComment> getBlogCommentList = (List<tblComment>)(from c in newEntity.tblComments select c).ToList();

            BlogFrontList crBlogFrontList = new BlogFrontList();
            List<BlogPaket> bPaketList = new List<BlogPaket>();
            foreach (vw_blog_category item in getBlogCategoryList)
            {
                bp = new BlogPaket();
                
                int commentCount = (int)(from c in getBlogCommentList where c.Bid == item.Bid select c).Count();
                bp.count = commentCount;
                
                if (item.BText.Length > 512)
                {
                    item.BText = item.BText.Substring(0, 512);
                }
                bp.blog_category = item;
                bPaketList.Add(bp);
            }


            crBlogFrontList = new BlogFrontList();
            crBlogFrontList.BlogPaketList = bPaketList; 

            List<tblBlogCategory> getBlogCategorytList = (List<tblBlogCategory>)(from c in newEntity.tblBlogCategories select c).ToList();
            crBlogFrontList.CategoryWidgetList = getBlogCategorytList;

            List<tblBlog> getBlogList = (List<tblBlog>)(from c in newEntity.tblBlogs orderby c.OkunmaSayisi descending select c).Take(4).ToList();
            crBlogFrontList.BlogWidgetList = getBlogList;

            //var result = crBlogFrontList.BlogPaketList.OrderBy(x => x.blog_category.BAd).ToPagedList(pageIndex, dataCount);

            return View(crBlogFrontList);
        }

        public ActionResult Blog()
        {
            UstMenu ss = new UstMenu();

            ViewBag.DriedFruits1 = ss.getDriedFruits();
            ViewBag.Seeds1 = ss.getSeeds_Grains();
            ViewBag.Snacks1 = ss.getSnacks();
            ViewBag.Gifts1 = ss.getGifts();
            ViewBag.Nuts1 = ss.getNut();

            var blogId = RouteData.Values.ContainsKey("id") ? RouteData.Values["id"].ToString() : "";
            List<vw_blog_category> getBlogCategoryList = new List<vw_blog_category>();
            int commentCount = 0;
            ViewBag.Category = "Anna and Sarah's Health Corner";
            vw_blog_category getblogCategory = new vw_blog_category();
            BlogPaket bp = new BlogPaket(); 
            if (blogId.Length > 0)
            {
                getBlogCategoryList = (List<vw_blog_category>)(from c in newEntity.vw_blog_category where c.UrlRewrite == blogId orderby c.Bid descending select c).ToList();
                if (getBlogCategoryList.Count > 0)
                {
                    getblogCategory = getBlogCategoryList.First();                    
                    bp.blog_category = getblogCategory;

                    List<tblBlog> getBlgList = (List<tblBlog>)(from c in newEntity.tblBlogs where c.UrlRewrite == blogId orderby c.Bid descending select c).ToList();
                    if (getBlgList.Count > 0)
                    {
                        tblBlog getBlg = getBlgList.First();
                        if (getBlg.OkunmaSayisi == null)
                        {
                            getBlg.OkunmaSayisi = 1;
                        }
                        else
                        {
                            getBlg.OkunmaSayisi = getBlg.OkunmaSayisi + 1;
                        }
                        newEntity.SaveChanges();
                    }

                    List<tblComment> getBlogCommentList = (List<tblComment>)(from c in newEntity.tblComments where c.Bid == getblogCategory.Bid orderby c.Cid descending select c).ToList();
                    if (getBlogCommentList.Count > 0)
                    {
                        commentCount = getBlogCommentList.Count;
                        bp.count = commentCount;
                        bp.commentList = getBlogCommentList;
                    }
                    else
                    {
                        bp.commentList = new List<tblComment>();
                    }
                }
                               

                List<tblBlogCategory> getBlogCategorytList = (List<tblBlogCategory>)(from c in newEntity.tblBlogCategories select c).ToList();
                bp.CategoryWidgetList = getBlogCategorytList;

                List<tblBlog> getBlogList = (List<tblBlog>)(from c in newEntity.tblBlogs orderby c.OkunmaSayisi descending select c).Take(4).ToList();
                bp.BlogWidgetList = getBlogList;

                return View(bp);
            }
            else
            {
                return View();
            }            
        }

        [HttpPost]
        public ActionResult Blog(BlogPaket bp)
        {
            var blogId = RouteData.Values.ContainsKey("id") ? RouteData.Values["id"].ToString() : "";
            List<vw_blog_category> getBlogCategoryList = new List<vw_blog_category>();
            vw_blog_category getblogCategory = new vw_blog_category();
            if (blogId.Length > 0)
            {
                getBlogCategoryList = (List<vw_blog_category>)(from c in newEntity.vw_blog_category where c.UrlRewrite == blogId orderby c.Bid descending select c).ToList();
                if (getBlogCategoryList.Count > 0)
                {
                    getblogCategory = getBlogCategoryList.First();
                    tblComment tCom = new tblComment();
                    tCom.Bid = getblogCategory.Bid;
                    tCom.Comment = bp.comment.Comment;
                    tCom.Email = bp.comment.Email;
                    tCom.Name = bp.comment.Name;
                    tCom.Tarih = DateTime.Now;
                    newEntity.tblComments.Add(tCom);
                    if (newEntity.SaveChanges() > 0)
                    {
                        return RedirectToAction("Blog/" + blogId);
                    }
                    else
                    {
                        return RedirectToAction("Blog/" + blogId);
                    }                    
                }
                else
                {
                    return RedirectToAction("BlogList");
                }
            }
            else
            {
                return RedirectToAction("BlogList");
            }                
        }



        public ActionResult ContactUs()
        {
            var tagId = RouteData.Values.ContainsKey("id") ? Convert.ToInt32(RouteData.Values["id"]) : 0;
            UstMenu ss = new UstMenu();
            ViewBag.Category = "Contact Us"; 
            ViewBag.DriedFruits1 = ss.getDriedFruits();
            ViewBag.Seeds1 = ss.getSeeds_Grains();
            ViewBag.Snacks1 = ss.getSnacks();
            ViewBag.Gifts1 = ss.getGifts();
            ViewBag.Nuts1 = ss.getNut();

            if (tagId != 0)
            {
                if (tagId.ToString() == "1")
                {
                    ViewBag.Uyar = "Your message has been successfully...";
                }
                else if (tagId.ToString() == "2")
                {
                    ViewBag.Uyar = "Ooops, something worong...";
                }              
            }
            else
            {
                ViewBag.Uyar = "";
            }
            return View();
        }
        [HttpPost]
        public ActionResult ContactUs(Contact newContact)
        {
            if (newContact == null)
            {
                return RedirectToAction("ContactUs/4");
            }
            else
            {
                tblContact getnewContact = new tblContact();
                getnewContact.Email = newContact.Email;
                getnewContact.FirstName = newContact.FirstName;
                getnewContact.LastName = newContact.LastName;
                getnewContact.Text = newContact.Text;
                getnewContact.Topic = newContact.Topic;

                newEntity.tblContacts.Add(getnewContact);
                if (newEntity.SaveChanges() > 0)
                {
                    return RedirectToAction("ContactUs/1");
                }
                else
                {
                    return RedirectToAction("ContactUs/2");
                }
            }
        }


        public ActionResult GetCategoryList()
        {
            var catId = RouteData.Values.ContainsKey("id") ? Convert.ToInt32(RouteData.Values["id"]) : 0;
            List<tblCategory> getProductDetailList = (List<tblCategory>)(from c in newEntity.tblCategories where c.CMainId == catId orderby c.SiraNo select c).ToList();

            UstMenu ss = new UstMenu();

            ViewBag.DriedFruits1 = ss.getDriedFruits();
            ViewBag.Seeds1 = ss.getSeeds_Grains();
            ViewBag.Snacks1 = ss.getSnacks();
            ViewBag.Gifts1 = ss.getGifts();
            ViewBag.Nuts1 = ss.getNut();

            return View(getProductDetailList);
        }
        

        public ActionResult DenemeSayfa()
        {          
            return View();
        }

        [HttpPost]
        public ActionResult DenemeSayfa2(string SearchText)
        {
            List<tblCategory> getCategorySearchList = (List<tblCategory>)(from c in newEntity.tblCategories where c.CatName.ToLower().Contains(SearchText.ToLower()) select c).ToList();
            if (getCategorySearchList.Count > 0)
            {
                tblCategory getCategory = getCategorySearchList.First();
                List<tblCategory> getCategoryCocukVarmiList = (List<tblCategory>)(from c in newEntity.tblCategories where c.CMainId == getCategory.Cid select c).ToList();
                //Aranan ürün, en alt ürün ise, direk ürün detaya yönlendir.
                if (getCategoryCocukVarmiList.Count > 0)
                {
                    return RedirectToAction("GetSearchList/" + SearchText.Replace(" ", "-").ToLower());
                }
                else
                {
                    return RedirectToAction("Product/" + getCategory.LinkUrl);
                }
            }
            else
            {
                return RedirectToAction("GetSearchList/" + SearchText.Replace(" ", "-").ToLower());
                //return RedirectToAction("Product/" + getCategory.LinkUrl);
            }
        }


        [HttpPost]
        public ActionResult DenemeSayfa(string query)
        {
            var getProductDetailList = (from c in newEntity.tblCategories where c.CatName.ToLower().Contains(query.ToLower()) select c.CatName).Distinct();
            //return View(getProductDetailList);

            return Json(new { Data = getProductDetailList });
        }

        public ActionResult GetSearchList()
        {
            var searchStr = RouteData.Values.ContainsKey("id") ? RouteData.Values["id"].ToString() : "";

            UstMenu ss = new UstMenu();

            ViewBag.DriedFruits1 = ss.getDriedFruits();
            ViewBag.Seeds1 = ss.getSeeds_Grains();
            ViewBag.Snacks1 = ss.getSnacks();
            ViewBag.Gifts1 = ss.getGifts();
            ViewBag.Nuts1 = ss.getNut();

            ViewBag.Category = searchStr.Replace("-", " ");
            List<tblCategory> getProductDetailList = (List<tblCategory>)(from c in newEntity.tblCategories where c.CatName.ToLower().Contains(searchStr.Replace("-"," ").ToLower()) select c).ToList();
            if (getProductDetailList.Count > 0)
            {
                return View(getProductDetailList);
            }
            else
            {
                List<tblTag> getTagList = (List<tblTag>)(from c in newEntity.tblTags where c.TagName.ToLower().Contains(searchStr.Replace("-", " ").ToLower()) select c).ToList();
                if (getTagList.Count > 0)
                {
                    tblTag getTag = getTagList.First();
                    List<tblTagCategory> getTagCatList = (List<tblTagCategory>)(from c in newEntity.tblTagCategories where c.TagId == getTag.TagId select c).ToList();
                    if (getTagCatList.Count > 0)
                    {
                        tblTagCategory getTagCat = getTagCatList.First();
                        List<tblCategory> getCategoryTagList = (List<tblCategory>)(from c in newEntity.tblCategories where c.Cid == getTagCat.CatId select c).ToList();
                        if (getCategoryTagList.Count > 0)
                        {
                            return View(getCategoryTagList);
                        }
                        else
                        {
                            ViewBag.SearchMesaj = "We couldn't find any product :( ";
                            return View();
                        }
                    }
                    else
                    {
                        ViewBag.SearchMesaj = "We couldn't find any product :( ";
                        return View();
                    }
                    //List<tblCategory> getProductTagList = (List<tblCategory>)(from c in newEntity.tblCategories where c. select c).ToList();
                }
                else
                {
                    ViewBag.SearchMesaj = "We couldn't find any product :( ";
                    return View();
                }
            }            
        }


        //public ActionResult categoryblog(List<BlogFrontList> newBlogFrontList)
        //{

        //    //List<vw_blog_category> getBlogCategoryList = (List<vw_blog_category>)(from c in newEntity.vw_blog_category select c).ToList();
        //    //List<tblComment> getBlogCommentList = (List<tblComment>)(from c in newEntity.tblComments select c).ToList();
        //    //int count = 0;
        //    //BlogFrontList newCatBlog = new BlogFrontList();
        //    //List<BlogFrontList> newCatBlogList = new List<BlogFrontList>();
        //    //foreach (var item in getBlogCategoryList)
        //    //{
        //    //    newCatBlog.BlogCategoryList = item;
        //    //    count = Convert.ToInt32((from c in getBlogCommentList where c.Bid == item.Bid select c).Count());
        //    //    newCatBlog.CommentCount = count;
        //    //    newCatBlogList.Add(newCatBlog);
        //    //}

        //    //List<tblBlog> getBlogCategoryList = (List<tblBlog>)(from c in newEntity.tblBlogs orderby c.OkunmaSayisi descending select c).Take(4).ToList();
        //    //BlogFrontList newCatBlog = new BlogFrontList();
        //    //List<BlogFrontList> newCatBlogList = new List<BlogFrontList>();
        //    //foreach (var item in getBlogCategoryList)
        //    //{
        //    //    newCatBlog.Blog = item;
        //    //    newCatBlogList.Add(newCatBlog);
        //    //}

        //    ////CategoryBlog
        //    //return View(newCatBlogList);

        //    //CategoryBlog
        //    BlogFrontList newCatList = new BlogFrontList();
        //    List<tblBlogCategory> getBlogCategorylist = (List<tblBlogCategory>)(from c in newEntity.tblBlogCategories select c).ToList();
        //    List<BlogFrontList> newCatBlogList = new List<BlogFrontList>();
        //    foreach (var item in getBlogCategorylist)
        //    {
        //        newCatList.Category = item;
        //        newCatBlogList.Add(newCatList);
        //    }
        //    //newCatList.CategoryList = getBlogCategorylist;
        //    return View(newCatBlogList);
        //}

        //public ActionResult PopularPosts(List<tblBlog> newBlogFrontList)
        //{

        //    List<tblBlog> getBlogCategoryList = (List<tblBlog>)(from c in newEntity.tblBlogs orderby c.OkunmaSayisi descending select c).Take(4).ToList();
        //    BlogFrontList newCatBlog = new BlogFrontList();
        //    List<BlogFrontList> newCatBlogList = new List<BlogFrontList>();
        //    foreach (var item in getBlogCategoryList)
        //    {
        //        newCatBlog.Blog = item;
        //        newCatBlogList.Add(newCatBlog);
        //    }

        //    //CategoryBlog
        //    return View(newCatBlogList);
        //}



        public ActionResult CategoryBlogDetay()
        {

            List<vw_blog_category> getBlogCategoryList = (List<vw_blog_category>)(from c in newEntity.vw_blog_category select c).ToList();
            List<tblComment> getBlogCommentList = (List<tblComment>)(from c in newEntity.tblComments select c).ToList();
            int count = 0;
            BlogFrontListDetay newCatBlog = new BlogFrontListDetay();
            List<BlogFrontListDetay> newCatBlogList = new List<BlogFrontListDetay>();
            foreach (var item in getBlogCategoryList)
            {
                newCatBlog.BlogCategory = item;
                count = Convert.ToInt32((from c in getBlogCommentList where c.Bid == item.Bid select c).Count());
                newCatBlog.CommentCount = count;
                newCatBlogList.Add(newCatBlog);
            }

            //CategoryBlog
            return View(newCatBlogList);
        }

        public ActionResult PopularDetayPosts()
        {

            List<tblBlog> getBlogCategoryList = (List<tblBlog>)(from c in newEntity.tblBlogs orderby c.OkunmaSayisi descending select c).Take(4).ToList();
            BlogFrontListDetay newCatBlog = new BlogFrontListDetay();
            List<BlogFrontListDetay> newCatBlogList = new List<BlogFrontListDetay>();
            foreach (var item in getBlogCategoryList)
            {
                newCatBlog.Blog = item;
                newCatBlogList.Add(newCatBlog);
            }

            //CategoryBlog
            return View(newCatBlogList);
        }

        //public ActionResult deneme3()
        //{
        //    ViewBag.Nuts1 = "asdasd";
        //    List<tblCategory> getCategoryAllList = (List<tblCategory>)(from c in newEntity.tblCategories select c).ToList();
        //    List<tblCategory> getNuts = (List<tblCategory>)(from c in getCategoryAllList where c.Cid == 177 select c).ToList();
        //    if (getNuts.Count > 0)
        //    {
        //        tblCategory getNut = getNuts.First();
        //        ViewBag.Nuts1 = getNut.Image;
        //    }

        //    List<tblCategory> getDriedFruits = (List<tblCategory>)(from c in getCategoryAllList where c.Cid == 178 select c).ToList();
        //    if (getDriedFruits.Count > 0)
        //    {
        //        tblCategory getDriedFruit = getDriedFruits.First();
        //        ViewBag.DriedFruits1 = getDriedFruit.Image;
        //    }
        //    List<tblCategory> getSeeds_Grains = (List<tblCategory>)(from c in getCategoryAllList where c.Cid == 179 select c).ToList();
        //    if (getSeeds_Grains.Count > 0)
        //    {
        //        tblCategory getSeeds_Grain = getSeeds_Grains.First();
        //        ViewBag.Seeds1 = getSeeds_Grain.Image;
        //    }
        //    List<tblCategory> getSnacks = (List<tblCategory>)(from c in getCategoryAllList where c.Cid == 180 select c).ToList();
        //    if (getSnacks.Count > 0)
        //    {
        //        tblCategory getSnack = getSnacks.First();
        //        ViewBag.Snacks1 = getSnack.Image;
        //    }
        //    List<tblCategory> getGifts = (List<tblCategory>)(from c in getCategoryAllList where c.Cid == 181 select c).ToList();
        //    if (getGifts.Count > 0)
        //    {
        //        tblCategory getGift = getGifts.First();
        //        ViewBag.Gifts1 = getGift.Image;
        //    }

        //    //CategoryBlog
        //    return View();
        //}        

    }
}