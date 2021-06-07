using CMS_DTO.CMSPage;
using CMS_Shared.CMSPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_Web.Controllers
{
    public class PageController : HQController
    {
        private readonly CMSPagesFactory _fac;
        public PageController()
        {
            _fac = new CMSPagesFactory();
        }
        // GET: Page
        public ActionResult Index(string q)
        {
            CMS_PageModes model = new CMS_PageModes();
            model = _fac.GetList().FirstOrDefault();
            return View(model);
        }
    }
}