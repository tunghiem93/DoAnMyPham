using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_Web.Controllers
{
    public class QuotationController : HQController
    {
        // GET: Quotation
        public ActionResult Index()
        {
            return View();
        }
    }
}