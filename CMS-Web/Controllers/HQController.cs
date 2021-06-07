using CMS_DTO.CMSSession;
using CMS_Shared;
using CMS_Shared.CMSBrands;
using CMS_Shared.CMSCategories;
using CMS_Shared.CMSLocations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace CMS_Web.Controllers
{
    public class HQController : Controller
    {
        public HQController()
        {
            var _Path = HostingEnvironment.MapPath("~/Uploads/Silder/");
            var list = Directory.GetFiles(_Path).Select(x => Path.GetFileName(x)).ToList();
            var ListSlider = new List<SliderSession>();
            if (list != null && list.Count > 0)
            {
                for (var i = 0; i < list.Count; i++)
                {
                    ListSlider.Add(new SliderSession
                    {
                        ImageUrl = "~/Uploads/Silder/" +  list[i]
                    });
                }
            }
            System.Web.HttpContext.Current.Session["SliderSession"] = ListSlider;
            var _factory = new CMSCategoriesFactory();
            var ListCate = _factory.GetList().Where(x => x.Type == 1).ToList();
            if (ListCate == null)
            {
                ListCate = new List<CMS_DTO.CMSCategories.CMSCategoriesModels>();
            }
            ListCate.ForEach(x =>
            {
                x.ImageURL = Commons._PublicImages + "Categories/" + x.ImageURL;
            });
            System.Web.HttpContext.Current.Session["ListCateSession"] = ListCate;
            //ViewBag.LstAnswer = GetListAnswerSelectItem();
            //ViewBag.Lstlocation = GetListLocationSelectItem();
            //ViewBag.LstCategory = GetListCategorySelectItem();
            //ViewBag.LstBrand = GetListBrandSelectItem();
            //ViewBag.LstStyle = GetListStyle();
            //ViewBag.LstCate = GetListCategory();
            ViewBag.LstOrder = null;
            var _lstOrder = System.Web.HttpContext.Current.Request["cms-order"];
            if (_lstOrder != null)
            {
                var strOrder = HttpUtility.UrlDecode(_lstOrder);
                var ListOrder = JsonConvert.DeserializeObject<List<OrderCookie>>(strOrder, new IsoDateTimeConverter());
                if (ListOrder != null)
                {
                    ViewBag.LstOrder = ListOrder;
                }
            }
        }

        public List<SelectListItem> GetListRangeSelectItem()
        {
            var _lstRange = new List<SelectListItem>() {
                new SelectListItem() { Text = "Price thấp nhất", Value = Commons.ERangeType.Leatest.ToString("d") },
                new SelectListItem() { Text = "Price cao nhất", Value = Commons.ERangeType.Hightest.ToString("d") },
            };
            return _lstRange;
        }

        public List<OrderCookie> GetListOrderCookie()
        {
            if (Request.Cookies["cms-order"] != null)
            {
                var _Orders = Request.Cookies["cms-order"].Value;
                var strOrder = Server.UrlDecode(_Orders);
                var ListOrder = JsonConvert.DeserializeObject<List<OrderCookie>>(strOrder, new IsoDateTimeConverter());
                return ListOrder;
            }
            return null;
        }

        public List<SelectListItem> GetListAnswerSelectItem()
        {
            var _lstRange = new List<SelectListItem>() {
                new SelectListItem() { Text = "Nick name của là gì?", Value = Commons.EAnswer.Answer1.ToString("d") },
                new SelectListItem() { Text = "Last name con vật ưu thích của bạn là gì?", Value = Commons.EAnswer.Answer2.ToString("d") },
                new SelectListItem() { Text = "Trường cấp 1 bạn Last name là gì?", Value = Commons.EAnswer.Answer3.ToString("d") },
                new SelectListItem() { Text = "Nơi sinh bạn ở đâu?", Value = Commons.EAnswer.Answer4.ToString("d") },
                new SelectListItem() { Text = "Người bạn yêu thích nhất là ai?", Value = Commons.EAnswer.Answer5.ToString("d") },
            };
            return _lstRange;
        }

        public List<SelectListItem> GetListCategorySelectItem()
        {
            var _factory = new CMSCategoriesFactory();
            var data = _factory.GetList().Select(x => new SelectListItem
            {
                Value = x.Id,
                Text = x.CategoryName,
            }).ToList();
            return data;
        }

        public List<SelectListItem> GetListBrandSelectItem()
        {
            var _factory = new CMSBrandsFactory();
            var data = _factory.GetList().Select(x => new SelectListItem
            {
                Value = x.Id,
                Text = x.BrandName,
            }).ToList();
            return data;
        }

        public List<SelectListItem> GetListLocationSelectItem()
        {
            var _factory = new CMSLocationFactory();
            var data = _factory.GetList().Select(x => new SelectListItem
            {
                Value = x.Id,
                Text = x.Name,
            }).ToList();
            return data;
        }

        //public List<SelectListItem> GetListStyle()
        //{
        //    var _factory = new CMSStyleFactory();
        //    var data = _factory.GetList().Select(x => new SelectListItem
        //    {
        //        Value = x.Id,
        //        Text = x.Name,
        //    }).ToList();
        //    return data;
        //}

        public List<CMS_DTO.CMSCategories.CMSCategoriesModels> GetListCategory()
        {
            var _factory = new CMSCategoriesFactory();
            var data = _factory.GetList();
            if (data == null)
            {
                data = new List<CMS_DTO.CMSCategories.CMSCategoriesModels>();
            }
            return data;
        }
    }
}