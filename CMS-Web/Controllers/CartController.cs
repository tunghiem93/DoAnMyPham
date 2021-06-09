using CMS_DTO.CMSOrder;
using CMS_DTO.CMSSession;
using CMS_Shared.CMSProducts;
using CMS_Shared.Promotions;
using CMS_Shared.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CMS_Web.Controllers
{
    public class CartController : HQController
    {
        private CMSProductFactory _fac;
        private PromotionsDAL _fapro;
        public CartController()
        {
            _fac = new CMSProductFactory();
            _fapro = new PromotionsDAL();
        }
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CheckOut()
        {
            CMS_CheckOutModels model = new CMS_CheckOutModels();
            try
            {
                var _Orders = GetListOrderCookie();
                NSLog.Logger.Info("List Order Cookie", JsonConvert.SerializeObject(_Orders));
                if (_Orders != null && _Orders.Any())
                {
                    var ItemIds = _Orders.Select(x => x.ItemId).ToList();
                    var data = _fac.GetList().Where(o => ItemIds.Contains(o.Id))
                                             .Select(o => new CMS_ItemModels
                                             {
                                                 ProductID = o.Id,
                                                 ProductName = o.ProductName,
                                             }).ToList();
                    if (data != null && data.Any())
                    {
                        data.ForEach(o =>
                        {
                            var item = _Orders.FirstOrDefault(z => z.ItemId.Equals(o.ProductID));
                            o.Quantity = item.Quantity;
                            o.ImageUrl = item.ImageUrl;
                            o.Price = item.Price;
                            o.TotalPrice = Convert.ToDouble(o.Price * item.Quantity);
                        });
                        model.ListItem = data;
                        model.TotalPrice = data.Sum(o => o.TotalPrice);
                        model.SubTotalPrice = data.Sum(o => o.TotalPrice);
                    }
                }

                /* get information customer from session */
                if (Session["UserClient"] != null)
                {
                    var CusInfo = Session["UserClient"] as UserSession;
                    model.Customer.FirstName = CusInfo.FirstName;
                    model.Customer.LastName = CusInfo.LastName;
                    model.Customer.FullName = CusInfo.UserName;
                    model.Customer.Phone = CusInfo.Phone;
                    model.Customer.Email = CusInfo.Email;
                    model.Customer.Address = CusInfo.Address;
                    model.Customer.Id = CusInfo.UserId;
                }
            }
            catch (Exception ex)
            {
                NSLog.Logger.Error("CheckOut", ex);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Payment(CMS_CheckOutModels model)
        {
            try
            {
                var CusInfo = Session["UserClient"] as UserSession;
                var _Orders = GetListOrderCookie();
                NSLog.Logger.Info("List Order Cookie", JsonConvert.SerializeObject(_Orders));
                if (_Orders != null && _Orders.Any())
                {
                    var ItemIds = _Orders.Select(x => x.ItemId).ToList();
                    var data = _fac.GetList().Where(o => ItemIds.Contains(o.Id))
                                             .Select(o => new CMS_ItemModels
                                             {
                                                 ProductID = o.Id,
                                                 ProductName = o.ProductName,
                                             }).ToList();
                    if (data != null && data.Any())
                    {
                        data.ForEach(o =>
                        {
                            var item = _Orders.FirstOrDefault(z => z.ItemId.Equals(o.ProductID));
                            o.Quantity = item.Quantity;
                            o.ImageUrl = item.ImageUrl;
                            o.Price = item.Price;
                            o.TotalPrice = Convert.ToDouble(o.Price * item.Quantity);
                        });
                        model.ListItem = data;
                        model.SubTotalPrice = data.Sum(o => o.TotalPrice);
                    }
                    model.OrderDate = DateTime.Now;
                    model.OrderNo = CommonHelper.RandomNumberOrder();
                    string body = MailHelper.CreateBodyMail(model);
                    var result = true;// MailHelper.SendMailOrder("[V/v đơn hàng " + model.OrderNo + "]", body, CusInfo.Email);
                    if (model.ListItem != null && model.ListItem.Any())
                    {
                        model.ListItem.ForEach(f =>
                        {
                            result = _fac.AddQuantity(f.ProductID, Convert.ToInt32(f.Quantity));
                        });
                    }
                    
                    if (result)
                    {
                        HttpCookie currentUserCookie = HttpContext.Request.Cookies["cms-order"];
                        HttpContext.Response.Cookies.Remove("cms-order");
                        currentUserCookie.Expires = DateTime.Now.AddDays(-10);
                        currentUserCookie.Value = null;
                        HttpContext.Response.SetCookie(currentUserCookie);
                        return Json(new { status = true, message = "Đơn hàng của bạn đang được quản trị xét duyệt!" });
                    }
                    else
                    {
                        return Json(new { status = false, message = "Quá trình thanh toán không thành công!" });
                    }
                }
                else
                {
                    return Json(new { status = false, message = "Giỏ hàng của bạn hiện đang không có sản phẩm!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Quá trình thanh toán không thành công!" });
            }
        }

        public async Task<ActionResult> CheckDiscount(string Code)
        {
            var data = await _fapro.CheckDiscount(Code);
            if (data != null && !string.IsNullOrEmpty(data.Id))
            {   
                return Json(new { status = true, data= data.Value, message = "Mã khuyễn mãi của bạn đã được áp dụng thành công!" });
            }
            else
            {   
                return Json(new { status = false, message = "Mã khuyễn mãi của bạn không tồn tại!" });
            }
        }
    }
}