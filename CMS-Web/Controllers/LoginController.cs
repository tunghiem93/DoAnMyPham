using CMS_DTO.CMSCustomer;
using CMS_DTO.CMSSession;
using CMS_Shared;
using CMS_Shared.CMSCustomers;
using CMS_Shared.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CMS_Web.Controllers
{
    public class LoginController : Controller
    {
        private CMSCustomersFactory _factory = null;
        List<string> listPropertyReject = null;
        public void PropertyReject()
        {
            foreach (var item in listPropertyReject)
            {
                if (ModelState.ContainsKey(item))
                    ModelState[item].Errors.Clear();
            }
        }

        public LoginController()
        {
            _factory = new CMSCustomersFactory();
            listPropertyReject = new List<string>();
            listPropertyReject.Add("FirstName");
            listPropertyReject.Add("LastName");
            listPropertyReject.Add("OldPassword");
            listPropertyReject.Add("Address");
        }
        // GET: ClientSite/Login
        public ActionResult SignIn()
        {
            ClientLoginModel model = new ClientLoginModel();
            return View(model);
        }

        public ActionResult Logout()
        {
            try
            {
                HttpCookie cookie = new HttpCookie("UserClientCookie");
                HttpContext.Response.Cookies.Remove("UserClientCookie");
                HttpContext.Response.SetCookie(cookie);

                if (Session["UserClient"] == null)
                    return RedirectToAction("Index", "Home");

                FormsAuthentication.SignOut();
                Session.Remove("UserClient");
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                NSLog.Logger.Error("Logout ", ex);
                return new HttpStatusCodeResult(400, ex.Message);
            }
        }
        [HttpPost]
        public ActionResult SignIn(ClientLoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            model.Password = CommonHelper.Encrypt(model.Password);
            var result = _factory.Login(model);
            if (result != null)
            {
                UserSession userSession = new UserSession();
                userSession.Email = result.Email;
                userSession.UserName = result.DisplayName;
                userSession.IsAdminClient = result.IsAdmin;
                userSession.FirstName = result.FirstName;
                userSession.LastName = result.LastName;
                userSession.Phone = result.Phone;
                userSession.Address = result.Address;
                userSession.UserId = result.Id;
                Session.Add("UserClient", userSession);
                string myObjectJson = JsonConvert.SerializeObject(userSession);  //new JavaScriptSerializer().Serialize(userSession);
                HttpCookie cookie = new HttpCookie("UserClientCookie");
                cookie.Expires = DateTime.Now.AddMonths(1);
                cookie.Value = Server.UrlEncode(myObjectJson);
                HttpContext.Response.Cookies.Add(cookie);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("Email", "Thông tin tài khoản không chính xác");
                return View(model);
            }
        }

        public ActionResult SignUp()
        {
            CMS_CustomerModels model = new CMS_CustomerModels();
            return View(model);
        }

        [HttpPost]
        public ActionResult SignUp(CMS_CustomerModels model)
        {
            try
            {
                PropertyReject();
                if (!string.IsNullOrEmpty(model.Password) && !string.IsNullOrEmpty(model.ConfirmPassword) && !model.Password.Equals(model.ConfirmPassword))
                    ModelState.AddModelError("ConfirmPassword", "Xác nhận Password không chính xác !");

                if (!ModelState.IsValid)
                    return View(model);
                model.Password = CommonHelper.Encrypt(model.Password);
                string msg = "";
                string cusId = "";
                model.Address = "None";
                model.UserLogin = (int)Commons.ETypeUserLogin.Normal;
                var result = _factory.InsertOrUpdate(model, ref cusId, ref msg);
                if (result)
                {
                    var data = _factory.GetDetail(cusId);
                    UserSession userSession = new UserSession();
                    userSession.UserId = data.ID;
                    userSession.Email = data.Email;
                    userSession.UserName = data.Name;
                    Session.Add("UserClient", userSession);
                    string myObjectJson = JsonConvert.SerializeObject(userSession);  //new JavaScriptSerializer().Serialize(userSession);
                    HttpCookie cookie = new HttpCookie("UserClientCookie");
                    cookie.Expires = DateTime.Now.AddMonths(1);
                    cookie.Value = Server.UrlEncode(myObjectJson);
                    HttpContext.Response.Cookies.Add(cookie);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("Email", msg);
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                NSLog.Logger.Error("SignUp", ex);
                return new HttpStatusCodeResult(400, ex.Message);
            }
        }

        [HttpGet]
        public ActionResult ForgetPassword()
        {
            ClientLoginModel model = new ClientLoginModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult ForgetPassword(ClientLoginModel model)
        {
            try
            {
                string msg = "";
                var result = _factory.ForgotPassword(model.Email, ref msg);
                if (result)
                {
                    //result = CommonHelper.ContactAdmin(model.ContactDTO);
                    if (result)
                    {
                        return RedirectToAction("Index", "Contact");
                    }
                    else
                    {
                        ModelState.AddModelError("Email", msg);
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", msg);
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                NSLog.Logger.Error("Contact_Email", ex);
                return new HttpStatusCodeResult(400, ex.Message);
            }
        }

        public ActionResult Profile(string q)
        {
            var model = new CMS_CustomerModels();
            try
            {
                NSLog.Logger.Info("Profile_Request:", q);
                model = _factory.GetDetail(q);
                if (model != null)
                {
                    model.Password = CommonHelper.Decrypt(model.Password);
                }
            }
            catch (Exception ex)
            {
                NSLog.Logger.Error("Profile_Error:", ex);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Profile(CMS_CustomerModels model)
        {
            try
            {
                NSLog.Logger.Info("Profile_Request:", model);
                var CusId = string.Empty;
                var msg = string.Empty;
                model.Password = CommonHelper.Encrypt(model.Password);
                var result = true;// _factory.CreateOrUpdate(model, ref CusId, ref msg);
                if (result)
                {
                    //model.IsShow = true;
                }
            }
            catch (Exception ex)
            {
                NSLog.Logger.Error("Profile_Error:", ex);
            }
            return View(model);
        }


        [HttpPost]
        public ActionResult GoogleLogin(string id, string firstname, string lastname, string fullname, string email, string picture)
        {
            FormsAuthentication.SetAuthCookie(email, false);
            ClientLoginModel model = new ClientLoginModel();
            model.Email = email;
            model.Picture = picture;
            model.Fb_ID = id;
            model.Password = id;

            var obj = new
            {
                message = 1
            };
            bool IsCheck = _factory.CheckExistLoginSosial(id);
            if (IsCheck)
            {
                var resultLogin = _factory.Login(model);
                if (resultLogin != null)
                {
                    UserSession userSession = new UserSession();
                    userSession.Email = resultLogin.Email;
                    userSession.UserName = resultLogin.DisplayName;
                    userSession.IsAdminClient = resultLogin.IsAdmin;
                    userSession.FirstName = resultLogin.FirstName;
                    userSession.LastName = resultLogin.LastName;
                    userSession.Phone = resultLogin.Phone;
                    userSession.Address = resultLogin.Address;
                    userSession.UserId = resultLogin.Id;
                    userSession.ImageUrl = resultLogin.ImageURL;
                    //userSession.PostCode = resultLogin.PostCode;
                    //userSession.Country = resultLogin.Country;
                    //userSession.City = resultLogin.City;
                    Session.Add("UserClient", userSession);
                    string myObjectJson = JsonConvert.SerializeObject(userSession);  //new JavaScriptSerializer().Serialize(userSession);
                    HttpCookie cookie = new HttpCookie("UserClientCookie");
                    cookie.Expires = DateTime.Now.AddMonths(1);
                    cookie.Value = Server.UrlEncode(myObjectJson);
                    HttpContext.Response.Cookies.Add(cookie);
                }
                else
                {
                    obj = new { message = 2 };
                    return Json(obj, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                CMS_CustomerModels modelGG = new CMS_CustomerModels();
                modelGG.GoogleID = id;
                modelGG.FirstName = firstname;
                modelGG.LastName = lastname;
                modelGG.Email = email;
                modelGG.ImageURL = picture;
                modelGG.UserLogin = (int)Commons.ETypeUserLogin.Gg;
                modelGG.Password = id;
                modelGG.Address = "User Facebook";
                string msg = "";
                string cusId = "";
                var resultSignUp = _factory.InsertOrUpdate(modelGG, ref cusId, ref msg);
                if (resultSignUp)
                {
                    var data = _factory.GetDetail(cusId);
                    UserSession userSession = new UserSession();
                    userSession.Email = data.Email;
                    userSession.UserName = data.FirstName + " " + data.LastName;
                    userSession.FirstName = data.FirstName;
                    userSession.LastName = data.LastName;
                    userSession.Phone = data.Phone;
                    userSession.Address = data.Address;
                    userSession.UserId = data.ID;
                    userSession.ImageUrl = data.ImageURL;
                    //userSession.PostCode = data.Postcode;
                    //userSession.Country = data.Country;
                    //userSession.City = data.City;
                    Session.Add("UserClient", userSession);
                    string myObjectJson = JsonConvert.SerializeObject(userSession);  //new JavaScriptSerializer().Serialize(userSession);
                    HttpCookie cookie = new HttpCookie("UserClientCookie");
                    cookie.Expires = DateTime.Now.AddMonths(1);
                    cookie.Value = Server.UrlEncode(myObjectJson);
                    HttpContext.Response.Cookies.Add(cookie);
                }
                else
                {
                    obj = new { message = 3 };
                    return Json(obj, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult FacebookLogin(string id, string firstname, string lastname, string fullname, string email, string picture)
        {
            FormsAuthentication.SetAuthCookie(email, false);
            ClientLoginModel model = new ClientLoginModel();
            model.Email = email;
            model.Picture = picture;
            model.Fb_ID = id;

            var obj = new
            {
                message = 1
            };
            bool IsCheck = _factory.CheckExistLoginSosial(id);
            if (IsCheck)
            {
                var resultLogin = _factory.Login(model);
                if (resultLogin != null)
                {
                    UserSession userSession = new UserSession();
                    userSession.Email = resultLogin.Email;
                    userSession.UserName = resultLogin.DisplayName;
                    userSession.IsAdminClient = resultLogin.IsAdmin;
                    userSession.FirstName = resultLogin.FirstName;
                    userSession.LastName = resultLogin.LastName;
                    userSession.Phone = resultLogin.Phone;
                    userSession.Address = resultLogin.Address;
                    userSession.UserId = resultLogin.Id;
                    userSession.ImageUrl = resultLogin.ImageURL;
                    //userSession.PostCode = resultLogin.PostCode;
                    //userSession.Country = resultLogin.Country;
                    //userSession.City = resultLogin.City;
                    Session.Add("UserClient", userSession);
                    string myObjectJson = JsonConvert.SerializeObject(userSession);  //new JavaScriptSerializer().Serialize(userSession);
                    HttpCookie cookie = new HttpCookie("UserClientCookie");
                    cookie.Expires = DateTime.Now.AddMonths(1);
                    cookie.Value = Server.UrlEncode(myObjectJson);
                    HttpContext.Response.Cookies.Add(cookie);
                }
                else
                {
                    obj = new { message = 2 };
                    return Json(obj, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                CMS_CustomerModels modelFB = new CMS_CustomerModels();
                modelFB.FbID = id;
                modelFB.FirstName = firstname;
                modelFB.LastName = lastname;
                modelFB.Email = email;
                modelFB.ImageURL = picture;
                modelFB.UserLogin = (int)Commons.ETypeUserLogin.Fb;
                modelFB.Password = id;
                modelFB.Address = "User Google";
                string msg = "";
                string cusId = "";
                var resultSignUp = _factory.InsertOrUpdate(modelFB, ref cusId, ref msg);
                if (resultSignUp)
                {
                    var data = _factory.GetDetail(cusId);
                    UserSession userSession = new UserSession();
                    userSession.Email = data.Email;
                    userSession.UserName = data.FirstName + " " + data.LastName;
                    userSession.FirstName = data.FirstName;
                    userSession.LastName = data.LastName;
                    userSession.Phone = data.Phone;
                    userSession.Address = data.Address;
                    userSession.UserId = data.ID;
                    userSession.ImageUrl = data.ImageURL;
                    //userSession.PostCode = data.Postcode;
                    //userSession.Country = data.Country;
                    //userSession.City = data.City;
                    Session.Add("UserClient", userSession);
                    string myObjectJson = JsonConvert.SerializeObject(userSession);  //new JavaScriptSerializer().Serialize(userSession);
                    HttpCookie cookie = new HttpCookie("UserClientCookie");
                    cookie.Expires = DateTime.Now.AddMonths(1);
                    cookie.Value = Server.UrlEncode(myObjectJson);
                    HttpContext.Response.Cookies.Add(cookie);
                }
                else
                {
                    obj = new { message = 3 };
                    return Json(obj, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        [AllowAnonymous]
        public ActionResult AjaxSignUp(CMS_CustomerModels model)
        {
            PropertyReject();
            model.Password = CommonHelper.Encrypt(model.Password);
            string msg = "";
            string cusId = "";
            model.FirstName = "None";
            model.LastName = "None";
            model.Address = "None";
            model.UserLogin = (int)Commons.ETypeUserLogin.Normal;
            var result = _factory.InsertOrUpdate(model, ref cusId, ref msg);
            if (result)
            {
                var data = _factory.GetDetail(cusId);
                UserSession userSession = new UserSession();
                userSession.UserId = data.ID;
                userSession.Email = data.Email;
                userSession.UserName = data.Name;
                Session.Add("UserClient", userSession);
                string myObjectJson = JsonConvert.SerializeObject(userSession);  
                HttpCookie cookie = new HttpCookie("UserClientCookie");
                cookie.Expires = DateTime.Now.AddMonths(1);
                cookie.Value = Server.UrlEncode(myObjectJson);
                HttpContext.Response.Cookies.Add(cookie);
                return Json(new { ok = true, url = "", users = model });
            }
            else
            {
                return Json(new { ok = false, message = "Đăng ký tài khoản không thành công!" });
            }
        }

        //[HttpPost]
        [AllowAnonymous]
        public ActionResult AjaxLogin(ClientLoginModel model)
        {
            model.Password = CommonHelper.Encrypt(model.Password);
            var result = _factory.Login(model);
            if (result != null)
            {
                UserSession userSession = new UserSession();
                userSession.Email = result.Email;
                userSession.UserName = result.DisplayName;
                userSession.IsAdminClient = result.IsAdmin;
                userSession.FirstName = result.FirstName;
                userSession.LastName = result.LastName;
                userSession.Phone = result.Phone;
                userSession.Address = result.Address;
                userSession.UserId = result.Id;
                Session.Add("UserClient", userSession);
                string myObjectJson = JsonConvert.SerializeObject(userSession); 
                HttpCookie cookie = new HttpCookie("UserClientCookie");
                cookie.Expires = DateTime.Now.AddMonths(1);
                cookie.Value = Server.UrlEncode(myObjectJson);
                HttpContext.Response.Cookies.Add(cookie);
                return Json(new { ok = true, url = "", users = model });
            }
            else
            {
                return Json(new { ok = false, message = "Thông tin tài khoản không chính xác!" });
            }
        }
    }
}