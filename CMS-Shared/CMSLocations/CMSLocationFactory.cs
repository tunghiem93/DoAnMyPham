using CMS_DTO.CMSLocation;
using CMS_Entity;
using CMS_Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Shared.CMSLocations
{
    public class CMSLocationFactory
    {
        public bool CreateOrUpdate(CMSLocationModels model, ref string Id, ref string msg)
        {
            var result = true;
            using (var cxt = new CMS_Context())
            {
                using (var beginTran = cxt.Database.BeginTransaction())
                {
                    try
                    {
                        var _IsExits = cxt.CMS_Brands.Any(x => (x.BrandCode.Equals(model.LocationCode) || x.BrandName.Equals(model.Name)) && (string.IsNullOrEmpty(model.Id) ? 1 == 1 : !x.Id.Equals(model.Id)));
                        if (_IsExits)
                        {
                            result = false;
                            msg = "Mã khu vực hoặc tên khu vực đã tồn tại";
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(model.Id))
                            {
                                var _Id = Guid.NewGuid().ToString();
                                var e = new CMS_Locations()
                                {
                                    LocationCode = model.LocationCode,
                                    Name = model.Name,
                                    Alias = model.Alias,
                                    Short_Description = model.Short_Description,
                                    CreatedBy = model.CreatedBy,
                                    CreatedDate = DateTime.Now,
                                    Description = model.Description,
                                    IsActive = model.IsActive,
                                    UpdatedBy = model.UpdatedBy,
                                    UpdatedDate = DateTime.Now,
                                    ImageURL = model.ImageURL,
                                    Id = _Id
                                };
                                Id = _Id;
                                cxt.CMS_Locations.Add(e);
                            }
                            else
                            {
                                var e = cxt.CMS_Locations.Find(model.Id);
                                if (e != null)
                                {
                                    e.LocationCode = model.LocationCode;
                                    e.Name = model.Name;
                                    e.Alias = model.Alias;
                                    e.Short_Description = model.Short_Description;
                                    e.Description = model.Description;
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
                    var e = cxt.CMS_Locations.Find(Id);
                    cxt.CMS_Locations.Remove(e);
                    cxt.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = "Không thể xóa khu vực này";
                result = false;
            }
            return result;
        }

        public CMSLocationModels GetDetail(string Id)
        {
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var data = cxt.CMS_Locations.Select(x => new CMSLocationModels
                    {
                        LocationCode = x.LocationCode,
                        Name = x.Name,
                        Alias = x.Alias,
                        Short_Description = x.Short_Description,
                        CreatedBy = x.CreatedBy,
                        CreatedDate = x.CreatedDate,
                        Description = x.Description,
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

        public List<CMSLocationModels> GetList()
        {
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var data = cxt.CMS_Locations.Select(x => new CMSLocationModels
                    {
                        LocationCode = x.LocationCode,
                        Name = x.Name,
                        Alias = x.Alias,
                        Short_Description = x.Short_Description,
                        CreatedBy = x.CreatedBy,
                        CreatedDate = x.CreatedDate,
                        Description = x.Description,
                        Id = x.Id,
                        IsActive = x.IsActive,
                        UpdatedBy = x.UpdatedBy,
                        UpdatedDate = x.UpdatedDate,
                        ImageURL = x.ImageURL
                    }).ToList();

                    /* count number of product */
                    var lstNumOfProduct = cxt.CMS_Products.GroupBy(o => o.LocationId).Select(o => new
                    {
                        ID = o.Key,
                        Count = o.Count(),
                    }).ToList();
                    data.ForEach(o =>
                    {
                        o.NumberOfProduct = lstNumOfProduct.Where(c => c.ID == o.Id).Select(c => c.Count).FirstOrDefault();
                    });

                    /* response data */
                    return data;
                }
            }
            catch (Exception ex) { }
            return null;
        }
    }
}
