using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_Web.Controllers
{
    public class NotFoundController : HQController
    {      
        public NotFoundController()
        {
            //ViewBag.Range = GetListRangeSelectItem();
        }
        // GET: Clients/NotFound
        public ActionResult Index()
        {            
            return View();
        }
    }
}