using CMS_DTO.CMSPage;
using CMS_Entity;
using CMS_Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Shared.CMSPages
{
    public class CMSPagesFactory
    {
        public bool CreateOrUpdate(CMS_PageModes model, ref string msg)
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
                            var e = new CMS_Pages
                            {
                                Id = _Id,
                                Name = model.Name,
                                Alias = model.Alias,
                                CreatedBy = model.CreatedBy,
                                CreatedDate = DateTime.Now,
                                Description = model.Description,
                                Type = model.Type,
                                UpdatedBy = model.UpdatedBy,
                                UpdatedDate = DateTime.Now,
                                IsActive = model.IsActive
                            };
                            cxt.CMS_Pages.Add(e);
                        }
                        else
                        {
                            var e = cxt.CMS_Pages.Find(model.Id);
                            if (e != null)
                            {
                                e.Name = model.Name;
                                e.Alias = model.Alias;
                                e.Description = model.Description;
                                e.Type = model.Type;
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
                        var e = cxt.CMS_Pages.Find(Id);
                        if (e != null)
                        {
                            cxt.CMS_Pages.Remove(e);
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

        public CMS_PageModes GetDetail(string Id)
        {
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var e = cxt.CMS_Pages.Where(x => x.Id.Equals(Id)).FirstOrDefault();
                    if (e != null)
                    {
                        var o = new CMS_PageModes
                        {
                            Id = e.Id,
                            CreatedBy = e.CreatedBy,
                            CreatedDate = e.CreatedDate,
                            Description = e.Description,
                            Type = e.Type,
                            IsActive = e.IsActive,
                            UpdatedBy = e.UpdatedBy,
                            UpdatedDate = e.UpdatedDate,
                            Alias = e.Alias,
                            Name = e.Name,
                        };
                        return o;
                    }
                }
            }
            catch (Exception ex) { }
            return null;
        }

        public List<CMS_PageModes> GetList()
        {
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var data = cxt.CMS_Pages
                        .Select(x => new CMS_PageModes
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Alias = x.Alias,
                            CreatedBy = x.CreatedBy,
                            CreatedDate = x.CreatedDate,
                            Description = x.Description,
                            Type = x.Type,
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
    }
}
