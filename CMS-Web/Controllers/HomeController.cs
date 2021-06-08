using CMS_DTO.CMSProduct;
using CMS_DTO.CMSSession;
using CMS_Shared;
using CMS_DTO.CMSSendMail;
using CMS_Shared.CMSCategories;
using CMS_Shared.CMSCompanies;
using CMS_Shared.CMSNews;
using CMS_Shared.CMSProducts;
using CMS_Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS_Shared.CMSCustomerInfor;
using CMS_DTO.CMSCustomerInfor;
using CMS_Shared.CMSLocations;
using CMS_Shared.CMSBrands;
using CMS_Shared.CMSbanners;

namespace CMS_Web.Controllers
{
    public class HomeController : HQController
    {
        private readonly CMSProductFactory _fac;
        private readonly CMSNewsFactory _facN;
        private readonly CMSCategoriesFactory _facCate;
        private readonly CMSBrandsFactory _facBrand;
        private readonly CMSLocationFactory _facLoca;
        private readonly CMSCustomerInforFactory _facCusInfor;
        private readonly CMSBannersFactory _facBan;
        //private int PageSize = 8;
        public HomeController()
        {
            _fac = new CMSProductFactory();
            _facN = new CMSNewsFactory();
            _facCate = new CMSCategoriesFactory();
            _facBrand = new CMSBrandsFactory();
            _facLoca = new CMSLocationFactory();
            _facCusInfor = new CMSCustomerInforFactory();
            _facBan = new CMSBannersFactory();
            //ViewBag.LstCate = GetListCategory();
        }
        // GET: Clients/Home
        public ActionResult Index(int page = 1)
        {
            try
            {
                ProductViewModels model = new ProductViewModels();
                model.ListBanner = _facBan.GetList().Skip(0).Take(2).ToList();
                if (model.ListBanner != null && model.ListBanner.Any())
                {
                    model.ListBanner.ForEach(x =>
                    {
                        x.ImageURL = Commons._PublicImages + "Banners/" + x.ImageURL;
                    });                    
                }
                var tempProduct = _fac.GetList().OrderByDescending(x => x.CreatedDate);
                model.TotalProduct = tempProduct.Count();
                model.TotalPage = (int)Math.Ceiling(model.TotalProduct / (double)24);
                model.CurrentPage = page;
                model.ListProduct = _fac.GetList().Skip((page - 1) * 24).Take(24).ToList();
                var info = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");
                if (model.ListProduct != null && model.ListProduct.Any())
                {
                    model.ListProduct.ForEach(x =>
                    {
                        x.ImageURL = Commons._PublicImages + "Products/" + x.ImageURL;
                        x.sPrice = String.Format(info, "{0:C0}", x.ProductPrice);
                    });

                    //var TotalProduct = model.ListProduct.Count;
                    //if (TotalProduct % PageSize == 0)
                    //    model.TotalPage = TotalProduct / PageSize;
                    //else
                    //    model.TotalPage = Convert.ToInt32(TotalProduct / PageSize) + 1;

                    //model.ListProduct = model.ListProduct.Skip(0).Take(PageSize).ToList();
                    model.ListProductTopSales = model.ListProduct.Skip(0).Take(8).ToList();
                }
                model.ListGroupCate = _facCate.GetList().Where(w=>string.IsNullOrEmpty(w.ParentId)).OrderByDescending(x => x.CreatedDate).ToList();
                if (model.ListGroupCate != null && model.ListGroupCate.Any())
                {
                    model.ListGroupCate.ForEach(x =>
                    {
                        x.ImageURL = Commons.HostImage + "Categories/" + x.ImageURL;
                    });
                }
                model.ListNews = _facN.GetList().OrderBy(x => x.CreatedDate).Skip(0).Take(12).ToList();
                if (model.ListNews != null && model.ListNews.Any())
                {
                    model.ListNews.ForEach(x =>
                    {
                        x.ImageURL = Commons.HostImage + "News/" + x.ImageURL;
                        x.ImageURLAuthor = Commons.HostImage + "Author/" + x.ImageURLAuthor;
                    });
                }
                return View(model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "NotFound");
            }
        }
        
        public ActionResult NewsLetter()
        {
            var name = Request.QueryString["name"];
            var email = Request.QueryString["email"];
            var model = new CMS_CustomerInforModels();
            model.Subject = "Nhận thông tin email Customer đăng ký từ Home!";
            model.Name = name;
            model.Email = email;
            model.ReceiveType = (int)Commons.ETypeUserRequest.RequestNewsLetter;
            //Save db
            string msg = "";
            var _temp = _facCusInfor.CreateOrUpdate(model, ref msg);
            if (!_temp)
            {
                NSLog.Logger.Info("SendMail_Home_Subcriber: ", msg);
            }
            //Send Mail
            string boby = string.Empty;
            try
            {
                boby += "<div class='clearfix'>";
                boby += "<h3>Cám ơn bạn đã quan tâm tới Product của chúng tôi.</h3>";
                boby += "<p><b>Ngày đặt:</b> <i>" + DateTime.Now.ToString("dd/MM/yyyy") + "</i></p>";
                boby += "<p><b>Last name:</b> <i>" + name + "</i></p>";
                boby += "<p><b>Email:</b> <i>" + email + "</i></p>";
                boby += "<p><b>Note:</b> <i>Bạn muốn tìm hiểu về thông tin gì từ chúng tôi?</i></p>";
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

        public ActionResult Subcriber()
        {
            var email = Request.QueryString["email"];
            var model = new CMS_CustomerInforModels();
            model.Subject = "Nhận thông tin từ Ford";
            model.Email = email;
            //Save db
            string msg = "";
            var _temp = _facCusInfor.CreateOrUpdate(model, ref msg);
            if (!_temp)
            {
                NSLog.Logger.Info("SendMail_Home_Subcriber: ", msg);
            }
            string boby = string.Empty;
            try
            {
                boby += "<div class='clearfix'>";
                boby += "<h3>Cám ơn bạn đã quan tâm tới Product của chúng tôi.</h3>";
                boby += "<p><b>Ngày đặt:</b> <i>" + DateTime.Now.ToString("dd/MM/yyyy") + "</i></p>";
                boby += "<p><b>Email:</b> <i>" + email + "</i></p>";
                boby += "<p><b>Note:</b> <i>Bạn muốn tìm hiểu về thông tin gì từ chúng tôi?</i></p>";
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
        
        public ActionResult SearchKey(string Key = "")
        {
            var model = new ProductViewModels();

            try
            {
                model.ListProduct = _fac.GetList().Where(x => CommonHelper.RemoveUnicode(x.ProductName.ToLower()).Contains(CommonHelper.RemoveUnicode(Key.ToLower()))).OrderByDescending(x => x.CreatedDate).ToList();

                if (model.ListProduct != null && model.ListProduct.Any())
                {
                    model.ListProduct.ForEach(x =>
                    {
                        if (!string.IsNullOrEmpty(x.ImageURL))
                            x.ImageURL = Commons.HostImage + "Products/" + x.ImageURL;
                    });
                    model.ListCate = _facCate.GetList().OrderByDescending(x => x.CreatedDate).Skip(0).Take(15).ToList();
                    //Category
                    model.ListBrand = _facBrand.GetList().OrderByDescending(x => x.CreatedDate).Skip(0).Take(15).ToList();
                    //Location
                    model.ListLocation = _facLoca.GetList().OrderByDescending(x => x.CreatedDate).Skip(0).Take(15).ToList();
                }
                else
                {
                    return RedirectToAction("Index", "NotFound");
                }
                return View(model);
            }
            catch
            {
                return RedirectToAction("Index", "NotFound");
            }

        }


        public ActionResult Detail(string q)
        {

            ProductDetailViewModels model = new ProductDetailViewModels();
            try
            {
                var dataDetail = _fac.GetList().FirstOrDefault(o=>o.Alias.Equals(q));
                if (!string.IsNullOrEmpty(q) || dataDetail != null)
                {
                    if (!string.IsNullOrEmpty(dataDetail.ImageURL))
                        dataDetail.ImageURL = Commons.HostImage + "Products/" + dataDetail.ImageURL;
                    dataDetail.ListImg = dataDetail.ListImg.Skip(0).Take(5).ToList();
                    if (dataDetail.ListImg != null)
                    {
                        dataDetail.ListImg.ForEach(x =>
                        {
                            if (x != null)
                            {
                                if (!string.IsNullOrEmpty(x.ImageURL))
                                    x.ImageURL = Commons.HostImage + "Products/" + x.ImageURL;
                            }
                        });
                    }

                    var oldData = _fac.GetList().Where(x => !x.Alias.Equals(q) && x.CategoryId.Equals(dataDetail.CategoryId)).OrderBy(x => x.CreatedDate).Skip(0).Take(18).ToList();
                    if (oldData != null && oldData.Any())
                    {
                        var dataImage = _fac.GetListImage();
                        oldData.ForEach(x =>
                        {
                            if (!string.IsNullOrEmpty(x.ImageURL))
                                x.ImageURL = Commons.HostImage + "Products/" + x.ImageURL;
                            if (x.ListImg != null && x.ListImg.Any())
                            {
                                x.ListImg.ForEach(o =>
                                {
                                    if (!string.IsNullOrEmpty(o.ImageURL))
                                        o.ImageURL = Commons.HostImage + "Products/" + o.ImageURL;
                                });
                            }
                        });
                    }

                    model.ListProduct = oldData;
                    model.Product = dataDetail;
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

        //public PartialViewResult LoadPagging(int pageIndex, string Id = "", string Key = "", bool isOrther = false)
        //{
        //    ProductViewModels model = new ProductViewModels();
        //    var data = new List<CMS_ProductsModels>();
        //    if (!isOrther)
        //    {
        //        data = _fac.GetList().Where(z => (string.IsNullOrEmpty(Id) ? 1 == 1 : z.CategoryId.Equals(Id)) && (string.IsNullOrEmpty(Key) ? 1 == 1 : CommonHelper.RemoveUnicode(z.ProductName.ToLower()).Contains(CommonHelper.RemoveUnicode(Key.ToLower())))).ToList();
        //    }
        //    else
        //    {
        //        var ListCate = Session["Catelogies"] as List<CateSession>;
        //        if (ListCate != null && ListCate.Count < 9)
        //        {
        //            data = _fac.GetList().OrderBy(x => x.ProductName).ToList();
        //        }
        //        else
        //        {
        //            data = _fac.GetList().Where(x => ListCate.Select(z => z.Id).Contains(x.CategoryId)).OrderBy(x => x.ProductName).ToList();
        //        }
        //    }

        //    if (data != null && data.Any())
        //    {
        //        model.CateID = Id;
        //        model.Key = Key;
        //        model.IsOrther = isOrther;
        //        var dataImage = _fac.GetListImage();
        //        data.ForEach(x =>
        //        {
        //            var _Image = dataImage.FirstOrDefault(y => y.ProductId.Equals(x.Id));
        //            if (_Image != null)
        //            {
        //                if (!string.IsNullOrEmpty(_Image.ImageURL))
        //                    x.ImageURL = Commons.HostImage + "Products/" + _Image.ImageURL;
        //            }
        //        });
        //        // model.ListProduct = model.ListProduct.OrderByDescending(x => x.CreatedDate).ToList();

        //        model.TotalProduct = data.Count;
        //        model.ListProduct = data.OrderByDescending(x => x.CreatedDate).Skip(0).Take(12 * pageIndex).ToList();
        //        var page = 0;
        //        if (data.Count % 12 == 0)
        //            page = data.Count / 12;
        //        else
        //            page = Convert.ToInt16(data.Count / 12) + 1;

        //        if (page > pageIndex)
        //            model.TotalPage = pageIndex + 1;
        //        else
        //        {
        //            model.IsAddMore = true;
        //        }

        //    }
        //    return PartialView("_ListItem", model);
        //}

        //public ActionResult LoadDataPagging(int pageIndex)
        //{
        //    ProductViewModels model = new ProductViewModels();
        //    try
        //    {
        //        //Product
        //        model.ListProduct = _fac.GetList().OrderByDescending(x => x.CreatedDate).ToList();
        //        var dataImage = _fac.GetListImage();
        //        if (model.ListProduct != null && model.ListProduct.Any())
        //        {
        //            model.ListProduct.ForEach(x =>
        //            {
        //                var _Image = dataImage.FirstOrDefault(z => z.ProductId.Equals(x.Id));
        //                if (_Image != null)
        //                {
        //                    x.ImageURL = _Image.ImageURL;
        //                    if (!string.IsNullOrEmpty(x.ImageURL))
        //                    {
        //                        x.ImageURL = Commons._PublicImages + "Products/" + x.ImageURL;
        //                    }
        //                    else
        //                    {
        //                        x.ImageURL = "";
        //                    }
        //                }
        //            });
        //            model.ListProduct = model.ListProduct.Skip((pageIndex - 1) * PageSize).Take(PageSize).ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        NSLog.Logger.Error("LoadDataPagging :", ex);
        //    }
        //    return PartialView("_ListItem", model);
        //}

        //public ActionResult LoadDataFilterPrice(int type)
        //{
        //    ProductViewModels model = new ProductViewModels();
        //    try
        //    {
        //        //Product
        //        model.ListProduct = _fac.GetList().ToList();
        //        var dataImage = _fac.GetListImage();
        //        if (model.ListProduct != null && model.ListProduct.Any())
        //        {
        //            model.ListProduct.ForEach(x =>
        //            {
        //                x.Alias = CommonHelper.RemoveUnicode(x.ProductName.Trim().Replace(" ", "-")).ToLower();
        //                var _Image = dataImage.FirstOrDefault(z => z.ProductId.Equals(x.Id));
        //                if (_Image != null)
        //                {
        //                    x.ImageURL = _Image.ImageURL;
        //                    if (!string.IsNullOrEmpty(x.ImageURL))
        //                    {
        //                        x.ImageURL = Commons._PublicImages + "Products/" + x.ImageURL;
        //                    }
        //                    else
        //                    {
        //                        x.ImageURL = "";
        //                    }
        //                }
        //            });
        //            if (type == (byte)Commons.ERangeType.Hightest)
        //            {
        //                model.ListProduct = model.ListProduct.OrderByDescending(o => o.ProductPrice).Skip(0).Take(PageSize).ToList();                        
        //            }
        //            else
        //            {
        //                model.ListProduct = model.ListProduct.OrderBy(o => o.ProductPrice).Skip(0).Take(PageSize).ToList();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        NSLog.Logger.Error("FilterListProduct :", ex);
        //    }
        //    return PartialView("_ListItem", model);
        //}
    }
}