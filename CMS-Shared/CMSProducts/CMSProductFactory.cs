using CMS_DTO.CMSImage;
using CMS_DTO.CMSProduct;
using CMS_Entity;
using CMS_Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Shared.CMSProducts
{
    public class CMSProductFactory
    {
        public bool CreateOrUpdate(CMS_ProductsModels model, ref string msg)
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
                            var e = new CMS_Products
                            {
                                Id = _Id,
                                CategoryId = model.CategoryId,
                                BrandId = model.BrandId,
                                LocationId = model.LocationId,
                                //StyleId = model.StyleId,
                                CreatedBy = model.CreatedBy,
                                CreatedDate = DateTime.Now,
                                TypeSize = model.TypeSize,
                                TypeState = model.TypeState,
                                Short_Description = model.Short_Description,
                                Description = model.Description,
                                ProductCode = model.ProductCode,
                                ProductName = model.ProductName,
                                Vendor = model.Vendor,
                                LinkVideo = model.LinkVideo,
                                Information = model.Information,
                                ProductPrice = model.ProductPrice,
                                ProductExtraPrice = model.ProductExtraPrice,
                                UpdatedBy = model.UpdatedBy,
                                UpdatedDate = DateTime.Now,
                                IsActive = model.IsActive,
                                ImageURL = model.ImageURL,
                                Alias = model.Alias,
                                Year = model.Year,
                            };
                            cxt.CMS_Products.Add(e);

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
                                        ProductId = _Id,
                                        UpdatedBy = model.UpdatedBy,
                                        UpdatedDate = DateTime.Now
                                    });
                                });
                                cxt.CMS_Images.AddRange(_e);
                            }
                        }
                        else
                        {
                            var e = cxt.CMS_Products.Find(model.Id);
                            if (e != null)
                            {
                                e.ProductName = model.ProductName;
                                e.ProductCode = model.ProductCode;
                                e.ProductPrice = model.ProductPrice;
                                e.ProductExtraPrice = model.ProductExtraPrice;
                                e.Short_Description = model.Short_Description;
                                e.Description = model.Description;
                                e.Vendor = model.Vendor;
                                e.LinkVideo = model.LinkVideo;
                                e.Information = model.Information;
                                e.TypeSize = model.TypeSize;
                                e.TypeState = model.TypeState;
                                e.CategoryId = model.CategoryId;
                                e.BrandId = model.BrandId;
                                e.LocationId = model.LocationId;
                                //e.StyleId = model.StyleId;
                                e.UpdatedBy = model.UpdatedBy;
                                e.UpdatedDate = DateTime.Now;
                                e.IsActive = model.IsActive;
                                e.ImageURL = model.ImageURL;
                                e.Alias = model.Alias;
                                e.Year = model.Year;
                            }

                            if (model.ListImageUrl != null && model.ListImageUrl.Any())
                            {
                                var _edel = cxt.CMS_Images.Where(x => x.ProductId != null && x.ProductId.Equals(e.Id));
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
                                        ProductId = e.Id,
                                        UpdatedBy = model.UpdatedBy,
                                        UpdatedDate = DateTime.Now
                                    });
                                });
                                cxt.CMS_Images.AddRange(_e);
                            }
                        }
                        cxt.SaveChanges();
                        trans.Commit();
                    }
                    catch (Exception ex)
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
                        // Delete Image of Product
                        var _eImage = cxt.CMS_Images.Where(x => x.ProductId != null && x.ProductId.Equals(Id));
                        cxt.CMS_Images.RemoveRange(_eImage);

                        var e = cxt.CMS_Products.Find(Id);
                        if (e != null)
                        {
                            cxt.CMS_Products.Remove(e);
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

        public CMS_ProductsModels GetDetail(string Id)
        {
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var e = cxt.CMS_Products.Join(cxt.CMS_Categories,
                                                    p => p.CategoryId,
                                                    c => c.Id,
                                                    (p, c) => new { p, CategoryName = c.CategoryName, CategoryType = c.Type })
                                             .Where(x => x.p.Id.Equals(Id)).FirstOrDefault();
                    if (e != null)
                    {
                        var result = new CMS_ProductsModels
                        {
                            Id = e.p.Id,
                            CategoryId = e.p.CategoryId,
                            BrandId = e.p.BrandId,
                            LocationId = e.p.LocationId,
                            CreatedBy = e.p.CreatedBy,
                            CreatedDate = e.p.CreatedDate,
                            Short_Description = e.p.Short_Description,
                            Description = e.p.Description,
                            IsActive = e.p.IsActive,
                            ProductCode = e.p.ProductCode,
                            ProductName = e.p.ProductName,
                            ProductPrice = e.p.ProductPrice,
                            ProductExtraPrice = (e.p.ProductExtraPrice > 0 && e.p.ProductExtraPrice < e.p.ProductPrice) ? e.p.ProductExtraPrice : e.p.ProductPrice,
                            LinkVideo = e.p.LinkVideo,
                            Vendor = e.p.Vendor,
                            Information = e.p.Information,
                            TypeState = e.p.TypeState,
                            TypeSize = e.p.TypeSize,
                            UpdatedBy = e.p.UpdatedBy,
                            UpdatedDate = e.p.UpdatedDate,
                            CategoryName = e.CategoryName,
                            ImageURL = e.p.ImageURL,
                            Alias = e.p.Alias,
                            Year = e.p.Year,
                            CategoryType = e.CategoryType
                        };

                        var _images = cxt.CMS_Images.Select(x => new
                        {
                            ID = x.Id,
                            ProductID = x.ProductId,
                            ImageUrL = x.ImageURL
                        }).Where(z => z.ProductID != null && z.ProductID == Id).ToList();

                        if (_images != null && _images.Any())
                        {
                            /// add list image
                            var _temp = _images.Select(m => new ImageProduct
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
                                result.ListImg.AddRange(_temp);
                            }
                        }
                        return result;
                    }
                }
            }
            catch (Exception ex) { }
            return null;
        }

        public List<CMS_ProductsModels> GetList()
        {
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var lstResult = cxt.CMS_Products
                        .Join(cxt.CMS_Categories, p => p.CategoryId, c => c.Id, (p, c) => new { p, CategoryName = c.CategoryName, CategoryType = c.Type, CategoryAlias = c.Alias })
                        .GroupJoin(cxt.CMS_Brands, p => p.p.BrandId, b => b.Id, (p, b) => new { p.p, p.CategoryName, p.CategoryType, b = b.FirstOrDefault(), p.CategoryAlias })
                        .Select(x => new CMS_ProductsModels
                        {
                            Id = x.p.Id,
                            CategoryId = x.p.CategoryId,
                            BrandId = x.p.BrandId,
                            LocationId = x.p.LocationId,
                            CreatedBy = x.p.CreatedBy,
                            CreatedDate = x.p.CreatedDate,
                            Short_Description = x.p.Short_Description,
                            Description = x.p.Description,
                            IsActive = x.p.IsActive,
                            ProductCode = x.p.ProductCode,
                            ProductName = x.p.ProductName,
                            ProductPrice = x.p.ProductPrice,
                            ProductExtraPrice = (x.p.ProductExtraPrice > 0 && x.p.ProductExtraPrice < x.p.ProductPrice) ? x.p.ProductExtraPrice : x.p.ProductPrice,
                            Vendor = x.p.Vendor,
                            LinkVideo = x.p.LinkVideo,
                            Information = x.p.Information,
                            TypeSize = x.p.TypeSize,
                            TypeState = x.p.TypeState,
                            UpdatedBy = x.p.UpdatedBy,
                            UpdatedDate = x.p.UpdatedDate,
                            Alias = x.p.Alias,
                            Year = x.p.Year,
                            ImageURL = x.p.ImageURL,
                            CategoryName = string.IsNullOrEmpty(x.CategoryName) ? "" : x.CategoryName,
                            BrandName = x.b == null ? "" : (string.IsNullOrEmpty(x.b.BrandName) ? "" : x.b.BrandName),
                            CategoryType = x.CategoryType,
                            AliasCate = x.CategoryAlias
                        }).ToList();
                    var _images = cxt.CMS_Images.Select(x => new
                    {
                        ID = x.Id,
                        ProductID = x.ProductId,
                        ImageUrL = x.ImageURL
                    }).ToList();

                    lstResult.ForEach(x =>
                    {
                        if (_images != null && _images.Any())
                        {
                            /// add list image
                            var _temp = _images.Where(z => z.ProductID != null && z.ProductID.Equals(x.Id))
                                                    .Select(m => new ImageProduct
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

                    return lstResult;
                }
            }
            catch (Exception ex) { }
            return null;
        }

        public List<CMS_ProductsModels> GetListProductCate(string alias)
        {
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var data = cxt.CMS_Products.Join(cxt.CMS_Categories,
                                                    p => p.CategoryId,
                                                    c => c.Id,
                                                    (p, c) => new { p, CategoryName = c.CategoryName, CategoryType = c.Type, AliasCate = c.Alias })
                                               .Select(x => new CMS_ProductsModels
                                               {
                                                   Id = x.p.Id,
                                                   CategoryId = x.p.CategoryId,
                                                   BrandId = x.p.BrandId,
                                                   LocationId = x.p.LocationId,
                                                   CreatedBy = x.p.CreatedBy,
                                                   CreatedDate = x.p.CreatedDate,
                                                   Short_Description = x.p.Short_Description,
                                                   Description = x.p.Description,
                                                   ImageURL = x.p.ImageURL,
                                                   IsActive = x.p.IsActive,
                                                   ProductCode = x.p.ProductCode,
                                                   ProductName = x.p.ProductName,
                                                   ProductPrice = x.p.ProductPrice,
                                                   ProductExtraPrice = (x.p.ProductExtraPrice > 0 && x.p.ProductExtraPrice < x.p.ProductPrice) ? x.p.ProductExtraPrice : x.p.ProductPrice,
                                                   Vendor = x.p.Vendor,
                                                   LinkVideo = x.p.LinkVideo,
                                                   Information = x.p.Information,
                                                   TypeSize = x.p.TypeSize,
                                                   TypeState = x.p.TypeState,
                                                   UpdatedBy = x.p.UpdatedBy,
                                                   UpdatedDate = x.p.UpdatedDate,
                                                   CategoryName = x.CategoryName,
                                                   Alias = x.p.Alias,
                                                   AliasCate = x.AliasCate,
                                                   Year = x.p.Year,
                                                   CategoryType = x.CategoryType
                                               }).Where(w => w.AliasCate.Equals(alias)).ToList();
                    return data;
                }
            }
            catch (Exception) { }
            return null;
        }

        public List<CMS_ProductsModels> GetListProductLocation(string alias)
        {
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var data = cxt.CMS_Products.Join(cxt.CMS_Locations,
                                                    p => p.CategoryId,
                                                    c => c.Id,
                                                    (p, lo) => new { p, LocationName = lo.Name, AliasLoca = lo.Alias })
                                               .Select(x => new CMS_ProductsModels
                                               {
                                                   Id = x.p.Id,
                                                   CategoryId = x.p.CategoryId,
                                                   BrandId = x.p.BrandId,
                                                   LocationId = x.p.LocationId,
                                                   CreatedBy = x.p.CreatedBy,
                                                   CreatedDate = x.p.CreatedDate,
                                                   Short_Description = x.p.Short_Description,
                                                   Description = x.p.Description,
                                                   ImageURL = x.p.ImageURL,
                                                   IsActive = x.p.IsActive,
                                                   ProductCode = x.p.ProductCode,
                                                   ProductName = x.p.ProductName,
                                                   ProductPrice = x.p.ProductPrice,
                                                   ProductExtraPrice = (x.p.ProductExtraPrice > 0 && x.p.ProductExtraPrice < x.p.ProductPrice) ? x.p.ProductExtraPrice : x.p.ProductPrice,
                                                   Vendor = x.p.Vendor,
                                                   LinkVideo = x.p.LinkVideo,
                                                   Information = x.p.Information,
                                                   TypeSize = x.p.TypeSize,
                                                   TypeState = x.p.TypeState,
                                                   UpdatedBy = x.p.UpdatedBy,
                                                   UpdatedDate = x.p.UpdatedDate,
                                                   LocationName = x.LocationName,
                                                   Alias = x.p.Alias,
                                                   AliasLoca = x.AliasLoca,
                                                   Year = x.p.Year,
                                               }).Where(w => w.AliasLoca.Equals(alias)).ToList();
                    return data;
                }
            }
            catch (Exception) { }
            return null;
        }

        public List<CMS_ProductsModels> GetListProductBrand(string alias)
        {
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var data = cxt.CMS_Products.Join(cxt.CMS_Brands,
                                                    p => p.BrandId,
                                                    b => b.Id,
                                                    (p, b) => new { p, BrandName = b.BrandName, AliasBrand = b.Alias })
                                               .Select(x => new CMS_ProductsModels
                                               {
                                                   Id = x.p.Id,
                                                   CategoryId = x.p.CategoryId,
                                                   BrandId = x.p.BrandId,
                                                   LocationId = x.p.LocationId,
                                                   CreatedBy = x.p.CreatedBy,
                                                   CreatedDate = x.p.CreatedDate,
                                                   Short_Description = x.p.Short_Description,
                                                   Description = x.p.Description,
                                                   ImageURL = x.p.ImageURL,
                                                   IsActive = x.p.IsActive,
                                                   ProductCode = x.p.ProductCode,
                                                   ProductName = x.p.ProductName,
                                                   ProductPrice = x.p.ProductPrice,
                                                   ProductExtraPrice = (x.p.ProductExtraPrice > 0 && x.p.ProductExtraPrice < x.p.ProductPrice) ? x.p.ProductExtraPrice : x.p.ProductPrice,
                                                   Vendor = x.p.Vendor,
                                                   LinkVideo = x.p.LinkVideo,
                                                   Information = x.p.Information,
                                                   TypeSize = x.p.TypeSize,
                                                   TypeState = x.p.TypeState,
                                                   UpdatedBy = x.p.UpdatedBy,
                                                   UpdatedDate = x.p.UpdatedDate,
                                                   BrandName = x.BrandName,
                                                   Alias = x.p.Alias,
                                                   AliasBrand = x.AliasBrand,
                                                   Year = x.p.Year,
                                               }).Where(w => w.AliasBrand.Equals(alias)).ToList();
                    return data;
                }
            }
            catch (Exception) { }
            return null;
        }

        //public List<CMS_ProductsModels> GetListProductStyle(string alias)
        //{
        //    try
        //    {
        //        using (var cxt = new CMS_Context())
        //        {
        //            var data = cxt.CMS_Products.Join(cxt.CMS_Styles,
        //                                            p => p.StyleId,
        //                                            s => s.Id,
        //                                            (p, s) => new { p, StyleName = s.Name, AliasStyle = s.Alias })
        //                                       .Select(x => new CMS_ProductsModels
        //                                       {
        //                                           Id = x.p.Id,
        //                                           CategoryId = x.p.CategoryId,
        //                                           BrandId = x.p.BrandId,
        //                                           LocationId = x.p.LocationId,
        //                                           StyleId = x.p.StyleId,
        //                                           CreatedBy = x.p.CreatedBy,
        //                                           CreatedDate = x.p.CreatedDate,
        //                                           Short_Description = x.p.Short_Description,
        //                                           Description = x.p.Description,
        //                                           ImageURL = x.p.ImageURL,
        //                                           IsActive = x.p.IsActive,
        //                                           ProductCode = x.p.ProductCode,
        //                                           ProductName = x.p.ProductName,
        //                                           ProductPrice = x.p.ProductPrice,
        //                                           ProductExtraPrice = (x.p.ProductExtraPrice > 0 && x.p.ProductExtraPrice < x.p.ProductPrice) ? x.p.ProductExtraPrice : x.p.ProductPrice,
        //                                           Vendor = x.p.Vendor,
        //                                           LinkVideo = x.p.LinkVideo,
        //                                           Information = x.p.Information,
        //                                           TypeSize = x.p.TypeSize,
        //                                           TypeState = x.p.TypeState,
        //                                           UpdatedBy = x.p.UpdatedBy,
        //                                           UpdatedDate = x.p.UpdatedDate,
        //                                           StyleName = x.StyleName,
        //                                           Alias = x.p.Alias,
        //                                           AliasStyle = x.AliasStyle,
        //                                           Year = x.p.Year,
        //                                       }).Where(w => w.AliasStyle.Equals(alias)).ToList();
        //            return data;
        //        }
        //    }
        //    catch (Exception) { }
        //    return null;
        //}

        public List<CMS_ImagesModels> GetListImage()
        {
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var data = cxt.CMS_Images
                                             .Select(x => new CMS_ImagesModels
                                             {
                                                 ImageName = x.ImageURL,
                                                 Id = x.Id,
                                                 ImageURL = x.ImageURL,
                                                 ProductId = x.ProductId
                                             }).ToList();
                    return data;
                }
            }
            catch (Exception ex) { }
            return null;
        }

        public List<CMS_ImagesModels> GetListImageOfProduct(string ProductId)
        {
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var data = cxt.CMS_Images.Where(x => x.ProductId != null && x.ProductId.Equals(ProductId))
                                             .Select(x => new CMS_ImagesModels
                                             {
                                                 ImageName = x.ImageURL,
                                                 Id = x.Id,
                                                 ImageURL = x.ImageURL,
                                                 ProductId = x.ProductId
                                             }).ToList();
                    return data;
                }
            }
            catch (Exception ex) { }
            return null;
        }

        public bool DeleteImage(string Id, ref string msg)
        {
            var result = true;
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var e = cxt.CMS_Images.Find(Id);
                    if (e != null)
                    {
                        msg = e.ImageURL;
                        cxt.CMS_Images.Remove(e);
                        cxt.SaveChanges();
                    }
                    else
                    {
                        result = false;
                        msg = "Vui lòng kiểm tra đường truyền";
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                msg = "Vui lòng kiểm tra đường truyền";
            }
            return result;
        }

        public List<CMS_ProductsModels> GetListProductByCategory(string alias, out int totalRecord, out int totalPage, int pageNo = 1 , int pageSize = 1, string a = "", string pr = "")
        {
            try
            {
                using(var cxt = new CMS_Context())
                {
                    var entity = cxt.CMS_Products.Join(cxt.CMS_Categories, p => p.CategoryId, c => c.Id, (p, c) => new
                    {
                        p,
                        Category_Name = c.CategoryName,
                        Alias = c.Alias
                    });
                    
                    if(!string.IsNullOrEmpty(alias) && alias.Equals("ruou-ngoai"))
                    {
                        var _Alias = new List<string> { "hennessy", "chivas", "johnnier-walker", "macalla", "vodka", "vang" };
                        entity = entity.Where(x => _Alias.Contains(x.Alias));
                    }
                    else if(!string.IsNullOrEmpty(alias) && alias.Equals("qua-tet"))
                    {
                        var _Alias2 = new List<string> { "hop-qua-tet", "gio-qua-tet", "tui-qua-tet", "hop-qua-cao-cap" };
                        entity = entity.Where(x => _Alias2.Contains(x.Alias));
                    }
                    else if(!string.IsNullOrEmpty(alias))
                    {
                        entity = entity.Where(x => x.Alias.Equals(alias));
                    }

                    totalRecord = entity.Count();
                    totalPage = (int)Math.Ceiling(totalRecord / (double)pageSize);
                    if (!string.IsNullOrEmpty(pr) && pr.ToLower() == "asc")
                    {
                        entity = entity.OrderBy(x => x.p.ProductPrice);
                    } else if (!string.IsNullOrEmpty(pr) && pr.ToLower() == "desc")
                    {
                        entity = entity.OrderByDescending(x => x.p.ProductPrice);
                    } else
                    {
                        entity = entity.OrderByDescending(x => x.p.CreatedDate);
                    }

                    entity = entity.Skip((pageNo - 1) * pageSize).Take(pageSize);
                    var data = entity.Select(x => new CMS_ProductsModels
                                    {
                                        Id = x.p.Id,
                                        CategoryId = x.p.CategoryId,
                                        BrandId = x.p.BrandId,
                                        LocationId = x.p.LocationId,
                                        CreatedBy = x.p.CreatedBy,
                                        CreatedDate = x.p.CreatedDate,
                                        Short_Description = x.p.Short_Description,
                                        Description = x.p.Description,
                                        ImageURL = x.p.ImageURL,
                                        IsActive = x.p.IsActive,
                                        ProductCode = x.p.ProductCode,
                                        ProductName = x.p.ProductName,
                                        ProductPrice = x.p.ProductPrice,
                                        ProductExtraPrice = (x.p.ProductExtraPrice > 0 && x.p.ProductExtraPrice < x.p.ProductPrice) ? x.p.ProductExtraPrice : x.p.ProductPrice,
                                        Vendor = x.p.Vendor,
                                        LinkVideo = x.p.LinkVideo,
                                        Information = x.p.Information,
                                        TypeSize = x.p.TypeSize,
                                        TypeState = x.p.TypeState,
                                        UpdatedBy = x.p.UpdatedBy,
                                        UpdatedDate = x.p.UpdatedDate,
                                        CategoryName = x.Category_Name,
                                        Alias = x.p.Alias,
                                        AliasCate = x.Alias,
                                        Year = x.p.Year,
                                    }).ToList();
                    return data;
                }
            }
            catch(Exception ex) { }
            totalPage = 0; totalRecord = 0;
            return null;
        }

        public CMS_ProductsModels GetProductByAlias(string alias)
        {
            try
            {
                using(var cxt = new CMS_Context())
                {
                    var data = cxt.CMS_Products.Join(cxt.CMS_Categories, p => p.CategoryId, c => c.Id, (p, c) => new { p, Category_Name = c.CategoryName, Category_Alias = c.Alias })
                                                .Where(x => x.p.Alias.Equals(alias))
                                                .Select(x => new CMS_ProductsModels
                                                {
                                                    Id = x.p.Id,
                                                    CategoryId = x.p.CategoryId,
                                                    BrandId = x.p.BrandId,
                                                    LocationId = x.p.LocationId,
                                                    CreatedBy = x.p.CreatedBy,
                                                    CreatedDate = x.p.CreatedDate,
                                                    Short_Description = x.p.Short_Description,
                                                    Description = x.p.Description,
                                                    ImageURL = x.p.ImageURL,
                                                    IsActive = x.p.IsActive,
                                                    ProductCode = x.p.ProductCode,
                                                    ProductName = x.p.ProductName,
                                                    ProductPrice = x.p.ProductPrice,
                                                    ProductExtraPrice = (x.p.ProductExtraPrice > 0 && x.p.ProductExtraPrice < x.p.ProductPrice) ? x.p.ProductExtraPrice : x.p.ProductPrice,
                                                    Vendor = x.p.Vendor,
                                                    LinkVideo = x.p.LinkVideo,
                                                    Information = x.p.Information,
                                                    TypeSize = x.p.TypeSize,
                                                    TypeState = x.p.TypeState,
                                                    UpdatedBy = x.p.UpdatedBy,
                                                    UpdatedDate = x.p.UpdatedDate,
                                                    CategoryName = x.Category_Name,
                                                    Alias = x.p.Alias,
                                                    AliasCate = x.Category_Alias,
                                                    Year = x.p.Year,
                                                }).FirstOrDefault();
                    if (data != null)
                    {
                        var _images = cxt.CMS_Images.Select(x => new
                        {
                            ID = x.Id,
                            ProductID = x.ProductId,
                            ImageUrL = x.ImageURL
                        }).Where(z => z.ProductID != null && z.ProductID == data.Id).ToList();

                        if (_images != null && _images.Any())
                        {
                            /// add list image
                            var _temp = _images.Select(m => new ImageProduct
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
                    }
                    return data;
                }
            }
            catch (Exception ex) { }
            return null;
        }
    }
}
