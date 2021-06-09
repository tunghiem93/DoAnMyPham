﻿using CMS_DTO.CMSPage;
using CMS_Shared;
using CMS_Shared.CMSPages;
using System;
using System.Collections.Generic;
using System.Linq;
using CMS_Shared.CMSProducts;
using System.Web;
using System.Web.Mvc;
using CMS_DTO.CMSProduct;
using CMS_Shared.CMSNews;

namespace CMS_Web.Controllers
{
    public class AdviceController : HQController
    {
        private readonly CMSProductFactory _fac;
        private CMSNewsFactory _facN;
        public AdviceController()
        {
            _fac = new CMSProductFactory();
            _facN = new CMSNewsFactory();
        }
        // GET: AboutUs
        public ActionResult Index()
        {
            int page = 1;
            ProductViewModels model = new ProductViewModels();
            model.ListNews = _facN.GetList().OrderByDescending(x => x.CreatedDate).Skip(0).Take(3).ToList();
            if (model.ListNews != null && model.ListNews.Any())
            {
                model.ListNews.ForEach(x =>
                {
                    x.ImageURL = Commons._PublicImages + "News/" + x.ImageURL;
                });
            }
            model.ListProduct = _fac.GetList().Skip((page - 1) * 24).Take(24).ToList();
            if (model.ListProduct != null && model.ListProduct.Any())
            {
                var info = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");
                model.ListProduct.ForEach(x =>
                {
                    x.ImageURL = Commons._PublicImages + "Products/" + x.ImageURL;
                    x.sPrice = String.Format(info, "{0:C0}", x.ProductPrice);
                });
            }
            return View(model);
        }
    }
}