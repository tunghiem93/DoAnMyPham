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
    public class AdviceController : HQController
    {

        public AdviceController()
        {
        }
        // GET: AboutUs
        public ActionResult Index()
        {
            return View();
        }
    }
}