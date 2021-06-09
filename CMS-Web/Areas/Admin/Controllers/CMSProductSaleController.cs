using CMS_DTO.CMSImage;
using CMS_DTO.CMSProduct;
using CMS_Shared;
using CMS_Shared.CMSProducts;
using CMS_Shared.Utilities;
using CMS_Web.Web.App_Start;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CMS_Web.Areas.Admin.Controllers
{
    [NuAuth]
    public class CMSProductSaleController : HQController
    {
        private CMSProductFactory _factory;
        public CMSProductSaleController()
        {
            _factory = new CMSProductFactory();
        }
        // GET: Admin/CMSProduct
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadGrid()
        {
            var model = _factory.GetList().Where(w => w.QuantitySale > 0).OrderByDescending(o => o.QuantitySale).ToList();
            model.ForEach(x =>
            {
                x.sStatus = x.IsActive ? "Active" : "Not activated";
            });
            return PartialView("_ListData", model);
        }    
    }
}