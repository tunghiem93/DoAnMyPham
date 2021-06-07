using CMS_DTO.CMSPage;
using CMS_Shared;
using CMS_Shared.CMSPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_Web.Controllers
{
    public class AboutController : HQController
    {

        private readonly CMSPagesFactory _fac;
        public AboutController()
        {
            _fac = new CMSPagesFactory();
        }
        // GET: AboutUs
        public ActionResult Index()
        {

            CMS_PageModes model = new CMS_PageModes();
            model = _fac.GetList().Where(w => w.Type == (int)Commons.ETypePage.GioiThieu).FirstOrDefault();
            return View(model);
        }
    }
}