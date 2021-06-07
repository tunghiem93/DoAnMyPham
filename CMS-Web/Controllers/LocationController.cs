using CMS_DTO.CMSCompany;
using CMS_DTO.CMSProduct;
using CMS_Shared;
using CMS_Shared.CMSBrands;
using CMS_Shared.CMSCategories;
using CMS_Shared.CMSCompanies;
using CMS_Shared.CMSLocations;
using CMS_Shared.CMSProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_Web.Controllers
{
    public class LocationController : HQController
    {
        private readonly CMSProductFactory _fac;
        private readonly CMSCategoriesFactory _facCate;
        private readonly CMSBrandsFactory _facBrand;
        private readonly CMSLocationFactory _facLoca;
        public LocationController()
        {
            _fac = new CMSProductFactory();
            _facCate = new CMSCategoriesFactory();
            _facBrand = new CMSBrandsFactory();
            _facLoca = new CMSLocationFactory();
            //ViewBag.Range = GetListRangeSelectItem();
        }

        // GET: Location
        public ActionResult Index()
        {
            var _alias = !string.IsNullOrEmpty(Request.QueryString["q"]) ? Request.QueryString["q"] : "";
            ProductViewModels model = new ProductViewModels();
            try
            {
                if (_alias.Length > 1)
                {
                    model.ListProduct = _fac.GetListProductLocation(_alias).OrderByDescending(x => x.CreatedDate).ToList();
                    if (model.ListProduct != null && model.ListProduct.Any())
                    {
                        model.ListProduct.ForEach(x =>
                        {
                            x.ImageURL = Commons.HostImage + "Products/" + x.ImageURL;                            
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
    }
}