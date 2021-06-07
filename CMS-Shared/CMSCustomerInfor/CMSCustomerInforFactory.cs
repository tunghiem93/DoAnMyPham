using CMS_DTO.CMSCustomerInfor;
using CMS_Entity;
using CMS_Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Shared.CMSCustomerInfor
{
    public class CMSCustomerInforFactory
    {
        public bool CreateOrUpdate(CMS_CustomerInforModels model, ref string msg)
        {
            var result = true;
            using (var cxt = new CMS_Context())
            {
                using (var beginTran = cxt.Database.BeginTransaction())
                {
                    try
                    {
                        
                        if (string.IsNullOrEmpty(model.Id))
                        {
                            var _Id = Guid.NewGuid().ToString();
                            var e = new CMS_CustomersInfor()
                            {
                                CustomerId = model.CustomerId,
                                Name = model.Name,
                                Email  =model.Email,
                                Phone = model.Phone,
                                ZipCode = model.ZipCode,
                                ReceiveType = model.ReceiveType,
                                PreferredtDate = model.PreferredtDate == DateTime.MinValue ? DateTime.Now : model.PreferredtDate,
                                PreferredtTime = model.PreferredtTime,
                                PriceAmong = model.PriceAmong,
                                FinancingRequired = model.FinancingRequired,
                                EmailFriend  =model.EmailFriend,
                                Message = model.Message,
                                Checked = model.Checked,
                                CheckedMail = model.CheckedMail,
                                CheckedPhone = model.CheckedPhone,
                                Subject = model.Subject,
                                CreatedBy = model.CreatedBy,
                                CreatedDate = DateTime.Now,
                                IsActive = model.IsActive,
                                UpdatedBy = model.UpdatedBy,
                                UpdatedDate = DateTime.Now,
                                Id = _Id
                            };
                            cxt.CMS_CustomersInfor.Add(e);
                        }
                        else
                        {
                            var e = cxt.CMS_CustomersInfor.Find(model.Id);
                            if (e != null)
                            {
                                e.CustomerId = model.CustomerId;
                                e.Name = model.Name;
                                e.Email = model.Email;
                                e.Phone = model.Phone;
                                e.ZipCode = model.ZipCode;
                                e.ReceiveType = model.ReceiveType;
                                e.PreferredtDate = model.PreferredtDate;
                                e.PreferredtTime = model.PreferredtTime;
                                e.PriceAmong = model.PriceAmong;
                                e.FinancingRequired = model.FinancingRequired;
                                e.EmailFriend = model.EmailFriend;
                                e.Message = model.Message;
                                e.Checked = model.Checked;
                                e.CheckedMail = model.CheckedMail;
                                e.CheckedPhone = model.CheckedPhone;
                                e.Subject = model.Subject;
                                e.IsActive = model.IsActive;
                                e.UpdatedBy = model.UpdatedBy;
                                e.UpdatedDate = DateTime.Now;
                            }
                        }
                        cxt.SaveChanges();
                        beginTran.Commit();
                    }
                    catch (Exception ex)
                    {
                        msg = "Lỗi khi lưu thông tin";
                        beginTran.Rollback();
                        result = false;
                    }
                }
            }
            return result;
        }

        public bool Delete(string Id, ref string msg)
        {
            var result = true;
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var e = cxt.CMS_CustomersInfor.Find(Id);
                    cxt.CMS_CustomersInfor.Remove(e);
                    cxt.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = "Không thể thông tin này";
                result = false;
            }
            return result;
        }

        public CMS_CustomerInforModels GetDetail(string Id)
        {
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var data = cxt.CMS_CustomersInfor.Select(x => new CMS_CustomerInforModels
                    {
                        Id = x.Id,
                        CustomerId = x.CustomerId,
                        Name = x.Name,
                        Email = x.Email,
                        Phone = x.Phone,
                        ZipCode = x.ZipCode,
                        ReceiveType = x.ReceiveType,
                        PreferredtDate = x.PreferredtDate,
                        PreferredtTime = x.PreferredtTime,
                        PriceAmong = x.PriceAmong,
                        EmailFriend = x.EmailFriend,
                        FinancingRequired = x.FinancingRequired,
                        Message = x.Message,
                        Checked = x.Checked,
                        CheckedMail = x.CheckedMail,
                        CheckedPhone = x.CheckedPhone,
                        Subject = x.Subject,
                        IsActive = x.IsActive,
                        UpdatedBy = x.UpdatedBy,
                        UpdatedDate = x.UpdatedDate,
                        CreatedBy = x.CreatedBy,
                        CreatedDate = x.CreatedDate,
                    }).Where(x => x.Id.Equals(Id)).FirstOrDefault();

                    return data;
                }
            }
            catch (Exception ex) { }
            return null;
        }

        public List<CMS_CustomerInforModels> GetList()
        {
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var data = cxt.CMS_CustomersInfor.Select(x => new CMS_CustomerInforModels
                    {
                        Id = x.Id,
                        CustomerId = x.CustomerId,
                        Name = x.Name,
                        Email = x.Email,
                        Phone = x.Phone,
                        ZipCode = x.ZipCode,
                        ReceiveType = x.ReceiveType,
                        PreferredtDate = x.PreferredtDate,
                        PreferredtTime = x.PreferredtTime,
                        PriceAmong = x.PriceAmong,
                        EmailFriend = x.EmailFriend,
                        FinancingRequired = x.FinancingRequired,
                        Message = x.Message,
                        Checked = x.Checked,
                        CheckedMail = x.CheckedMail,
                        CheckedPhone = x.CheckedPhone,
                        Subject = x.Subject,
                        IsActive = x.IsActive,
                        UpdatedBy = x.UpdatedBy,
                        UpdatedDate = x.UpdatedDate,
                        CreatedBy = x.CreatedBy,
                        CreatedDate = x.CreatedDate,
                    }).ToList();                    
                    return data;
                }
            }
            catch (Exception ex) { }
            return null;
        }
    }
}
