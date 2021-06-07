using CMS_DTO.CMSBanner;
using CMS_Entity;
using CMS_Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Shared.CMSbanners
{
    public class CMSBannersFactory
    {
        public bool CreateOrUpdate(CMSBannerModels model, ref string Id, ref string msg)
        {
            var result = true;
            using (var cxt = new CMS_Context())
            {
                using (var beginTran = cxt.Database.BeginTransaction())
                {
                    try
                    {
                        var _IsExits = cxt.CMS_Banners.Any(x => (x.Name.Equals(model.Name)) && (string.IsNullOrEmpty(model.Id) ? 1 == 1 : !x.Id.Equals(model.Id)));
                        if (_IsExits)
                        {
                            result = false;
                            msg = "Tên banner đã tồn tại";
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(model.Id))
                            {
                                var _Id = Guid.NewGuid().ToString();
                                var e = new CMS_Banners()
                                {
                                    Name = model.Name,
                                    Alias = model.Alias,
                                    Link = model.Link,
                                    CreatedBy = model.CreatedBy,
                                    CreatedDate = DateTime.Now,
                                    IsActive = model.IsActive,
                                    UpdatedBy = model.UpdatedBy,
                                    UpdatedDate = DateTime.Now,
                                    ImageURL = model.ImageURL,
                                    Id = _Id
                                };
                                Id = _Id;
                                cxt.CMS_Banners.Add(e);
                            }
                            else
                            {
                                var e = cxt.CMS_Banners.Find(model.Id);
                                if (e != null)
                                {
                                    e.Name = model.Name;
                                    e.Alias = model.Alias;
                                    e.Link = model.Link;
                                    e.IsActive = model.IsActive;
                                    e.UpdatedBy = model.UpdatedBy;
                                    e.UpdatedDate = DateTime.Now;
                                    e.ImageURL = model.ImageURL;
                                }
                            }
                            cxt.SaveChanges();
                            beginTran.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        msg = "Lỗi đường truyền mạng";
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
                    var e = cxt.CMS_Banners.Find(Id);
                    cxt.CMS_Banners.Remove(e);
                    cxt.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = "Không thể xóa banner này";
                result = false;
            }
            return result;
        }

        public CMSBannerModels GetDetail(string Id)
        {
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var data = cxt.CMS_Banners.Select(x => new CMSBannerModels
                    {
                        Name = x.Name,
                        Alias = x.Alias,
                        Link = x.Link,
                        CreatedBy = x.CreatedBy,
                        CreatedDate = x.CreatedDate,
                        Id = x.Id,
                        IsActive = x.IsActive,
                        UpdatedBy = x.UpdatedBy,
                        UpdatedDate = x.UpdatedDate,
                        ImageURL = x.ImageURL,
                    }).Where(x => x.Id.Equals(Id)).FirstOrDefault();

                    return data;
                }
            }
            catch (Exception ex) { }
            return null;
        }

        public List<CMSBannerModels> GetList()
        {
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var data = cxt.CMS_Banners.Select(x => new CMSBannerModels
                    {
                        Name = x.Name,
                        Alias = x.Alias,
                        Link = x.Link,
                        CreatedBy = x.CreatedBy,
                        CreatedDate = x.CreatedDate,
                        Id = x.Id,
                        IsActive = x.IsActive,
                        UpdatedBy = x.UpdatedBy,
                        UpdatedDate = x.UpdatedDate,
                        ImageURL = x.ImageURL
                    }).ToList();                    
                    return data;
                }
            }
            catch (Exception ex) { }
            return null;
        }
    }
}
