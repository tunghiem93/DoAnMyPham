using CMS_DTO.CMSCustomer;
using CMS_DTO.CMSProduct;
using CMS_DTO.CMSSession;
using CMS_Shared;
using CMS_Shared.CMSCustomers;
using CMS_Shared.CMSProducts;
using CMS_Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_Web.Controllers
{
    public class AccountController : HQController
    {
        private readonly CMSProductFactory _facP;
        private readonly CMSCustomersFactory _facCus;
        List<string> listPropertyReject = null;
        public void PropertyReject()
        {
            foreach (var item in listPropertyReject)
            {
                if (ModelState.ContainsKey(item))
                    ModelState[item].Errors.Clear();
            }
        }

        public AccountController()
        {
            _facP = new CMSProductFactory();
            _facCus = new CMSCustomersFactory();
            listPropertyReject = new List<string>();
            listPropertyReject.Add("Address");
        }

        // GET: Account
        public ActionResult Index()
        {
            CMS_CustomerModels model = new CMS_CustomerModels();
            try
            {
                if (Session["UserClient"] != null)
                {
                    var CusInfo = Session["UserClient"] as UserSession;
                    model.ID = CusInfo.UserId;
                    model = _facCus.GetDetail(model.ID);
                    model.Password = CommonHelper.Decrypt(model.Password);
                }
                return View(model);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(400, ex.Message);
            }            
        }

        [HttpPost]
        public ActionResult Index(CMS_CustomerModels model)
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
                var result = _facCus.InsertOrUpdate(model, ref cusId, ref msg);
                if (result)
                {                   
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

        public ActionResult Dealer()
        {
            try
            {
                ProductViewModels model = new ProductViewModels();
                model.ListProduct = _facP.GetList().OrderByDescending(x => x.CreatedDate).Skip(0).Take(32).ToList();
                if (model.ListProduct != null && model.ListProduct.Any())
                {
                    model.ListProduct.ForEach(x =>
                    {
                        x.ImageURL = Commons.HostImage + "Products/" + x.ImageURL;
                        x.ListImg.ForEach(o => {
                            if (!string.IsNullOrEmpty(o.ImageURL))
                            {
                                o.ImageURL = Commons.HostImage + "Products/" + o.ImageURL;
                            }
                            o.IsDelete = false;
                        });
                    });
                }
                return View(model);
            }
            catch (Exception ex)
            {
                NSLog.Logger.Error("Index: ", ex);
                return new HttpStatusCodeResult(400, ex.Message);
            }
        }

        public ActionResult Category()
        {
            try
            {
                ProductViewModels model = new ProductViewModels();
                model.ListProduct = _facP.GetList().OrderByDescending(x => x.CreatedDate).ToList();
                if (model.ListProduct != null && model.ListProduct.Any())
                {
                    model.ListProduct.ForEach(x =>
                    {
                        x.ImageURL = Commons.HostImage + "Products/" + x.ImageURL;
                        x.ListImg.ForEach(o => {
                            if (!string.IsNullOrEmpty(o.ImageURL))
                            {
                                o.ImageURL = Commons.HostImage + "Products/" + o.ImageURL;
                            }
                            o.IsDelete = false;
                        });
                    });
                }
                return View(model);
            }
            catch (Exception ex)
            {
                NSLog.Logger.Error("Index: ", ex);
                return new HttpStatusCodeResult(400, ex.Message);
            }
        }

        public ActionResult ManagementSearch()
        {
            try
            {
                ProductViewModels model = new ProductViewModels();
                model.ListProduct = _facP.GetList().OrderByDescending(x => x.CreatedDate).ToList();
                if (model.ListProduct != null && model.ListProduct.Any())
                {
                    model.ListProduct.ForEach(x =>
                    {
                        x.ImageURL = Commons.HostImage + "Products/" + x.ImageURL;
                        x.ListImg.ForEach(o => {
                            if (!string.IsNullOrEmpty(o.ImageURL))
                            {
                                o.ImageURL = Commons.HostImage + "Products/" + o.ImageURL;
                            }
                            o.IsDelete = false;
                        });
                    });
                }
                return View(model);
            }
            catch (Exception ex)
            {
                NSLog.Logger.Error("Index: ", ex);
                return new HttpStatusCodeResult(400, ex.Message);
            }
        }

        public ActionResult ManagementSave()
        {
            try
            {
                ProductViewModels model = new ProductViewModels();
                model.ListProduct = _facP.GetList().OrderByDescending(x => x.CreatedDate).ToList();
                if (model.ListProduct != null && model.ListProduct.Any())
                {
                    model.ListProduct.ForEach(x =>
                    {
                        x.ImageURL = Commons.HostImage + "Products/" + x.ImageURL;
                        x.ListImg.ForEach(o => {
                            if (!string.IsNullOrEmpty(o.ImageURL))
                            {
                                o.ImageURL = Commons.HostImage + "Products/" + o.ImageURL;
                            }
                            o.IsDelete = false;
                        });
                    });
                }
                return View(model);
            }
            catch (Exception ex)
            {
                NSLog.Logger.Error("Index: ", ex);
                return new HttpStatusCodeResult(400, ex.Message);
            }
        }

        public ActionResult ManagementRequest()
        {
            try
            {
                ProductViewModels model = new ProductViewModels();
                model.ListProduct = _facP.GetList().OrderByDescending(x => x.CreatedDate).ToList();
                if (model.ListProduct != null && model.ListProduct.Any())
                {
                    model.ListProduct.ForEach(x =>
                    {
                        x.ImageURL = Commons.HostImage + "Products/" + x.ImageURL;
                        x.ListImg.ForEach(o => {
                            if (!string.IsNullOrEmpty(o.ImageURL))
                            {
                                o.ImageURL = Commons.HostImage + "Products/" + o.ImageURL;
                            }
                            o.IsDelete = false;
                        });
                    });
                }
                return View(model);
            }
            catch (Exception ex)
            {
                NSLog.Logger.Error("Index: ", ex);
                return new HttpStatusCodeResult(400, ex.Message);
            }
        }

        public ActionResult ProfileAccount()
        {
            try
            {
                CMS_CustomerModels model = new CMS_CustomerModels();
                var customer = Session["UserClient"] as UserSession;
                if (customer != null && customer.UserId != null)
                {
                    model = _facCus.GetDetail(customer.UserId);
                    if (model == null)
                    {
                        model = new CMS_CustomerModels();
                    }
                }           
                return View(model);
            }
            catch (Exception ex)
            {
                NSLog.Logger.Error("Index: ", ex);
                return new HttpStatusCodeResult(400, ex.Message);
            }
        }

        [HttpPost]
        public ActionResult ProfileAccount(CMS_CustomerModels model)
        {
            try
            {
                
                return View(model);
            }
            catch (Exception ex)
            {
                NSLog.Logger.Error("Index: ", ex);
                return new HttpStatusCodeResult(400, ex.Message);
            }
        }

        public ActionResult EditAccount()
        {
            try
            {
                ProductViewModels model = new ProductViewModels();
                model.ListProduct = _facP.GetList().OrderByDescending(x => x.CreatedDate).ToList();
                if (model.ListProduct != null && model.ListProduct.Any())
                {
                    model.ListProduct.ForEach(x =>
                    {
                        x.ImageURL = Commons.HostImage + "Products/" + x.ImageURL;
                        x.ListImg.ForEach(o => {
                            if (!string.IsNullOrEmpty(o.ImageURL))
                            {
                                o.ImageURL = Commons.HostImage + "Products/" + o.ImageURL;
                            }
                            o.IsDelete = false;
                        });
                    });
                }
                return View(model);
            }
            catch (Exception ex)
            {
                NSLog.Logger.Error("Index: ", ex);
                return new HttpStatusCodeResult(400, ex.Message);
            }
        }
    }
}