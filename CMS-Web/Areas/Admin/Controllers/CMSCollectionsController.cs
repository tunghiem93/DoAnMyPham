using CMS_DTO.CMSCollection;
using CMS_Shared;
using CMS_Shared.CMSCollections;
using CMS_Shared.Utilities;
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
    public class CMSCollectionsController : HQController
    {
        private CMSCollectionsFactory _factory;
        public CMSCollectionsController()
        {
            _factory = new CMSCollectionsFactory();
            ViewBag.Link = getListLink();
        }
        // GET: Admin/CMSCollections
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadGrid()
        {
            var model = _factory.GetList();
            model.ForEach(x =>
            {
                x.sStatus = x.IsActive ? "Active" : "Not activated";
                if (!string.IsNullOrEmpty(x.ImageURL))
                    x.ImageURL = Commons.HostImage + "Collections/" + x.ImageURL;
            });
            return PartialView("_ListData", model);
        }

        public ActionResult Create()
        {
            CMSCollectionModels model = new CMSCollectionModels();
            return PartialView("_Create", model);
        }

        public CMSCollectionModels GetDetail(string Id)
        {
            var model = _factory.GetDetail(Id);
            if (model != null)
            {
                if (!string.IsNullOrEmpty(model.ImageURL))
                    model.ImageURL = Commons.HostImage + "Collections/" + model.ImageURL;
                if (model.ListImg != null && model.ListImg.Count > 0)
                {                    
                    model.ListImg.ForEach(o => {
                        if (!string.IsNullOrEmpty(o.ImageURL))
                        {
                            o.ImageURL = Commons.HostImage + "Collections/" + o.ImageURL;
                        }
                        o.IsDelete = false;
                    });
                }
            }
            return model;
        }

        [HttpPost]
        public ActionResult Create(CMSCollectionModels model)
        {
            try
            {
                byte[] photoByte = null;

                if (!ModelState.IsValid)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return PartialView("_Create", model);
                }

                /**** Main Image of product ***/
                if (model.PictureUpload != null && model.PictureUpload.ContentLength > 0)
                {
                    Byte[] mainByte = new Byte[model.PictureUpload.ContentLength];
                    model.PictureUpload.InputStream.Read(mainByte, 0, model.PictureUpload.ContentLength);
                    model.PictureByte = mainByte;
                    model.ImageURL = Guid.NewGuid() + Path.GetExtension(model.PictureUpload.FileName);
                    model.PictureUpload = null;
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
                var Id = "";
                var msg = "";
                var result = _factory.CreateOrUpdate(model, ref Id, ref msg);
                if (result)
                {
                    /*** main product ***/
                    if (!string.IsNullOrEmpty(model.ImageURL) && model.PictureByte != null)
                    {
                        var path = Server.MapPath("~/Uploads/Collections/" + model.ImageURL);
                        MemoryStream ms = new MemoryStream(model.PictureByte, 0, model.PictureByte.Length);
                        ms.Write(model.PictureByte, 0, model.PictureByte.Length);
                        System.Drawing.Image imageTmp = System.Drawing.Image.FromStream(ms, true);
                        ImageHelper.Me.SaveCroppedImage(imageTmp, path, model.ImageURL, ref photoByte, 600, 600, 400);// widht: 600, height: 400
                    }

                    foreach (var item in ListImage)
                    {
                        if (!string.IsNullOrEmpty(item.ImageURL) && item.PictureByte != null)
                        {
                            var path = Server.MapPath("~/Uploads/Collections/" + item.ImageURL);
                            MemoryStream ms = new MemoryStream(lstImgByte[item.OffSet], 0, lstImgByte[item.OffSet].Length);
                            ms.Write(lstImgByte[item.OffSet], 0, lstImgByte[item.OffSet].Length);
                            System.Drawing.Image imageTmp = System.Drawing.Image.FromStream(ms, true);
                            ImageHelper.Me.SaveCroppedImage(imageTmp, path, item.ImageURL, ref photoByte, 600, Commons.WidthCollection, Commons.HeightCollection);
                        }
                    }
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("CollectionName", msg);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_Create", model);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_Create", model);
            }
        }

        [HttpGet]
        public ActionResult Edit(string Id)
        {
            var model = GetDetail(Id);
            return PartialView("_Edit", model);
        }

        [HttpPost]
        public ActionResult Edit(CMSCollectionModels model)
        {
            var temp = model.ImageURL;
            try
            {
                List<string> ListNotChangeImg = new List<string>();
                byte[] photoByte = null;
                if (!ModelState.IsValid)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return PartialView("_Edit", model);
                }
                if (!string.IsNullOrEmpty(model.ImageURL))
                {
                    model.ImageURL = model.ImageURL.Replace(Commons._PublicImages, "").Replace("Collections/", "").Replace(Commons.Image600_400, "");
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

                Dictionary<int, byte[]> lstImgByte = new Dictionary<int, byte[]>();
                var ListImageDelete = model.ListImg.Where(o => o.IsDelete).ToList();
                if (ListImageDelete != null && ListImageDelete.Any())
                {
                    foreach (var item in ListImageDelete)
                    {
                        if (!string.IsNullOrEmpty(item.ImageURL) && item.PictureUpload == null)
                        {
                            item.ImageURL = item.ImageURL.Replace(Commons._PublicImages, "").Replace("Collections/", "").Replace(Commons.Image600_400, "");
                            ListNotChangeImg.Add(item.ImageURL);
                        }
                    }
                }
                var ListImage = model.ListImg.Where(o => !o.IsDelete).ToList();
                foreach (var item in ListImage)
                {
                    if (item.PictureUpload != null && item.PictureUpload.ContentLength > 0)
                    {
                        if (!string.IsNullOrEmpty(item.ImageURL))
                        {
                            item.ImageURL = item.ImageURL.Replace(Commons._PublicImages, "").Replace("Collections/", "").Replace(Commons.Image600_400, "");
                            ListNotChangeImg.Add(item.ImageURL);
                        }

                        Byte[] imgByte = new Byte[item.PictureUpload.ContentLength];
                        item.PictureUpload.InputStream.Read(imgByte, 0, item.PictureUpload.ContentLength);
                        item.PictureByte = imgByte;
                        item.ImageURL = Guid.NewGuid() + Path.GetExtension(item.PictureUpload.FileName);
                        item.PictureUpload = null;
                        lstImgByte.Add(item.OffSet, imgByte);
                        model.ListImageUrl.Add(item.ImageURL);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(item.ImageURL))
                        {
                            item.ImageURL = item.ImageURL.Replace(Commons._PublicImages, "").Replace("Collections/", "").Replace(Commons.Image600_400, "");
                            model.ListImageUrl.Add(item.ImageURL);
                        }
                    }
                }

                var Id = "";
                var msg = "";
                var result = _factory.CreateOrUpdate(model, ref Id, ref msg);
                if (result)
                {
                    if (ListNotChangeImg != null && ListNotChangeImg.Any())
                    {
                        //Delete image on forder
                        foreach (var item in ListNotChangeImg)
                        {
                            if (!item.Equals(Commons.Image600_400))
                            {
                                var filePath = Server.MapPath("~/Uploads/Collections/" + item);
                                if (System.IO.File.Exists(filePath))
                                {
                                    System.IO.File.Delete(filePath);
                                }
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(model.ImageURL) && model.PictureByte != null)
                    {
                        if (System.IO.File.Exists(Server.MapPath("~/Uploads/Collections/" + temp)))
                        {
                            ImageHelper.Me.TryDeleteImageUpdated(Server.MapPath("~/Uploads/Collections/" + temp));
                        }

                        var path = Server.MapPath("~/Uploads/Collections/" + model.ImageURL);
                        MemoryStream ms = new MemoryStream(photoByte, 0, photoByte.Length);
                        ms.Write(photoByte, 0, photoByte.Length);
                        System.Drawing.Image imageTmp = System.Drawing.Image.FromStream(ms, true);
                        ImageHelper.Me.SaveCroppedImage(imageTmp, path, model.ImageURL, ref photoByte, 600, 600, 400);// width : 600, height: 400
                    }

                    foreach (var item in ListImage)
                    {
                        if (!string.IsNullOrEmpty(item.ImageURL) && item.PictureByte != null)
                        {
                            var path = Server.MapPath("~/Uploads/Collections/" + item.ImageURL);
                            MemoryStream ms = new MemoryStream(lstImgByte[item.OffSet], 0, lstImgByte[item.OffSet].Length);
                            ms.Write(lstImgByte[item.OffSet], 0, lstImgByte[item.OffSet].Length);
                            System.Drawing.Image imageTmp = System.Drawing.Image.FromStream(ms, true);
                            ImageHelper.Me.SaveCroppedImage(imageTmp, path, item.ImageURL, ref photoByte, 400, Commons.WidthCollection, Commons.HeightCollection);
                        }
                    }
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("CollectionName", msg);
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
                model.ImageURL = Commons.HostImage + "Colections/" + model.ImageURL;
            return PartialView("_View", model);
        }

        [HttpGet]
        public ActionResult Delete(string Id)
        {
            var model = GetDetail(Id);
            return PartialView("_Delete", model);
        }

        [HttpPost]
        public ActionResult Delete(CMSCollectionModels model)
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
                            var tempImg = item.ImageURL.Replace(Commons._PublicImages, "").Replace("Colections/", "").Replace(Commons.Image600_400, "");
                            // delete image for folder
                            if (System.IO.File.Exists(Server.MapPath("~/Uploads/Colections/" + tempImg)))
                            {
                                ImageHelper.Me.TryDeleteImageUpdated(Server.MapPath("~/Uploads/Colections/" + tempImg));
                            }
                        }
                    }

                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("ColectionName", msg);
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
            ImageCollection model = new ImageCollection();
            model.OffSet = OffSet;
            return PartialView("_ImageItemCollection", model);
        }
    }
}