using CMS_DTO.CMSProduct;
using CMS_Shared;
using CMS_Shared.CMSBrands;
using CMS_Shared.CMSCategories;
using CMS_Shared.CMSLocations;
using CMS_Shared.CMSProducts;
using CMS_Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_Web.Controllers
{
    public class BrandController : HQController
    {
        private CMSProductFactory _fac;
        private CMSCategoriesFactory _facCate;
        private CMSBrandsFactory _facBrand;
        private readonly CMSLocationFactory _facLoca;
        private int PageSize = 8;
        public BrandController()
        {
            _fac = new CMSProductFactory();
            _facCate = new CMSCategoriesFactory();
            _facBrand = new CMSBrandsFactory();
            _facLoca = new CMSLocationFactory();
            //ViewBag.Range = GetListRangeSelectItem();
        }

        // GET: Collection
        public ActionResult Index()
        {
            var _alias = !string.IsNullOrEmpty(Request.QueryString["q"]) ? Request.QueryString["q"] : "";
            ProductViewModels model = new ProductViewModels();
            try
            {
                if (_alias.Length > 1)
                {
                    model.ListProduct = _fac.GetListProductBrand(_alias).OrderByDescending(x => x.CreatedDate).ToList();
                    if (model.ListProduct != null && model.ListProduct.Any())
                    {
                        model.ListProduct.ForEach(x =>
                        {
                            x.ImageURL = Commons.HostImage + "Products/" + x.ImageURL;
                            if (x.ListImg != null)
                            {
                                x.ListImg.ForEach(o => {
                                    if (!string.IsNullOrEmpty(o.ImageURL))
                                    {
                                        o.ImageURL = Commons.HostImage + "Products/" + o.ImageURL;
                                    }
                                    o.IsDelete = false;
                                });
                            }
                        });
                    }
                }
                //Category
                model.ListCate = _facCate.GetList().OrderByDescending(x => x.CreatedDate).Skip(0).Take(10).ToList();
                //Category
                model.ListBrand = _facBrand.GetList().OrderByDescending(x => x.CreatedDate).Skip(0).Take(7).ToList();
                //Location
                model.ListLocation = _facLoca.GetList().OrderByDescending(x => x.CreatedDate).Skip(0).Take(7).ToList();
                //Styles
                //model.ListStyles = _facSt.GetList().OrderByDescending(x => x.CreatedDate).Skip(0).Take(7).ToList();
                //Product
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
            try
            {
                ProductViewModels model = new ProductViewModels();
                model.ListCate = _facCate.GetList().OrderByDescending(x => x.CreatedDate).Skip(0).Take(10).ToList();
                if (model.ListCate != null && model.ListCate.Any())
                {
                    model.ListCate.ForEach(o =>
                    {
                        o.Alias = CommonHelper.RemoveUnicode(o.CategoryName.Trim().Replace(" ", "-")).ToLower();
                    });
                }
                //Category
                model.ListBrand = _facBrand.GetList().OrderByDescending(x => x.CreatedDate).Skip(0).Take(5).ToList();
                if (model.ListBrand != null && model.ListBrand.Any())
                {
                    model.ListBrand.ForEach(o =>
                    {
                        o.Alias = CommonHelper.RemoveUnicode(o.BrandName.Trim().Replace(" ", "-")).ToLower();
                    });
                }
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
                    q = q.Trim().Replace("-", " ");
                    model.ListProductTopSales = model.ListProduct.Skip(0).Take(5).ToList();
                    var TotalProduct = model.ListProduct.Count(o => CommonHelper.RemoveUnicode(o.BrandName.Trim().Replace("-", " ")).ToLower().Equals(q.ToLower()));
                    if (TotalProduct % PageSize == 0)
                        model.TotalPage = TotalProduct / PageSize;
                    else
                        model.TotalPage = Convert.ToInt32(TotalProduct / PageSize) + 1;
                    model.ListProduct = model.ListProduct.Where(o => CommonHelper.RemoveUnicode(o.BrandName.Trim().Replace("-", " ")).ToLower().Equals(q.ToLower())).Skip(0).Take(PageSize).ToList();

                }
                if (!string.IsNullOrEmpty(q))
                {
                    var dataDetail = _facBrand.GetList().Where(o => CommonHelper.RemoveUnicode(o.BrandName.Trim().Replace("-", " ")).ToLower().Equals(q.ToLower())).FirstOrDefault();
                    if (dataDetail != null)
                    {
                        if (dataDetail.ImageURL != null)
                        {
                            dataDetail.ImageURL = Commons.HostImage + "Brands/" + dataDetail.ImageURL;
                        }
                        model.BrandModel = dataDetail;
                        return View(model);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                //NSLog.Logger.Error("GetDetail: ", ex);
                return new HttpStatusCodeResult(400, ex.Message);
            }
        }

        public ActionResult LoadDataPagging(int pageIndex, string cateId)
        {
            ProductViewModels model = new ProductViewModels();
            try
            {
                //Product
                model.ListProduct = _fac.GetList().Where(o => o.CategoryId.Equals(cateId)).OrderByDescending(x => x.CreatedDate).ToList();
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