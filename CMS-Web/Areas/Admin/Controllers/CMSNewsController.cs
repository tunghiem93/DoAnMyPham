using CMS_DTO.CMSNews;
using CMS_Shared;
using CMS_Shared.CMSNews;
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
    public class CMSNewsController : HQController
    {
        private CMSNewsFactory _factory;
        public CMSNewsController()
        {
            ViewBag.Category = getListCateNews();
            _factory = new CMSNewsFactory();
        }
        // GET: Admin/CMSCategories
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
                    x.ImageURL = Commons.HostImage + "News/" + x.ImageURL;
                if (!string.IsNullOrEmpty(x.ImageURLAuthor))
                    x.ImageURLAuthor = Commons.HostImage + "Author/" + x.ImageURLAuthor;
            });
            return PartialView("_ListData", model);
        }

        public ActionResult Create()
        {
            CMS_NewsModels model = new CMS_NewsModels();
            return PartialView("_Create", model);
        }

        public CMS_NewsModels GetDetail(string Id)
        {
            var model = _factory.GetDetail(Id);
            //if (model != null)
            //    model.ImageURL = Commons.HostImage + "News/" + model.ImageURL;
            return model;
        }

        [HttpPost]
        public ActionResult Create(CMS_NewsModels model)
        {
            try
            {
                byte[] photoByte = null;
                byte[] photoByteAuthor = null;
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
                    model.ImageURL = model.PictureUpload.FileName;
                    model.PictureUpload = null;
                    photoByte = imgByte;
                }

                //Author
                if (model.PictureUploadAuthor != null && model.PictureUploadAuthor.ContentLength > 0)
                {
                    Byte[] imgByteAuthor = new Byte[model.PictureUploadAuthor.ContentLength];
                    model.PictureUploadAuthor.InputStream.Read(imgByteAuthor, 0, model.PictureUploadAuthor.ContentLength);
                    model.PictureByteAuthor = imgByteAuthor;
                    model.ImageURLAuthor = model.PictureUploadAuthor.FileName + Path.GetExtension(model.PictureUploadAuthor.FileName);
                    model.PictureUploadAuthor = null;
                    photoByteAuthor = imgByteAuthor;
                }
                var msg = "";
                var result = _factory.CreateOrUpdate(model, ref msg);
                if (result)
                {
                    if (!string.IsNullOrEmpty(model.ImageURL) && model.PictureByte != null)
                    {
                        var path = Server.MapPath("~/Uploads/News/" + model.ImageURL);
                        MemoryStream ms = new MemoryStream(photoByte, 0, photoByte.Length);
                        ms.Write(photoByte, 0, photoByte.Length);
                        System.Drawing.Image imageTmp = System.Drawing.Image.FromStream(ms, true);

                        ImageHelper.Me.SaveCroppedImage(imageTmp, path, model.ImageURL, ref photoByte, 1200,Commons.WidthImageNews, Commons.HeightImageNews);
                    }
                    if (!string.IsNullOrEmpty(model.ImageURLAuthor) && model.PictureByteAuthor != null)
                    {
                        var path = Server.MapPath("~/Uploads/Author/" + model.ImageURLAuthor);
                        MemoryStream ms = new MemoryStream(photoByteAuthor, 0, photoByteAuthor.Length);
                        ms.Write(photoByteAuthor, 0, photoByteAuthor.Length);
                        System.Drawing.Image imageTmpAuthor = System.Drawing.Image.FromStream(ms, true);

                        ImageHelper.Me.SaveCroppedImage(imageTmpAuthor, path, model.ImageURLAuthor, ref photoByteAuthor, 100, Commons.WidthImageAuthor, Commons.HeightImageAuthor);
                    }
                    return RedirectToAction("Index");
                }
                    
                ModelState.AddModelError("Title", msg);
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
            if (!string.IsNullOrEmpty(model.ImageURL))
                model.ImageURL = Commons.HostImage + "News/" + model.ImageURL;
            if (!string.IsNullOrEmpty(model.ImageURLAuthor))
                model.ImageURLAuthor = Commons.HostImage + "Author/" + model.ImageURLAuthor;
            return PartialView("_Edit", model);
        }

        [HttpPost]
        public ActionResult Edit(CMS_NewsModels model)
        {
            var temp = model.ImageURL;
            var tempAuthor = model.ImageURLAuthor;
            try
            {
                byte[] photoByte = null;
                byte[] photoByteAuthor = null;
                if (!ModelState.IsValid)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return PartialView("_Edit", model);
                }
                if (!string.IsNullOrEmpty(model.ImageURL))
                {
                    model.ImageURL = model.ImageURL.Replace(Commons._PublicImages +"News/", "").Replace(Commons.Image1200_650, "");
                }

                if (model.PictureUpload != null && model.PictureUpload.ContentLength > 0)
                {
                    Byte[] imgByte = new Byte[model.PictureUpload.ContentLength];
                    model.PictureUpload.InputStream.Read(imgByte, 0, model.PictureUpload.ContentLength);
                    model.PictureByte = imgByte;
                    model.ImageURL = model.PictureUpload.FileName;
                    model.PictureUpload = null;
                    photoByte = imgByte;
                }

                //Author
                if (!string.IsNullOrEmpty(model.ImageURLAuthor))
                {
                    model.ImageURLAuthor = model.ImageURLAuthor.Replace(Commons._PublicImages + "Author/", "").Replace(Commons.Image100_100, "");
                }

                if (model.PictureUploadAuthor != null && model.PictureUploadAuthor.ContentLength > 0)
                {
                    Byte[] imgByteAuthor = new Byte[model.PictureUploadAuthor.ContentLength];
                    model.PictureUploadAuthor.InputStream.Read(imgByteAuthor, 0, model.PictureUploadAuthor.ContentLength);
                    model.PictureByteAuthor = imgByteAuthor;
                    model.ImageURLAuthor = model.PictureUploadAuthor.FileName;
                    model.PictureUploadAuthor = null;
                    photoByteAuthor = imgByteAuthor;
                }
                var msg = "";
                var result = _factory.CreateOrUpdate(model, ref msg);
                if (result)
                {
                    if (!string.IsNullOrEmpty(model.ImageURL) && model.PictureByte != null)
                    {
                        if (!string.IsNullOrEmpty(temp))
                        {
                            temp = temp.Replace(Commons._PublicImages + "News/", "").Replace(Commons.Image1200_650, "");
                            temp = "~/Uploads/News/" + temp;
                            if (System.IO.File.Exists(Server.MapPath(temp)))
                            {
                                ImageHelper.Me.TryDeleteImageUpdated(Server.MapPath(temp));
                            }
                        }
                        var path = Server.MapPath("~/Uploads/News/" + model.ImageURL);
                        MemoryStream ms = new MemoryStream(photoByte, 0, photoByte.Length);
                        ms.Write(photoByte, 0, photoByte.Length);
                        System.Drawing.Image imageTmp = System.Drawing.Image.FromStream(ms, true);
                        ImageHelper.Me.SaveCroppedImage(imageTmp, path, model.ImageURL, ref photoByte, 1200, Commons.WidthImageNews, Commons.HeightImageNews);
                    }
                    //Author
                    if (!string.IsNullOrEmpty(model.ImageURLAuthor) && model.PictureByteAuthor != null)
                    {
                        if (!string.IsNullOrEmpty(tempAuthor))
                        {
                            tempAuthor = tempAuthor.Replace(Commons._PublicImages + "Author/", "").Replace(Commons.Image100_100, "");
                            tempAuthor = "~/Uploads/Author/" + tempAuthor;
                            if (System.IO.File.Exists(Server.MapPath(tempAuthor)))
                            {
                                ImageHelper.Me.TryDeleteImageUpdated(Server.MapPath(tempAuthor));
                            }
                        }
                        var path = Server.MapPath("~/Uploads/Author/" + model.ImageURLAuthor);
                        MemoryStream ms = new MemoryStream(photoByteAuthor, 0, photoByteAuthor.Length);
                        ms.Write(photoByteAuthor, 0, photoByteAuthor.Length);
                        System.Drawing.Image imageTmpAuthor = System.Drawing.Image.FromStream(ms, true);
                        ImageHelper.Me.SaveCroppedImage(imageTmpAuthor, path, model.ImageURLAuthor, ref photoByteAuthor, 100, Commons.WidthImageAuthor, Commons.HeightImageAuthor);
                    }
                    return RedirectToAction("Index");
                }
                    
                ModelState.AddModelError("Title", msg);
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
                model.ImageURL = Commons.HostImage + "News/" + model.ImageURL;
            if (!string.IsNullOrEmpty(model.ImageURLAuthor))
                model.ImageURLAuthor = Commons.HostImage + "Author/" + model.ImageURLAuthor;
            return PartialView("_View", model);
        }

        [HttpGet]
        public ActionResult Delete(string Id)
        {
            var model = GetDetail(Id);
            return PartialView("_Delete", model);
        }

        [HttpPost]
        public ActionResult Delete(CMS_NewsModels model)
        {
            try
            {
                //if (!ModelState.IsValid)
                //{
                //    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                //    return PartialView("_Delete", model);
                //}
                var msg = "";
                var result = _factory.Delete(model.Id, ref msg);
                if (result)
                    return RedirectToAction("Index");
                ModelState.AddModelError("Title", msg);
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