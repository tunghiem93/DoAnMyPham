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
    public class CMSCategoriesController : HQController
    {
        private CMSCategoriesFactory _factory;
        public CMSCategoriesController()
        {
            _factory = new CMSCategoriesFactory();
            ViewBag.Category = GetListCategorySelectItem();
            ViewBag.GroupCate = getListGroupCate();
        }
        // GET: Admin/CMSCategories
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadGrid()
        {
            var model = _factory.GetList().ToList();
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

                if (!string.IsNullOrEmpty(model.RawImageUrl))
                {
                    model.ListImageUrl.Add(model.RawImageUrl);
                }
                //========
                Dictionary<int, byte[]> lstImgByte = new Dictionary<int, byte[]>();
                var ListImage = model.ListImg.Where(x => !x.IsDelete).ToList();
                foreach (var item in ListImage)
                {
                    if (item.PictureUpload != null && item.PictureUpload.ContentLength > 0)
                    {
                        Byte[] imgByte = new Byte[item.PictureUpload.ContentLength];
                        item.PictureUpload.InputStream.Read(imgByte, 0, item.PictureUpload.ContentLength);
                        item.PictureByte = imgByte;
                        item.ImageURL = Guid.NewGuid() + Path.GetExtension(item.PictureUpload.FileName);
                        item.PictureUpload = null;
                        lstImgByte.Add(item.OffSet, imgByte);
                        model.ListImageUrl.Add(item.ImageURL);
                    }
                }
                model.Type = _factory.GetList().Where(w => w.Id == model.ParentId).Select(s => s.Type).FirstOrDefault();
                var Id = "";
                var msg = "";
                var result = _factory.CreateOrUpdate(model,ref Id,ref msg);
                if (result)
                {
                    foreach (var item in ListImage)
                    {
                        if (!string.IsNullOrEmpty(item.ImageURL) && item.PictureByte != null)
                        {
                            var path = Server.MapPath("~/Uploads/Categories/" + item.ImageURL);
                            MemoryStream ms = new MemoryStream(lstImgByte[item.OffSet], 0, lstImgByte[item.OffSet].Length);
                            ms.Write(lstImgByte[item.OffSet], 0, lstImgByte[item.OffSet].Length);
                            System.Drawing.Image imageTmp = System.Drawing.Image.FromStream(ms, true);

                            ImageHelper.Me.SaveCroppedImage(imageTmp, path, item.ImageURL, ref photoByte, 400, Commons.WidthCate, Commons.HeightCate);
                        }
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
                    model.ImageURL = model.ImageURL.Replace(Commons._PublicImages, "").Replace("Categories/", "").Replace(Commons.Image600_400, "");
                    temp = model.ImageURL;
                }

                Dictionary<int, byte[]> lstImgByte = new Dictionary<int, byte[]>();
                var ListImage = model.ListImg.Where(x => !x.IsDelete).ToList();
                foreach (var item in ListImage)
                {
                    if (item.PictureUpload != null && item.PictureUpload.ContentLength > 0)
                    {
                        Byte[] imgByte = new Byte[item.PictureUpload.ContentLength];
                        item.PictureUpload.InputStream.Read(imgByte, 0, item.PictureUpload.ContentLength);
                        item.PictureByte = imgByte;
                        item.ImageURL = Guid.NewGuid() + Path.GetExtension(item.PictureUpload.FileName);
                        item.PictureUpload = null;
                        lstImgByte.Add(item.OffSet, imgByte);
                        model.ListImageUrl.Add(item.ImageURL);
                    }else
                    {
                        var tempImg = item.ImageURL.Replace(Commons._PublicImages, "").Replace("Categories/", "").Replace(Commons.Image600_400, "");
                        model.ListImageUrl.Add(tempImg);
                    }
                }
                model.Type = _factory.GetList().Where(w => w.Id == model.ParentId).Select(s => s.Type).FirstOrDefault();
                var Id = "";
                var msg = "";
                var result = _factory.CreateOrUpdate(model, ref Id, ref msg);
                if (result)
                {
                    foreach (var item in ListImage)
                    {
                        if (!string.IsNullOrEmpty(item.ImageURL) && item.PictureByte != null)
                        {
                            var path = Server.MapPath("~/Uploads/Categories/" + item.ImageURL);
                            MemoryStream ms = new MemoryStream(lstImgByte[item.OffSet], 0, lstImgByte[item.OffSet].Length);
                            ms.Write(lstImgByte[item.OffSet], 0, lstImgByte[item.OffSet].Length);
                            System.Drawing.Image imageTmp = System.Drawing.Image.FromStream(ms, true);

                            ImageHelper.Me.SaveCroppedImage(imageTmp, path, item.ImageURL, ref photoByte, 400, Commons.WidthCate, Commons.HeightCate);
                        }
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
                            var tempImg = item.ImageURL.Replace(Commons._PublicImages, "").Replace("Categories/", "").Replace(Commons.Image600_400, "");
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