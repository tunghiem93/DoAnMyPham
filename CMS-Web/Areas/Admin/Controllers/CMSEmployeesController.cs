using CMS_DTO.CMSEmployee;
using CMS_Shared;
using CMS_Shared.CMSEmployees;
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
    public class CMSEmployeesController : HQController
    {
        private CMSEmployeeFactory _factory;
        public CMSEmployeesController()
        {
            _factory = new CMSEmployeeFactory();
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
                    x.ImageURL = Commons.HostImage + "Employees/" + x.ImageURL;
            });
            return PartialView("_ListData", model);
        }

        public ActionResult Create()
        {
            CMS_EmployeeModels model = new CMS_EmployeeModels();
            return PartialView("_Create", model);
        }

        public CMS_EmployeeModels GetDetail(string Id)
        {
            var data =  _factory.GetDetail(Id);
            if(data != null)
                data.Password = CommonHelper.Decrypt(data.Password);
            return data;
        }

        [HttpPost]
        public ActionResult Create(CMS_EmployeeModels model)
        {
            try
            {
                byte[] photoByte = null;
                if (!model.Password.Trim().ToLower().Equals(model.ConfirmPassword.ToString().Trim().ToLower()))
                {
                    ModelState.AddModelError("ConfirmPassword", "Vui lòng Confirm Password!");
                }
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

                var msg = "";
                model.Password = CommonHelper.Encrypt(model.Password);
                var result = _factory.CreateOrUpdate(model, ref msg);
                if (result)
                {
                    if (!string.IsNullOrEmpty(model.ImageURL) && model.PictureByte != null)
                    {
                        var path = Server.MapPath("~/Uploads/Employees/" + model.ImageURL);
                        MemoryStream ms = new MemoryStream(photoByte, 0, photoByte.Length);
                        ms.Write(photoByte, 0, photoByte.Length);
                        System.Drawing.Image imageTmp = System.Drawing.Image.FromStream(ms, true);
                        ImageHelper.Me.SaveCroppedImage(imageTmp, path, model.ImageURL, ref photoByte, 400, 600, 400);
                    }
                    return RedirectToAction("Index");
                }
                   
                ModelState.AddModelError("FirstName", msg);
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
                model.ImageURL = Commons.HostImage + "Employees/" + model.ImageURL;
            return PartialView("_Edit", model);
        }

        [HttpPost]
        public ActionResult Edit(CMS_EmployeeModels model)
        {
            var temp = model.ImageURL;
            try
            {
                byte[] photoByte = null;
                if (!model.Password.Trim().ToLower().Equals(model.ConfirmPassword.ToString().Trim().ToLower()))
                {
                    ModelState.AddModelError("ConfirmPassword", "Vui lòng Confirm Password!");
                }
                if (!ModelState.IsValid)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return PartialView("_Edit", model);
                }
                if (!string.IsNullOrEmpty(model.ImageURL))
                {
                    model.ImageURL = model.ImageURL.Replace(Commons._PublicImages, "").Replace("Employees/", "").Replace(Commons.Image300_300, "");
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

                var msg = "";
                model.Password = CommonHelper.Encrypt(model.Password);
                var result = _factory.CreateOrUpdate(model, ref msg);
                if (result)
                {
                    if (!string.IsNullOrEmpty(model.ImageURL) && model.PictureByte != null)
                    {                        
                        if (System.IO.File.Exists(Server.MapPath("~/Uploads/Employees/" + temp)))
                        {
                            ImageHelper.Me.TryDeleteImageUpdated(Server.MapPath("~/Uploads/Employees/" + temp));
                        }

                        var path = Server.MapPath("~/Uploads/Employees/" + model.ImageURL);
                        MemoryStream ms = new MemoryStream(photoByte, 0, photoByte.Length);
                        ms.Write(photoByte, 0, photoByte.Length);
                        System.Drawing.Image imageTmp = System.Drawing.Image.FromStream(ms, true);
                        ImageHelper.Me.SaveCroppedImage(imageTmp, path, model.ImageURL, ref photoByte, 400, 600, 400);
                    }
                    return RedirectToAction("Index");
                }
                    
                ModelState.AddModelError("FirstName", msg);
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
                model.ImageURL = Commons.HostImage + "Employees/" + model.ImageURL;
            return PartialView("_View", model);
        }

        [HttpGet]
        public ActionResult Delete(string Id)
        {
            var model = GetDetail(Id);
            return PartialView("_Delete", model);
        }

        [HttpPost]
        public ActionResult Delete(CMS_EmployeeModels model)
        {
            try
            {
                ModelState.Clear();
                if (!ModelState.IsValid)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return PartialView("_Delete", model);
                }
                var msg = "";
                var result = _factory.Delete(model.Id, ref msg);
                if (result)
                {
                    var tempImg = model.ImageURL.Replace(Commons._PublicImages, "").Replace("Employees/", "").Replace(Commons.Image300_300, "");
                    // delete image for folder
                    if (System.IO.File.Exists(Server.MapPath("~/Uploads/Employees/" + tempImg)))
                    {
                        ImageHelper.Me.TryDeleteImageUpdated(Server.MapPath("~/Uploads/Employees/" + tempImg));
                    }
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("FirstName", msg);
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