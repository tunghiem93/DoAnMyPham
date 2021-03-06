using CMS_DTO.CMSCustomer;
using CMS_Entity;
using CMS_Entity.Entity;
using CMS_Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Shared.CMSCustomers
{
    public class CMSCustomersFactory
    {
        public bool InsertOrUpdate(CMS_CustomerModels model, ref string Id, ref string msg)
        {
            var result = true;
            using (var cxt = new CMS_Context())
            {
                using (var trans = cxt.Database.BeginTransaction())
                {
                    var _isExits = cxt.CMS_Customers.Any(x => x.Email.Equals(model.Email)); 
                    try
                    {
                        if (string.IsNullOrEmpty(model.ID))
                        {
                            if (_isExits)
                            {
                                msg = "Địa chỉ email đã tồn tại";
                                result = false;
                            }
                            else
                            {
                                Id = Guid.NewGuid().ToString();
                                var e = new CMS_Customers
                                {
                                    Id = Id,
                                    FbID = model.FbID,
                                    GoogleID = model.GoogleID,
                                    Address = model.Address,
                                    BirthDate = model.BirthDate,
                                    City = model.City,
                                    CompanyName = model.CompanyName,
                                    Country = model.Country,
                                    CreatedBy = model.CreatedBy,
                                    CreatedDate = DateTime.Now,
                                    Description = model.Description,
                                    Email = model.Email,
                                    Gender = model.Gender,
                                    ImageURL = model.ImageURL,
                                    IsActive = model.IsActive,
                                    Status = (int)Commons.EStatus.Actived,
                                    Name = model.Name,
                                    MaritalStatus = model.MaritalStatus,
                                    Password = model.Password,
                                    Phone = model.Phone,
                                    Street = model.Street,
                                    UserLogin = model.UserLogin,
                                    UpdatedBy = model.UpdatedBy,
                                    UpdatedDate = DateTime.Now
                                };
                                cxt.CMS_Customers.Add(e);
                            }                            
                        }
                        else
                        {
                            var e = cxt.CMS_Customers.Find(model.ID);
                            if (e != null)
                            {
                                if (e.Email.Equals(model.Email) || !_isExits)
                                {
                                    e.Address = model.Address;
                                    e.BirthDate = model.BirthDate;
                                    e.City = model.City;
                                    e.CompanyName = model.CompanyName;
                                    e.Country = model.Country;
                                    e.UpdatedBy = model.UpdatedBy;
                                    e.Description = model.Description;
                                    e.Email = model.Email;
                                    e.Gender = model.Gender;
                                    e.ImageURL = model.ImageURL;
                                    e.IsActive = model.IsActive;
                                    e.Name = model.Name;
                                    e.MaritalStatus = model.MaritalStatus;
                                    e.Password = model.Password;
                                    e.Phone = model.Phone;
                                    e.Street = model.Street;
                                }
                                else
                                {
                                    msg = "Địa chỉ email đã tồn tại";
                                    result = false;
                                }                                    
                            }
                        }
                        cxt.SaveChanges();
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        result = false;
                        trans.Rollback();
                        msg = "Lỗi đường truyền mạng";
                    }
                    finally
                    {
                        cxt.Dispose();
                    }               
                }
            }
            return result;
        }

        public bool Delete(string Id, ref string msg)
        {
            var result = true;
            using (var cxt = new CMS_Context())
            {
                using (var trans = cxt.Database.BeginTransaction())
                {
                    try
                    {
                        var e = cxt.CMS_Customers.Find(Id);
                        cxt.CMS_Customers.Remove(e);
                        cxt.SaveChanges();
                        trans.Commit();
                    }
                    catch (Exception ex) {
                        result = false;
                        msg = "Không thể xóa thể loại này";
                        trans.Rollback();
                    }
                    finally
                    {
                        cxt.Dispose();
                    }

                }
            }
            return result;
        }

        public CMS_CustomerModels GetDetail(string Id)
        {
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var data = cxt.CMS_Customers.Where(x => x.Id.Equals(Id))
                                                .Select(x => new CMS_CustomerModels
                                                {
                                                    Address = x.Address,
                                                    BirthDate = x.BirthDate,
                                                    City = x.City,
                                                    CompanyName = x.CompanyName,
                                                    Country = x.Country,
                                                    CreatedBy = x.CreatedBy,
                                                    CreatedDate = x.CreatedDate,
                                                    Description = x.Description,
                                                    Email = x.Email,
                                                    Gender = x.Gender,
                                                    ID = x.Id,
                                                    ImageURL = x.ImageURL,
                                                    IsActive = x.IsActive,
                                                    Name = x.Name,
                                                    MaritalStatus = x.MaritalStatus,
                                                    Password = x.Password,
                                                    Phone = x.Phone,
                                                    Street = x.Street,
                                                    UpdatedBy = x.UpdatedBy,
                                                    UpdatedDate = x.UpdatedDate,
                                                    FbID = x.FbID,
                                                    GoogleID = x.GoogleID,
                                                    UserLogin = x.UserLogin
                                                }).FirstOrDefault();
                    return data;
                }
            }
            catch(Exception ex) { }
            return null;
        }

        public List<CMS_CustomerModels> GetList()
        {
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var data = cxt.CMS_Customers.Select(x => new CMS_CustomerModels
                    {
                        Address = x.Address,
                        BirthDate = x.BirthDate,
                        City = x.City,
                        CompanyName = x.CompanyName,
                        Country = x.Country,
                        CreatedBy = x.CreatedBy,
                        CreatedDate = x.CreatedDate,
                        Description = x.Description,
                        Email = x.Email,
                        Gender = x.Gender,
                        ID = x.Id,
                        ImageURL = x.ImageURL,
                        IsActive = x.IsActive,
                        MaritalStatus = x.MaritalStatus,
                        Password = x.Password,
                        Phone = x.Phone,
                        Street = x.Street,
                        UpdatedBy = x.UpdatedBy,
                        UpdatedDate = x.UpdatedDate,
                        FbID = x.FbID, 
                        GoogleID = x.GoogleID,
                        UserLogin = x.UserLogin,
                        Name = x.Name
                    }).ToList();
                    return data;
                }
            }
            catch(Exception ex) { }
            return null;
        }

        public ClientLoginModel Login(ClientLoginModel model)
        {
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var data = cxt.CMS_Customers.Where(x => x.Email.Equals(model.Email) &&
                                                         x.Password.Equals(model.Password) &&
                                                         x.IsActive)
                                              .Select(x => new ClientLoginModel
                                              {
                                                  Email = x.Email,
                                                  DisplayName = x.Name,
                                                  Password = x.Password,
                                                  IsAdmin = false,
                                                  Phone = x.Phone,
                                                  Id = x.Id,
                                                  ImageURL = x.ImageURL
                                              })
                                              .FirstOrDefault();
                    return data;
                }
            }
            catch (Exception ex)
            {
                NSLog.Logger.Error("Login", ex);
            }
            return null;
        }

        public bool ForgotPassword(string email, ref string msg)
        {
            NSLog.Logger.Info("CustomerForgotPassword", email);

            var result = false;
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var cus = cxt.CMS_Customers.Where(o => o.Email.ToLower().Trim() == email.ToLower().Trim() && o.IsActive).FirstOrDefault();
                    if (cus != null)
                    {
                        if (cus.IsActive)
                        {
                            string newPass = CommonHelper.GenerateCode(1, new List<string>(), 8).FirstOrDefault();

                            CommonHelper.SendContentMail(email, "New password: " + newPass, "", "Forgot password");

                            cus.Password = CommonHelper.Encrypt(newPass);
                            cus.UpdatedDate = DateTime.Now;

                            if (cxt.SaveChanges() > 0)
                                result = true;
                            else
                                msg = "Unable to change password.";
                        }
                        else
                            msg = "This customer is inactive. Please contact your administrator for more support.";
                    }
                    else
                        msg = "Email của bạn không tồn tại.";

                    NSLog.Logger.Info("ResponseCustomerForgotPassword", new { result, msg });
                }
            }
            catch (Exception ex)
            {
                msg = "System Error.";
                result = false;
                NSLog.Logger.Error("ErrorCustomerForgotPassword", ex);
            }
            return result;
        }

        public bool CheckExistLoginSosial(string Id)
        {
            bool result = true;
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var data = cxt.CMS_Customers.Where(o => (o.FbID == Id || o.GoogleID == Id) && o.Status == (byte)Commons.EStatus.Actived).FirstOrDefault();
                    if (data == null)
                    {
                        result = false;
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                NSLog.Logger.Error("ErrorCheckExistLoginSosial", ex);
            }
            return result;
        }
    }
}
