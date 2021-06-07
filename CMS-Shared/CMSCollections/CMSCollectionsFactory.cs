using CMS_DTO.CMSCollection;
using CMS_Entity;
using CMS_Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Shared.CMSCollections
{
    public class CMSCollectionsFactory
    {
        public bool CreateOrUpdate(CMSCollectionModels model, ref string Id, ref string msg)
        {
            var result = true;
            using (var cxt = new CMS_Context())
            {
                using (var beginTran = cxt.Database.BeginTransaction())
                {
                    try
                    {
                        var _IsExits = cxt.CMS_Collections.Any(x => (x.CollectionName.Equals(model.CollectionName)) && (string.IsNullOrEmpty(model.Id) ? 1 == 1 : !x.Id.Equals(model.Id)));
                        if (_IsExits)
                        {
                            result = false;
                            msg = "Tên bộ sưu tập đã tồn tại";
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(model.Id))
                            {
                                var _Id = Guid.NewGuid().ToString();
                                var e = new CMS_Collections()
                                {
                                    CollectionName = model.CollectionName,
                                    Alias = model.Alias,
                                    Link = model.Link,
                                    TypeLink = model.TypeLink,
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
                                cxt.CMS_Collections.Add(e);
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
                                            CollectionId = _Id,
                                            UpdatedBy = model.UpdatedBy,
                                            UpdatedDate = DateTime.Now
                                        });
                                    });
                                    cxt.CMS_Images.AddRange(_e);
                                }
                            }
                            else
                            {
                                var e = cxt.CMS_Collections.Find(model.Id);
                                if (e != null)
                                {
                                    e.CollectionName = model.CollectionName;
                                    e.Alias = model.Alias;
                                    e.Link = model.Link;
                                    e.TypeLink = model.TypeLink;
                                    //e.IsActive = model.sStatus;
                                    e.Description = model.Description;
                                    e.IsActive = model.IsActive;
                                    e.UpdatedBy = model.UpdatedBy;
                                    e.UpdatedDate = DateTime.Now;
                                    e.ImageURL = model.ImageURL;
                                }
                                if (model.ListImageUrl != null && model.ListImageUrl.Any())
                                {
                                    var _edel = cxt.CMS_Images.Where(x => x.CollectionId != null && x.CollectionId.Equals(e.Id));
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
                                            CollectionId = e.Id,
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
                    // Delete Image of Product
                    var _eImage = cxt.CMS_Images.Where(x => x.CollectionId != null && x.CollectionId.Equals(Id));
                    cxt.CMS_Images.RemoveRange(_eImage);

                    var e = cxt.CMS_Collections.Find(Id);
                    cxt.CMS_Collections.Remove(e);
                    cxt.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = "Không thể xóa bộ sưu tập này";
                result = false;
            }
            return result;
        }

        public CMSCollectionModels GetDetail(string Id)
        {
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var data = cxt.CMS_Collections.Select(x => new CMSCollectionModels
                    {
                        CollectionName = x.CollectionName,
                        Alias = x.Alias,
                        Link = x.Link,
                        TypeLink = x.TypeLink,
                        CreatedBy = x.CreatedBy,
                        CreatedDate = x.CreatedDate,
                        Description = x.Description,
                        Id = x.Id,
                        IsActive = x.IsActive,
                        UpdatedBy = x.UpdatedBy,
                        UpdatedDate = x.UpdatedDate,
                        ImageURL = x.ImageURL,
                    }).Where(x => x.Id.Equals(Id)).FirstOrDefault();
                    var _images = cxt.CMS_Images.Select(x => new
                    {
                        ID = x.Id,
                        CollectionId = x.CollectionId,
                        ImageUrL = x.ImageURL
                    }).Where(z => z.CollectionId != null && z.CollectionId == Id).ToList();

                    if (_images != null && _images.Any())
                    {
                        /// add list image
                        var _temp = _images.Select(m => new ImageCollection
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
            catch (Exception ex) { }
            return null;
        }

        public List<CMSCollectionModels> GetList()
        {
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var data = cxt.CMS_Collections.Select(x => new CMSCollectionModels
                    {
                        CollectionName = x.CollectionName,
                        Alias = x.Alias,
                        Link = x.Link,
                        TypeLink = x.TypeLink,
                        CreatedBy = x.CreatedBy,
                        CreatedDate = x.CreatedDate,
                        Description = x.Description,
                        Id = x.Id,
                        IsActive = x.IsActive,
                        UpdatedBy = x.UpdatedBy,
                        UpdatedDate = x.UpdatedDate,
                        ImageURL = x.ImageURL
                    }).ToList();
                    
                    var _images = cxt.CMS_Images.Select(x => new
                    {
                        ID = x.Id,
                        CollectionId = x.CollectionId,
                        ImageUrL = x.ImageURL
                    }).ToList();

                    data.ForEach(x =>
                    {
                        if (_images != null && _images.Any())
                        {
                            /// add list image
                            var _temp = _images.Where(z => z.CollectionId != null && z.CollectionId.Equals(x.Id))
                                                    .Select(m => new ImageCollection
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
            }
            catch (Exception ex) { }
            return null;
        }
    }
}
