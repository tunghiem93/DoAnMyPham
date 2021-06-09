using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CMS_DTO.Promotions;
using CMS_Shared.Promotions;
using CMS_Web.Web.App_Start;
namespace CMS_Web.Areas.Admin.Controllers
{
    [NuAuth]
    public class CMSPromotionsController : Controller
    {
        private readonly PromotionsDAL promotionsDAL;
        public CMSPromotionsController()
        {
            this.promotionsDAL = new PromotionsDAL();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadGrid()
        {
            var models = promotionsDAL.GetPromotions();
            models.ForEach(x =>
            {
                x.sStatus = x.IsActive ? "Đã kích hoạt" : "Chưa kích hoạt";
            });
            return PartialView("_ListData", models);
        }

        public ActionResult Create()
        {
            PromotionDTO model = new PromotionDTO();
            return PartialView("_Create", model);
        }

        [HttpPost]
        public ActionResult Create(PromotionDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return PartialView("_Create", model);
                }
                var result = promotionsDAL.CreatePromotion(model);
                if (result)
                {
                    return RedirectToAction("Index");
                }
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_Create", model);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_Create", model);
            }
        }

        public PromotionDTO GetDetail(string Id)
        {
            PromotionDTO model = promotionsDAL.GetPromotions().FirstOrDefault(z => z.Id == Id);
            return model;
        }

        [HttpGet]
        public ActionResult Delete(string Id)
        {
            var model = GetDetail(Id);
            return PartialView("_Delete", model);
        }

        [HttpPost]
        public ActionResult Delete(PromotionDTO model)
        {
            try
            {
                ModelState.Clear();
                if (!ModelState.IsValid)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return PartialView("_Delete", model);
                }
                var result = promotionsDAL.Delete(model.Id);
                if (result)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("PromotionCode", "Không thể xoá thông tin khuyến mãi");
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_Delete", model);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_Delete", model);
            }
        }
    }
}