using CMS_DTO.CMSCustomerInfor;
using CMS_Shared.CMSCustomerInfor;
using CMS_Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_Web.Controllers
{
    public class ContactController : HQController
    {        
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ContactInfor(CMS_CustomerInforModels model)
        {
            CMSCustomerInforFactory _facCusInfor = new CMSCustomerInforFactory();
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
                boby += "<h3>Cám ơn bạn đã quan tâm tới cửa hàng chúng tôi.</h3>";
                boby += "<p><b>Preferredt Date:</b> <i>" + DateTime.Now.ToString("dd/MM/yyyy hh:mm") + "</i></p>";
                boby += "<p><b>Title:</b> <i>" + model.Subject + "</i></p>";
                boby += "<p><b>Content tin nhắn:</b> <i>" + model.Message + "</i></p>";
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
    }
}