using CMS_DTO.CMSNews;
using CMS_Shared;
using CMS_Shared.CMSCategories;
using CMS_Shared.CMSNews;
using CMS_Shared.CMSProducts;
using CMS_Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_Web.Controllers
{
    public class NewsController : HQController
    {
        private CMSNewsFactory _fac;
        private CMSCategoriesFactory _facCate;
        private CMSProductFactory _facPro;
        public NewsController()
        {
            _fac = new CMSNewsFactory();
            _facCate = new CMSCategoriesFactory();
            _facPro = new CMSProductFactory();
        }
        // GET: Clients/News
        public ActionResult Index(int page = 1)
        {
            var model = new CMS_NewsViewModel();
            try
            {
                int totalPage, totalRecord;
                var data = _fac.GetListByType(0, out totalRecord, out totalPage, page);
                if (data == null)
                {
                    return RedirectToAction("Index", "NotFound");
                }
                else
                {
                    data.ForEach(x =>
                    {
                        if (!string.IsNullOrEmpty(x.ImageURL))
                        {
                            x.ImageURL = Commons._PublicImages + "News/" + x.ImageURL;
                        }
                    });
                }
                model.ListNewsNew = _fac.GetList().OrderByDescending(x => x.CreatedDate).Skip(0).Take(3).ToList();
                if (model.ListNewsNew != null && model.ListNewsNew.Any())
                {
                    model.ListNewsNew.ForEach(x =>
                    {
                        x.ImageURL = Commons._PublicImages + "News/" + x.ImageURL;
                    });
                }
                model.ListNews = data;
                model.TotalPage = totalPage;
                model.TotalRecord = totalRecord;
                model.CurrentPage = page;
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "NotFound");
            }
            return View(model);
        }
        
        public ActionResult Detail(string q)
        {
            var model = new CMS_NewsViewModel();
            try
            {
                var data = _fac.GetNewsByAlias(q);
                if (string.IsNullOrEmpty(q) || data == null)
                {
                    return RedirectToAction("Index", "NotFound");
                }
                else
                {
                    if (!string.IsNullOrEmpty(data.ImageURL))
                    {
                        data.ImageURL = Commons.HostImage + "News/" + data.ImageURL;
                    }

                    model.News = data;
                    model.ListNewsNew = _fac.GetList().OrderByDescending(x => x.CreatedDate).Skip(0).Take(3).ToList();
                    if (model.ListNewsNew != null && model.ListNewsNew.Any())
                    {
                        model.ListNewsNew.ForEach(x =>
                        {
                            x.ImageURL = Commons._PublicImages + "News/" + x.ImageURL;
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "NotFound");
            }
            return View(model);
        }

        public ActionResult Tin_Thi_Truong(int page = 1)
        {
            var model = new CMS_NewsViewModel();
            try
            {
                var type = Convert.ToInt16(Commons.ETypeNews.ThiTruong);
                int totalPage, totalRecord;
                var data = _fac.GetListByType(type, out totalRecord, out totalPage, page);
                if (data == null)
                {
                    return RedirectToAction("Index", "NotFound");
                } else
                {
                    data.ForEach(x =>
                    {
                        if (!string.IsNullOrEmpty(x.ImageURL))
                        {
                            x.ImageURL = Commons._PublicImages + "News/" + x.ImageURL;
                        }
                    });
                }
                model.ListNewsNew = _fac.GetList().OrderByDescending(x => x.CreatedDate).Skip(0).Take(3).ToList();
                if (model.ListNewsNew != null && model.ListNewsNew.Any())
                {
                    model.ListNewsNew.ForEach(x =>
                    {
                        x.ImageURL = Commons._PublicImages + "News/" + x.ImageURL;
                    });
                }
                model.ListNews = data;
                model.TotalPage = totalPage;
                model.TotalRecord = totalRecord;
                model.CurrentPage = page;
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "NotFound");
            }
            return View(model);
        }

        public ActionResult Tin_Khuyen_Mai(int page = 1)
        {
            var model = new CMS_NewsViewModel();
            try
            {
                var type = Convert.ToInt16(Commons.ETypeNews.KhuyenMai);
                int totalPage, totalRecord;
                var data = _fac.GetListByType(type, out totalRecord, out totalPage, page);
                if (data == null)
                {
                    return RedirectToAction("Index", "NotFound");
                }
                else
                {
                    data.ForEach(x =>
                    {
                        if (!string.IsNullOrEmpty(x.ImageURL))
                        {
                            x.ImageURL = Commons._PublicImages + "News/" + x.ImageURL;
                        }
                    });
                }
                model.ListNewsNew = _fac.GetList().OrderByDescending(x => x.CreatedDate).Skip(0).Take(3).ToList();
                if (model.ListNewsNew != null && model.ListNewsNew.Any())
                {
                    model.ListNewsNew.ForEach(x =>
                    {
                        x.ImageURL = Commons._PublicImages + "News/" + x.ImageURL;
                    });
                }
                model.ListNews = data;
                model.TotalPage = totalPage;
                model.TotalRecord = totalRecord;
                model.CurrentPage = page;
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "NotFound");
            }
            return View(model);
        }
    }
}