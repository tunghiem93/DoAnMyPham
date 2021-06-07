using CMS_DTO.CMSCategories;
using CMS_Shared;
using CMS_Shared.CMSCategories;
using CMS_Shared.Utilities;
using CMS_Web.Areas.Admin.Models.Categories;
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
    public class CMSListCategoriesController : HQController
    {
        private CMSCategoriesFactory _factory;
        public CMSListCategoriesController()
        {
            _factory = new CMSCategoriesFactory();
            ViewBag.Category = GetListCategorySelectItem();
        }
        // GET: Admin/CMSCategories
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadGrid()
        {
            var model = _factory.GetList().Where(w=>string.IsNullOrEmpty(w.ParentId)).ToList();
            model.ForEach(x =>
            {
                x.sStatus = x.IsActive ? "Active" : "Not activated";
                if (!string.IsNullOrEmpty(x.ImageURL))
                    x.ImageURL = Commons.HostImage + "Categories/" + x.ImageURL;
            });
            return PartialView("_ListData",model);
        }

        public ActionResult Create()
        {
            CMSCategoriesModels model = new CMSCategoriesModels();
            return PartialView("_Create", model);
        }

        public CMSCategoriesModels GetDetail(string Id)
        {
            var model = _factory.GetDetail(Id);
            if (model != null)
            {
                if (model.ListImg != null && model.ListImg.Count > 0)
                {
                    model.ListImg.ForEach(o => {
                        if (!string.IsNullOrEmpty(o.ImageURL))
                        {
                            o.ImageURL = Commons.HostImage + "Categories/" + o.ImageURL;
                        }
                        o.IsDelete = false;
                    });
                }
            }
            return model;
        }

        [HttpPost]
        public ActionResult Create(CMSCategoriesModels model)
        {
            try
            {
                byte[] photoByte = null;

                if (!ModelState.IsValid)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return PartialView("_Create", model);
                }
                if (model.PictureUpload != null && model.PictureUpload.ContentLength > 0)
                {
                    Byte[] imgByte = new Byte[model.PictureUpload.ContentLength];
                    model.PictureUpload.InputStream.Read(imgByte, 0, model.PictureUpload.ContentLength);
                    model.PictureByte = imgByte;
                    model.ImageURL = Guid.NewGuid() + Path.GetExtension(model.PictureUpload.FileName);
                    model.PictureUpload = null;
                    photoByte = imgByte;
                }
                var Id = "";
                var msg = "";
                var result = _factory.CreateOrUpdate(model,ref Id,ref msg);
                if (result)
                {
                    if (!string.IsNullOrEmpty(model.ImageURL) && model.PictureByte != null)
                    {
                        var path = Server.MapPath("~/Uploads/Categories/" + model.ImageURL);
                        MemoryStream ms = new MemoryStream(photoByte, 0, photoByte.Length);
                        ms.Write(photoByte, 0, photoByte.Length);
                        System.Drawing.Image imageTmp = System.Drawing.Image.FromStream(ms, true);

                        ImageHelper.Me.SaveCroppedImage(imageTmp, path, model.ImageURL, ref photoByte, Commons.WidthListCate, Commons.WidthListCate, Commons.HeightListCate);
                    }
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("CategoryCode", msg);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_Create", model);
            }
            catch(Exception ex) {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_Create", model);
            }
        }

        [HttpGet]
        public ActionResult Edit(string Id)
        {
            var model = GetDetail(Id);
            if (!string.IsNullOrEmpty(model.ImageURL))
                model.ImageURL = Commons.HostImage + "Categories/" + model.ImageURL;
            return PartialView("_Edit", model);
        }

        [HttpPost]
        public ActionResult Edit(CMSCategoriesModels model)
        {
            var temp = model.ImageURL;
            try
            {
                byte[] photoByte = null;
                if (!ModelState.IsValid)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return PartialView("_Edit", model);
                }
                if (!string.IsNullOrEmpty(model.ImageURL))
                {
                    model.ImageURL = model.ImageURL.Replace(Commons._PublicImages, "").Replace("Categories/", "").Replace(Commons.Image170_170, "");
                    temp = model.ImageURL;
                }

                if (model.PictureUpload != null && model.PictureUpload.ContentLength > 0)
                {
                    Byte[] imgByte = new Byte[model.PictureUpload.ContentLength];
                    model.PictureUpload.InputStream.Read(imgByte, 0, model.PictureUpload.ContentLength);
                    model.PictureByte = imgByte;
                    model.ImageURL = Guid.NewGuid() + Path.GetExtension(model.PictureUpload.FileName);
                    model.PictureUpload = null;
                    photoByte = imgByte;
                }
                var Id = "";
                var msg = "";
                var result = _factory.CreateOrUpdate(model, ref Id, ref msg);
                if (result)
                {
                    if (!string.IsNullOrEmpty(model.ImageURL) && model.PictureByte != null)
                    {
                        if (System.IO.File.Exists(Server.MapPath("~/Uploads/Categories/" + temp)))
                        {
                            ImageHelper.Me.TryDeleteImageUpdated(Server.MapPath("~/Uploads/Categories/" + temp));
                        }

                        var path = Server.MapPath("~/Uploads/Categories/" + model.ImageURL);
                        MemoryStream ms = new MemoryStream(photoByte, 0, photoByte.Length);
                        ms.Write(photoByte, 0, photoByte.Length);
                        System.Drawing.Image imageTmp = System.Drawing.Image.FromStream(ms, true);

                        ImageHelper.Me.SaveCroppedImage(imageTmp, path, model.ImageURL, ref photoByte, Commons.WidthListCate, Commons.WidthListCate, Commons.HeightListCate);
                    }
                    return RedirectToAction("Index");
                }                    
                ModelState.AddModelError("CategoryCode", msg);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_Edit", model);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_Edit", model);
            }
        }

        [HttpGet]
        public ActionResult View(string Id)
        {
            var model = GetDetail(Id);
            if (!string.IsNullOrEmpty(model.ImageURL))
                model.ImageURL = Commons.HostImage + "Categories/" + model.ImageURL;
            return PartialView("_View", model);
        }

        [HttpGet]
        public ActionResult Delete(string Id)
        {
            var model = GetDetail(Id);
            return PartialView("_Delete", model);
        }

        [HttpPost]
        public ActionResult Delete(CMSCategoriesModels model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return PartialView("_Delete", model);
                }
                var msg = "";
                var result = _factory.Delete(model.Id, ref msg);
                if (result)
                {
                    if (model.ListImg != null && model.ListImg.Any())
                    {
                        foreach (var item in model.ListImg)
                        {
                            var tempImg = item.ImageURL.Replace(Commons._PublicImages, "").Replace("Categories/", "").Replace(Commons.Image170_170, "");
                            // delete image for folder
                            if (System.IO.File.Exists(Server.MapPath("~/Uploads/Categories/" + tempImg)))
                            {
                                ImageHelper.Me.TryDeleteImageUpdated(Server.MapPath("~/Uploads/Categories/" + tempImg));
                            }
                        }
                    }

                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("CategoryName", msg);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_Delete", model);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_Delete", model);
            }
        }

        public PartialViewResult AddImageItem(int OffSet)
        {
            ImageCate model = new ImageCate();
            model.OffSet = OffSet;
            return PartialView("_ImageItemCate", model);
        }
    }
}