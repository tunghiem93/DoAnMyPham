using CMS_DTO.CMSNews;
using CMS_Entity;
using CMS_Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Shared.CMSNews
{
    public class CMSNewsFactory
    {
        public bool CreateOrUpdate(CMS_NewsModels model, ref string msg)
        {
            var Result = true;
            using (var cxt = new CMS_Context())
            {
                using (var trans = cxt.Database.BeginTransaction())
                {
                    try
                    {
                        if (string.IsNullOrEmpty(model.Id))
                        {
                            var _Id = Guid.NewGuid().ToString();
                            var e = new CMS_News
                            {
                                Id = _Id,
                                Title = model.Title,
                                Alias = model.Alias,
                                Publisher = model.Publisher,
                                Short_Description = model.Short_Description,
                                Type = model.Type,
                                ImageURL = model.ImageURL,
                                ImageURLAuthor = model.ImageURLAuthor,
                                CreatedBy = model.CreatedBy,
                                CreatedDate = DateTime.Now,
                                Description = model.Description,
                                UpdatedBy = model.UpdatedBy,
                                UpdatedDate = DateTime.Now,
                                IsActive = model.IsActive
                            };
                            cxt.CMS_News.Add(e);
                        }
                        else
                        {
                            var e = cxt.CMS_News.Find(model.Id);
                            if (e != null)
                            {
                                e.Title = model.Title;
                                e.Alias = model.Alias;
                                e.Publisher = model.Publisher;
                                e.Short_Description = model.Short_Description;
                                e.Type = model.Type;
                                e.ImageURL = model.ImageURL;
                                e.ImageURLAuthor = model.ImageURLAuthor;
                                e.Description = model.Description;
                                e.UpdatedBy = model.UpdatedBy;
                                e.UpdatedDate = DateTime.Now;
                                e.IsActive = model.IsActive;
                            }
                        }
                        cxt.SaveChanges();
                        trans.Commit();
                    }
                    catch (Exception)
                    {
                        Result = false;
                        msg = "Vui lòng kiểm tra đường truyền";
                        trans.Rollback();
                    }
                    finally
                    {
                        cxt.Dispose();
                    }
                }
            }
            return Result;
        }

        public bool Delete(string Id, ref string msg)
        {
            var Result = true;
            using (var cxt = new CMS_Context())
            {
                using (var trans = cxt.Database.BeginTransaction())
                {
                    try
                    {
                        var e = cxt.CMS_News.Find(Id);
                        if (e != null)
                        {
                            cxt.CMS_News.Remove(e);
                            cxt.SaveChanges();
                            trans.Commit();
                        }
                        else
                        {
                            msg = "Vui lòng kiểm tra đường truyền";
                        }
                    }
                    catch (Exception)
                    {
                        Result = false;
                        msg = "Vui lòng kiểm tra đường truyền";
                        trans.Rollback();
                    }
                    finally
                    {
                        cxt.Dispose();
                    }
                }
            }
            return Result;
        }

        public CMS_NewsModels GetDetail(string Id)
        {
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var e = cxt.CMS_News.Where(x => x.Id.Equals(Id)).FirstOrDefault();
                    if (e != null)
                    {
                        var o = new CMS_NewsModels
                        {
                            Id = e.Id,
                            CreatedBy = e.CreatedBy,
                            CreatedDate = e.CreatedDate,
                            Description = e.Description,
                            Type = e.Type,
                            IsActive = e.IsActive,
                            UpdatedBy = e.UpdatedBy,
                            UpdatedDate = e.UpdatedDate,
                            Title = e.Title,
                            Alias = e.Alias,
                            Publisher = e.Publisher,
                            Short_Description = e.Short_Description,
                            ImageURL = e.ImageURL,
                            ImageURLAuthor = e.ImageURLAuthor
                        };
                        return o;
                    }
                }
            }
            catch (Exception ex) { }
            return null;
        }

        public List<CMS_NewsModels> GetList()
        {
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var data = cxt.CMS_News
                        .Select(x => new CMS_NewsModels
                        {
                            Id = x.Id,
                            Title = x.Title,
                            Alias = x.Alias,
                            Publisher = x.Publisher,
                            Short_Description = x.Short_Description,
                            Type = x.Type,
                            ImageURL = x.ImageURL,
                            ImageURLAuthor = x.ImageURLAuthor,
                            CreatedBy = x.CreatedBy,
                            CreatedDate = x.CreatedDate,
                            Description = x.Description,
                            IsActive = x.IsActive,
                            UpdatedBy = x.UpdatedBy,
                            UpdatedDate = x.UpdatedDate,
                        }).ToList();
                    return data;
                }
            }
            catch (Exception) { }
            return null;
        }

        public List<CMS_NewsModels> GetListByType(int type , out int totalRecord, out int totalPage, int pageNo = 1, int pageSize = 25)
        {
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var _entity = cxt.CMS_News.Where( x => 1 == 1);
                    if (type != 0)
                    {
                        _entity = _entity.Where(x => x.Type == type);
                    }
                    totalRecord = _entity.Count();
                    totalPage = (int)Math.Ceiling(totalRecord / (double)pageSize);
                    pageNo = pageNo == 0 ? 1 : pageNo;
                    var data = _entity.OrderByDescending(x => x.CreatedDate).Skip((pageNo - 1) * pageSize).Take(pageSize)
                                            .Select(x => new CMS_NewsModels
                                            {
                                                Id = x.Id,
                                                Title = x.Title,
                                                Alias = x.Alias,
                                                Publisher = x.Publisher,
                                                Short_Description = x.Short_Description,
                                                Type = x.Type,
                                                ImageURL = x.ImageURL,
                                                ImageURLAuthor = x.ImageURLAuthor,
                                                CreatedBy = x.CreatedBy,
                                                CreatedDate = x.CreatedDate,
                                                Description = x.Description,
                                                IsActive = x.IsActive,
                                                UpdatedBy = x.UpdatedBy,
                                                UpdatedDate = x.UpdatedDate,
                                            }).ToList();
                    return data;
                }
            }
            catch (Exception ex) { }
            totalPage = 0;
            totalRecord = 0;
            return null;
        }

        public CMS_NewsModels GetNewsByAlias(string alias)
        {
            try
            {
                using(var cxt = new CMS_Context())
                {
                    var data = cxt.CMS_News.Where(x => x.Alias == alias)
                                            .Select(x => new CMS_NewsModels {
                                                Id = x.Id,
                                                Title = x.Title,
                                                Alias = x.Alias,
                                                Publisher = x.Publisher,
                                                Short_Description = x.Short_Description,
                                                Type = x.Type,
                                                ImageURL = x.ImageURL,
                                                ImageURLAuthor = x.ImageURLAuthor,
                                                CreatedBy = x.CreatedBy,
                                                CreatedDate = x.CreatedDate,
                                                Description = x.Description,
                                                IsActive = x.IsActive,
                                                UpdatedBy = x.UpdatedBy,
                                                UpdatedDate = x.UpdatedDate,
                                            }).FirstOrDefault();
                    return data;
                }
            }
            catch(Exception ex) { }
            return null;
        }
    }
}
