using CMS_DTO.CMSCategories;
using CMS_DTO.CMSProduct;
using CMS_Entity;
using CMS_Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Shared.CMSCategories
{
    public class CMSCategoriesFactory
    {
        public bool CreateOrUpdate(CMSCategoriesModels model,ref string Id, ref string msg)
        {
            var result = true;
            using (var cxt = new CMS_Context())
            {
                using (var beginTran = cxt.Database.BeginTransaction())
                {
                    try
                    {
                        var _IsExits = cxt.CMS_Categories.Any(x =>(x.CategoryCode.Equals(model.CategoryCode) || x.CategoryName.Equals(model.CategoryName)) && (string.IsNullOrEmpty(model.Id) ? 1 == 1 : !x.Id.Equals(model.Id)));
                        if (_IsExits)
                        {
                            result = false;
                            msg = "Mã thể loại hoặc tên thể lại đã tồn tại";
                        } 
                        else
                        {
                            if (string.IsNullOrEmpty(model.Id))
                            {
                                var _Id = Guid.NewGuid().ToString();
                                var e = new CMS_Categories()
                                {
                                    CategoryCode = model.CategoryCode,
                                    CategoryName = model.CategoryName,
                                    Alias = model.Alias,
                                    Short_Description = model.Short_Description,
                                    CreatedBy = model.CreatedBy,
                                    CreatedDate = DateTime.Now,
                                    Description = model.Description,
                                    IsActive = model.IsActive,
                                    UpdatedBy = model.UpdatedBy,
                                    UpdatedDate = DateTime.Now,
                                    ParentId = model.ParentId,
                                    ImageURL = model.ImageURL,
                                    Type = model.Type,
                                    Id = _Id
                                };
                                Id = _Id;
                                cxt.CMS_Categories.Add(e);
                                if (model.ListImageUrl != null && model.ListImageUrl.Any())
                                {
                                    var _e = new List<CMS_Images>();
                                    model.ListImageUrl.ForEach(x =>
                                    {
                                        _e.Add(new CMS_Images
                                        {
                                            Id = Guid.NewGuid().ToString(),
                                            CreatedBy = model.CreatedBy,
                                            CreatedDate = DateTime.Now,
                                            ImageURL = x,
                                            CategoryId = _Id,
                                            UpdatedBy = model.UpdatedBy,
                                            UpdatedDate = DateTime.Now
                                        });
                                    });
                                    cxt.CMS_Images.AddRange(_e);
                                }
                            }
                            else
                            {
                                var e = cxt.CMS_Categories.Find(model.Id);
                                if (e != null)
                                {
                                    e.CategoryCode = model.CategoryCode;
                                    e.CategoryName = model.CategoryName;
                                    e.Alias = model.Alias;
                                    //e.IsActive = model.sStatus;
                                    e.Description = model.Description;
                                    e.Short_Description = model.Short_Description;
                                    e.IsActive = model.IsActive;
                                    e.UpdatedBy = model.UpdatedBy;
                                    e.UpdatedDate = DateTime.Now;
                                    e.ParentId = model.ParentId;
                                    e.ImageURL = model.ImageURL;
                                    e.Type = model.Type;
                                }
                                if (model.ListImageUrl != null && model.ListImageUrl.Any())
                                {
                                    var _edel = cxt.CMS_Images.Where(x => x.CategoryId != null && x.CategoryId.Equals(e.Id));
                                    cxt.CMS_Images.RemoveRange(_edel);

                                    var _e = new List<CMS_Images>();
                                    model.ListImageUrl.ForEach(x =>
                                    {
                                        _e.Add(new CMS_Images
                                        {
                                            Id = Guid.NewGuid().ToString(),
                                            CreatedBy = model.CreatedBy,
                                            CreatedDate = DateTime.Now,
                                            ImageURL = x,
                                            CategoryId = e.Id,
                                            UpdatedBy = model.UpdatedBy,
                                            UpdatedDate = DateTime.Now
                                        });
                                    });
                                    cxt.CMS_Images.AddRange(_e);
                                }
                            }
                            cxt.SaveChanges();
                            beginTran.Commit();
                        }
                    }
                    catch(Exception ex) {
                        msg = "Lỗi đường truyền mạng";
                        beginTran.Rollback();
                        result = false;
                    }
                }
            }
            return result;
        }

        public bool Delete(string Id , ref string msg)
        {
            var result = true;
            try
            {
                using (var cxt = new CMS_Context())
                {
                    // Delete Image of Product
                    var _eImage = cxt.CMS_Images.Where(x => x.CategoryId != null && x.CategoryId.Equals(Id));
                    cxt.CMS_Images.RemoveRange(_eImage);

                    var e = cxt.CMS_Categories.Find(Id);
                    cxt.CMS_Categories.Remove(e);
                    cxt.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                msg = "Không thể xóa thể loại này";
                result = false;
            }
            return result;
        }

        public CMSCategoriesModels GetDetail(string Id)
        {
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var data = cxt.CMS_Categories.Select(x => new CMSCategoriesModels
                    {
                        CategoryCode = x.CategoryCode,
                        CategoryName = x.CategoryName,
                        Alias = x.Alias,
                        Short_Description = x.Short_Description,
                        CreatedBy = x.CreatedBy,
                        CreatedDate = x.CreatedDate,
                        Description = x.Description,
                        Id = x.Id,
                        IsActive = x.IsActive,
                        UpdatedBy = x.UpdatedBy,
                        UpdatedDate = x.UpdatedDate,
                        ParentId = x.ParentId,
                        ImageURL = x.ImageURL,
                        Type = x.Type,
                    }).Where(x=>x.Id.Equals(Id)).FirstOrDefault();
                    var _images = cxt.CMS_Images.Select(x => new
                    {
                        ID = x.Id,
                        CategoryId = x.CategoryId,
                        ImageUrL = x.ImageURL
                    }).Where(z => z.CategoryId != null && z.CategoryId == Id).ToList();

                    if (_images != null && _images.Any())
                    {
                        /// add list image
                        var _temp = _images.Select(m => new ImageCate
                        {
                            ImageURL = m.ImageUrL,
                            IsDelete = true
                        }).ToList();
                        var _offSet = 0;
                        _temp.ForEach(k =>
                        {
                            k.OffSet = _offSet;
                            _offSet++;
                        });
                        if (_temp != null && _temp.Any())
                        {
                            data.ListImg.AddRange(_temp);
                        }
                    }
                    return data;
                }
            }
            catch(Exception ex) { }
            return null;
        }

        public List<CMSCategoriesModels> GetList()
        {
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var data = cxt.CMS_Categories.Select(x => new CMSCategoriesModels
                    {
                        CategoryCode = x.CategoryCode,
                        CategoryName = x.CategoryName,
                        Alias = x.Alias,
                        Short_Description = x.Short_Description,
                        CreatedBy = x.CreatedBy,
                        CreatedDate = x.CreatedDate,
                        Description = x.Description,
                        Id = x.Id,
                        IsActive = x.IsActive,
                        UpdatedBy = x.UpdatedBy,
                        UpdatedDate = x.UpdatedDate,
                        ParentId = x.ParentId,
                        ImageURL = x.ImageURL,
                        Type = x.Type,
                    }).ToList();

                    /* count number of product */
                    var lstNumOfProduct = cxt.CMS_Products.GroupBy(o => o.CategoryId).Select(o => new
                    {
                        ID = o.Key,
                        Count = o.Count(),
                    }).ToList();
                    data.ForEach(o =>
                    {
                        o.NumberOfProduct = lstNumOfProduct.Where(c => c.ID == o.Id).Select(c => c.Count).FirstOrDefault();
                    });

                    var _images = cxt.CMS_Images.Select(x => new
                    {
                        ID = x.Id,
                        CategoryId = x.CategoryId,
                        ImageUrL = x.ImageURL
                    }).ToList();

                    data.ForEach(x =>
                    {
                        if (_images != null && _images.Any())
                        {
                            /// add list image
                            var _temp = _images.Where(z => z.CategoryId != null && z.CategoryId.Equals(x.Id))
                                                    .Select(m => new ImageCate
                                                    {
                                                        ImageURL = m.ImageUrL,
                                                        IsDelete = true
                                                    }).ToList();
                            var _offSet = 0;
                            _temp.ForEach(k =>
                            {
                                k.OffSet = _offSet;
                                _offSet++;
                            });
                            if (_temp != null && _temp.Any())
                            {
                                x.ListImg.AddRange(_temp);
                            }

                        }
                    });

                    /* response data */
                    return data;
                }
            }catch(Exception ex) { }
            return null;
        }
    }
}
