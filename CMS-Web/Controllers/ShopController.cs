using CMS_DTO.CMSCustomerInfor;
using CMS_DTO.CMSProduct;
using CMS_Shared;
using CMS_Shared.CMSBrands;
using CMS_Shared.CMSCategories;
using CMS_Shared.CMSCustomerInfor;
using CMS_Shared.CMSLocations;
using CMS_Shared.CMSProducts;
using CMS_Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_Web.Controllers
{
    public class ShopController : HQController
    {
        private readonly CMSProductFactory _fac;
        private readonly CMSCategoriesFactory _facCate;
        private readonly CMSBrandsFactory _facBrand;
        private readonly CMSLocationFactory _facLoca;
        private readonly CMSCustomerInforFactory _facCusInfor;
        private int PageSize = 8;
        public ShopController()
        {
            _fac = new CMSProductFactory();
            _facCate = new CMSCategoriesFactory();
            _facBrand = new CMSBrandsFactory();
            _facLoca = new CMSLocationFactory();
            _facCusInfor = new CMSCustomerInforFactory();
            //ViewBag.Range = GetListRangeSelectItem();
        }
        // GET: Shop
        public ActionResult Index(string q = "", int  page = 1, string a = "",int pageSize = 8, int sortBy = 1, decimal min = 0, decimal max = 0 )
        {
            try
            {
                ProductViewModels model = new ProductViewModels();
                //Category
                model.ListCate = _facCate.GetList().OrderBy(x => x.CategoryName).ToList();
                if (!string.IsNullOrEmpty(q))
                {
                    var dataCurrentCate = model.ListCate.FirstOrDefault(x => x.Alias.Equals(q));
                    if (dataCurrentCate != null)
                    {
                        model.CurrentCateAlias = string.IsNullOrEmpty(q) ? "" : dataCurrentCate.Alias;
                        model.CurrentCategoryName = string.IsNullOrEmpty(q) ? "" : dataCurrentCate.CategoryName;
                    }
                }
                
                //Product
                int totalRecord, totalPage;
                model.ListProduct = _fac.GetListProductByCategory(q, out totalRecord, out totalPage, page, pageSize, a, sortBy, min, max);
                var info = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");
                if (model.ListProduct != null && model.ListProduct.Any())
                {
                    model.ListProduct.ForEach(x =>
                    {
                        if (!string.IsNullOrEmpty(x.ImageURL))
                            x.ImageURL = Commons._PublicImages + "Products/" + x.ImageURL;
                        else
                            x.ImageURL = "";

                        x.sPrice = String.Format(info, "{0:C0}", x.ProductPrice);
                    });
                }
                model.TotalProduct = totalRecord;
                model.TotalPage = totalPage;
                model.CurrentPage = page;
                model.CurrentSortNew = a;
                model.CurrentSortBy = sortBy;
                model.PageSize = pageSize;
                model.Star1 = _fac.GetTotalProductByStar(1);
                model.Star2 = _fac.GetTotalProductByStar(2);
                model.Star3 = _fac.GetTotalProductByStar(3);
                model.Star4 = _fac.GetTotalProductByStar(4);
                model.Star5 = _fac.GetTotalProductByStar(5);
                return View(model);
            }
            catch (Exception ex)
            {
                NSLog.Logger.Error("Index: ", ex);
                return RedirectToAction("Index", "NotFound");
            }
        }

        public ActionResult Detail(string q)
        {
            ProductViewModels model = new ProductViewModels();
            try
            {
                var info = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");
                //Category
                model.ListCate = _facCate.GetList().OrderBy(x => x.CategoryName).ToList();
                if (!string.IsNullOrEmpty(q))
                {
                    model.ProductModel = _fac.GetProductByAlias(q);
                    if (!string.IsNullOrEmpty(model.ProductModel.ImageURL))
                        model.ProductModel.ImageURL = Commons.HostImage + "Products/" + model.ProductModel.ImageURL;

                    model.ProductModel.sPrice = String.Format(info, "{0:C0}", model.ProductModel.ProductPrice);
                    model.ProductModel.ListImg = model.ProductModel.ListImg.Skip(0).Take(4).ToList();
                    if (model.ProductModel.ListImg != null)
                    {
                        model.ProductModel.ListImg.ForEach(x =>
                        {
                            if (x != null)
                            {
                                if (!string.IsNullOrEmpty(x.ImageURL))
                                    x.ImageURL = Commons.HostImage + "Products/" + x.ImageURL;
                            }
                        });
                    }

                    model.ListSameProduct = _fac.GetList().Where(o => o.CategoryId.Equals(model.ProductModel.CategoryId)).Skip(0).Take(3).OrderByDescending(o => o.CreatedDate).ToList();
                    if (model.ListSameProduct != null && model.ListSameProduct.Any())
                    {
                        model.ListSameProduct.ForEach(x =>
                        {
                            if (!string.IsNullOrEmpty(x.ImageURL))
                                x.ImageURL = Commons.HostImage + "Products/" + x.ImageURL;

                            x.sPrice = String.Format(info, "{0:C0}", x.ProductPrice);
                        });
                    }
                    return View(model);
                }
                else
                {
                    return RedirectToAction("Index", "NotFound");
                }
            }
            catch (Exception ex)
            {
                NSLog.Logger.Error("GetDetail: ", ex);
                return RedirectToAction("Index", "NotFound");
            }
        }

        public ActionResult RequestInfor()
        {
            var name = Request.QueryString["name"];
            var email = Request.QueryString["email"];
            var phone = Request.QueryString["phone"];
            var subject = Request.QueryString["subject"];
            var message = Request.QueryString["message"];
            var model = new CMS_CustomerInforModels();
            model.Subject = "Customer nhận xét Product: " + subject;
            model.Name = name;
            model.Email = email;
            model.Phone = phone;
            model.Message = message;
            model.ReceiveType = (int)Commons.ETypeUserRequest.RequestInfor;
            //Save db
            string msg = "";
            var _temp = _facCusInfor.CreateOrUpdate(model, ref msg);
            if (!_temp)
            {
                NSLog.Logger.Info("SendMail_Home_RequestInfor: ", msg);
            }
            string boby = string.Empty;
            try
            {
                boby += "<div class='clearfix'>";
                boby += "<h3>Cám ơn bạn đã quan tâm tới Product của chúng tôi.</h3>";
                boby += "<p><b>Preferredt Date:</b> <i>" + DateTime.Now.ToString("dd/MM/yyyy hh:mm") + "</i></p>";
                boby += "<p><b>Last name:</b> <i>" + name + "</i></p>";
                boby += "<p><b>Email:</b> <i>" + email + "</i>||<b>Sđt:</b> <i>" + phone + "</i></p>";
                boby += "<p><b>Content:</b> <i>" + message + "</i></p>";
                boby += "</div>";
                model.Body = boby;
                var result = MailHelper.SendMailInfor(model);
            }
            catch (Exception ex)
            {
                NSLog.Logger.Error("SendMail_Home_Subcriber: ", ex);
            }
            return RedirectToAction("Index");
        }
        
        public ActionResult RequestBook()
        {
            var name = Request.QueryString["name"];
            var email = Request.QueryString["email"];
            var phone = Request.QueryString["phone"];
            var bookDate = Request.QueryString["bookDate"];
            var bookTime = Request.QueryString["bookTime"];
            var ischeckedMail = Request.QueryString["ischeckedMail"];
            var ischeckedPhone = Request.QueryString["ischeckedPhone"];
            var model = new CMS_CustomerInforModels();
            model.Subject = "Yêu cầu đặt lịch để tư vấn";
            model.Name = name;
            model.Email = email;
            model.Phone = phone;
            model.PreferredtDate = DateTime.ParseExact(bookDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            model.PreferredtTime = DateTime.Parse(bookTime).TimeOfDay;
            model.CheckedMail = ischeckedMail == "on" ? true : false;
            model.CheckedPhone = ischeckedPhone == "on" ? true : false;
            model.ReceiveType = (int)Commons.ETypeUserRequest.RequestBook;
            //Save db
            string msg = "";
            var _temp = _facCusInfor.CreateOrUpdate(model, ref msg);
            if (!_temp)
            {
                NSLog.Logger.Info("SendMail_Home_RequestInfor: ", msg);
            }
            string boby = string.Empty;
            try
            {
                boby += "<div class='clearfix'>";
                boby += "<h3>Cám ơn bạn đã quan tâm tới Product của chúng tôi.</h3>";
                boby += "<p><b>Ngày đặt:</b> <i>" + DateTime.Now.ToString("dd/MM/yyyy") + "</i></p>";
                boby += "<p><b>Last name:</b> <i>" + name + "</i></p>";
                boby += "<p><b>Email:</b> <i>" + email + "</i>||<b>Sđt:</b> <i>" + phone + "</i></p>";
                boby += "<p><b>Ngày đặt:</b> <i>" + bookDate + "</i></p>";
                boby += "<p><b>Thời gian đặt:</b> <i>" + bookTime + "</i></p>";
                boby += "</div>";
                model.Body = boby;
                var result = MailHelper.SendMailInfor(model);
            }
            catch (Exception ex)
            {
                NSLog.Logger.Error("SendMail_Home_RequestBook: ", ex);
            }
            return RedirectToAction("Index");
        }

        public ActionResult RequestMakeOffer()
        {
            var name = Request.QueryString["name"];
            var email = Request.QueryString["email"];
            var phone = Request.QueryString["phone"];
            var price = Request.QueryString["price"];
            var financingRequired = Request.QueryString["financingRequired"];
            var message = Request.QueryString["message"];
            var ischeckedMail = Request.QueryString["ischeckedMail"];
            var ischeckedPhone = Request.QueryString["ischeckedPhone"];
            var model = new CMS_CustomerInforModels();
            model.Subject = "Yêu cầu tư vấn Price cho Product";
            model.Name = name;
            model.Email = email;
            model.Phone = phone;
            model.FinancingRequired = financingRequired == "on" ? 0 : 1;
            model.CheckedMail = ischeckedMail == "on" ? true : false;
            model.CheckedPhone = ischeckedPhone == "on" ? true : false;
            model.Message = message;
            model.ReceiveType = (int)Commons.ETypeUserRequest.RequestMakeInfor;
            //Save db
            string msg = "";
            var _temp = _facCusInfor.CreateOrUpdate(model, ref msg);
            if (!_temp)
            {
                NSLog.Logger.Info("SendMail_Home_RequestMakeOffer: ", msg);
            }
            string boby = string.Empty;
            try
            {
                boby += "<div class='clearfix'>";
                boby += "<h3>Cám ơn bạn đã quan tâm tới Product của chúng tôi.</h3>";
                boby += "<p><b>Ngày đặt:</b> <i>" + DateTime.Now.ToString("dd/MM/yyyy") + "</i></p>";
                boby += "<p><b>Last name:</b> <i>" + name + "</i></p>";
                boby += "<p><b>Email:</b> <i>" + email + "</i>||<b>Sđt:</b> <i>" + phone + "</i></p>";
                boby += "<p><b>Yêu cầu trả góp:</b> <i>Liên hệ</i></p>";
                boby += "<p><b>Content:</b> <i>" + message + "</i></p>";
                boby += "</div>";
                model.Body = boby;
                var result = MailHelper.SendMailInfor(model);
            }
            catch (Exception ex)
            {
                NSLog.Logger.Error("SendMail_Home_RequestMakeOffer: ", ex);
            }
            return RedirectToAction("Index");
        }

        public ActionResult RequestSendInforFriend()
        {
            var name = Request.QueryString["name"];
            var email = Request.QueryString["email"];
            var emailFriend = Request.QueryString["emailFriend"];
            var message = Request.QueryString["message"];
            var ischeckedMail = Request.QueryString["ischeckedMail"];
            var ischeckedPhone = Request.QueryString["ischeckedPhone"];
            var model = new CMS_CustomerInforModels();
            model.Subject = "Đề nghị gửi yêu cầu cho bạn bè";
            model.Name = name;
            model.Email = email;
            model.EmailFriend = emailFriend;
            model.Message = message;
            model.ReceiveType = (int)Commons.ETypeUserRequest.RequestSendFriend;
            model.CheckedMail = ischeckedMail == "on" ? true : false;
            model.CheckedPhone = ischeckedPhone == "on" ? true : false;
            //Save db
            string msg = "";
            var _temp = _facCusInfor.CreateOrUpdate(model, ref msg);
            if (!_temp)
            {
                NSLog.Logger.Info("SendMail_Home_RequestSendInforFriend: ", msg);
            }
            string boby = string.Empty;
            try
            {
                boby += "<div class='clearfix'>";
                boby += "<h3>Cám ơn bạn đã quan tâm tới Product của chúng tôi.</h3>";
                boby += "<p><b>Ngày đặt:</b> <i>" + DateTime.Now.ToString("dd/MM/yyyy") + "</i></p>";
                boby += "<p><b>Last name:</b> <i>" + name + "</i></p>";
                boby += "<p><b>Email:</b> <i>" + email + "</i></p>";
                boby += "<p><b>Gửi tới Email:</b> <i>" + emailFriend + "</i></p>";
                boby += "<p><b>Content:</b> <i>" + message + "</i></p>";
                boby += "</div>";
                model.Body = boby;
                var result = MailHelper.SendMailInfor(model);
            }
            catch (Exception ex)
            {
                NSLog.Logger.Error("SendMail_Home_RequestSendInforFriend: ", ex);
            }
            return RedirectToAction("Index");
        }

        public ActionResult LoadDataPagging(int pageIndex)
        {
            ProductViewModels model = new ProductViewModels();
            try
            {
                //Product
                model.ListProduct = _fac.GetList().OrderByDescending(x => x.CreatedDate).ToList();
                var dataImage = _fac.GetListImage();
                if (model.ListProduct != null && model.ListProduct.Any())
                {
                    model.ListProduct.ForEach(x =>
                    {
                        x.Alias = CommonHelper.RemoveUnicode(x.ProductName.Trim().Replace(" ", "-")).ToLower();
                        var _Image = dataImage.FirstOrDefault(z => z.ProductId.Equals(x.Id));
                        if (_Image != null)
                        {
                            x.ImageURL = _Image.ImageURL;
                            if (!string.IsNullOrEmpty(x.ImageURL))
                            {
                                x.ImageURL = Commons._PublicImages + "Products/" + x.ImageURL;
                            }
                            else
                            {
                                x.ImageURL = "";
                            }
                        }
                    });
                    model.ListProduct = model.ListProduct.Skip((pageIndex - 1) * PageSize).Take(PageSize).ToList();
                }
            }
            catch (Exception ex)
            {
                NSLog.Logger.Error("LoadDataPagging :", ex);
            }
            return PartialView("_ListItem", model);
        }
    }
}