using CMS_DTO.CMSBase;
using CMS_Shared;
using CMS_Shared.CMSBrands;
using CMS_Shared.CMSCategories;
using CMS_Shared.CMSLocations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_Web.Areas.Admin.Controllers
{
    public class HQController : Controller
    {
        public List<SelectListItem> GetListCategorySelectItem()
        {
            var _factory = new CMSCategoriesFactory();
            var data = _factory.GetList().Select(x => new SelectListItem
            {
                Value = x.Id,
                Text = x.CategoryName,
            }).ToList();
            return data;
        }

        public List<SelectListItem> GetListBrandSelectItem()
        {
            var _factory = new CMSBrandsFactory();
            var data = _factory.GetList().Select(x => new SelectListItem
            {
                Value = x.Id,
                Text = x.BrandName,
            }).ToList();
            return data;
        }

        public List<SelectListItem> GetListLocationSelectItem()
        {
            var _factory = new CMSLocationFactory();
            var data = _factory.GetList().Select(x => new SelectListItem
            {
                Value = x.Id,
                Text = x.Name,
            }).ToList();
            return data;
        }

        public List<CategoryByCategory> GetListCategory()
        {
            var _factory = new CMSCategoriesFactory();
            //var data = _factory.GetList().Where(x => !string.IsNullOrEmpty(x.ParentId)).Select(x => new SelectListItem
            //{
            //    Value = x.Id,
            //    Text = x.CategoryName,
            //}).ToList();
            //return data;
            var models = new List<CategoryByCategory>();
            var data = _factory.GetList();
            if (data != null)
            {
                var groupCate = data.Where(x => string.IsNullOrEmpty(x.ParentId)).ToList();
                if (groupCate != null)
                {
                    groupCate.ForEach(x =>
                    {
                        var model = new CategoryByCategory();
                        model.id = x.Id;
                        model.text = x.CategoryName.ToUpper();
                        model.children = data.Where(y => !string.IsNullOrEmpty(y.ParentId) && y.ParentId.Equals(x.Id))
                                                .Select(z => new CategoryChildren
                                                {
                                                    id = z.Id,
                                                    text = z.CategoryName
                                                }).ToList();
                        models.Add(model);
                    });
                }
            }

            return models;
        }

        //public List<SelectListItem> GetListStyle()
        //{
        //    var _factory = new CMSStyleFactory();
        //    var data = _factory.GetList().Select(x => new SelectListItem
        //    {
        //        Value = x.Id,
        //        Text = x.Name,
        //    }).ToList();
        //    return data;
        //}

        public List<SelectListItem> getListSize()
        {
            var _lstSize = new List<SelectListItem>() {
                new SelectListItem() { Text = "XS", Value = Commons.ESizeType.XS.ToString("d") },
                new SelectListItem() { Text = "S", Value = Commons.ESizeType.S.ToString("d") },
                new SelectListItem() { Text = "M", Value = Commons.ESizeType.M.ToString("d") },
                new SelectListItem() { Text = "L", Value = Commons.ESizeType.L.ToString("d") },
                new SelectListItem() { Text = "XL", Value = Commons.ESizeType.XL.ToString("d") },
            };
            return _lstSize;
        }

        public List<SelectListItem> getListStar()
        {
            var _lstStar = new List<SelectListItem>() {
                new SelectListItem() { Text = "1 Star", Value = Commons.EStarType.Star1.ToString("d") },
                new SelectListItem() { Text = "2 Star", Value = Commons.EStarType.Star2.ToString("d") },
                new SelectListItem() { Text = "3 Star", Value = Commons.EStarType.Star3.ToString("d") },
                new SelectListItem() { Text = "4 Star", Value = Commons.EStarType.Star4.ToString("d") },
                new SelectListItem() { Text = "5 Star", Value = Commons.EStarType.Star5.ToString("d") },
            };
            return _lstStar;
        }

        public List<SelectListItem> getListState()
        {
            var _lstSize = new List<SelectListItem>() {
                new SelectListItem() { Text = "Available", Value = Commons.EStateType.Avalable.ToString("d") },
                new SelectListItem() { Text = "Out of Stock", Value = Commons.EStateType.None.ToString("d") },
            };
            return _lstSize;
        }

        public List<SelectListItem> getListLink()
        {
            var _lstLink = new List<SelectListItem>() {
                new SelectListItem() { Text = "image", Value = Commons.ETypeLink.Images.ToString("d") },
                new SelectListItem() { Text = "Links", Value = Commons.ETypeLink.Links.ToString("d") },
                new SelectListItem() { Text = "Sliders", Value = Commons.ETypeLink.Sliders.ToString("d") },
                new SelectListItem() { Text = "Videos", Value = Commons.ETypeLink.Videos.ToString("d") },
            };
            return _lstLink;
        }

        public List<SelectListItem> getListRequestInfor()
        {
            var _lstLink = new List<SelectListItem>() {
                new SelectListItem() { Text = "Bình thường", Value = Commons.ETypeUserRequest.RequestNormal.ToString("d") },
                new SelectListItem() { Text = "NewsLetter", Value = Commons.ETypeUserRequest.RequestNewsLetter.ToString("d") },
                new SelectListItem() { Text = "Yêu cầu thông tin", Value = Commons.ETypeUserRequest.RequestInfor.ToString("d") },
                new SelectListItem() { Text = "Yêu cầu đặt lịch", Value = Commons.ETypeUserRequest.RequestBook.ToString("d") },
                new SelectListItem() { Text = "Yêu cầu Price", Value = Commons.ETypeUserRequest.RequestMakeInfor.ToString("d") },
                new SelectListItem() { Text = "Yêu cầu gửi bạn bè", Value = Commons.ETypeUserRequest.RequestSendFriend.ToString("d") },
            };
            return _lstLink;
        }

        public List<SelectListItem> getListGroupCate()
        {
            var _factory = new CMSCategoriesFactory();
            var data = _factory.GetList().Where(w => string.IsNullOrEmpty(w.ParentId)).Select(x => new SelectListItem
            {
                Value = x.Id,
                Text = x.CategoryName,
            }).ToList();
            return data;
            //var _lstSize = new List<SelectListItem>() {
            //    new SelectListItem() { Text = "Thiết bị", Value = Commons.ETypeMachine.ThietBi.ToString("d") },
            //    new SelectListItem() { Text = "Dịch vụ", Value = Commons.ETypeMachine.DichVu.ToString("d") },
            //};
            //return _lstSize;
        }

        public List<SelectListItem> getListCateNews()
        {
            var _lst = new List<SelectListItem>() {
                new SelectListItem() { Text = "Khuyến mãi", Value = Commons.ETypeNews.KhuyenMai.ToString("d") },
                new SelectListItem() { Text = "Thị Trường", Value = Commons.ETypeNews.ThiTruong.ToString("d") },
            };
            return _lst;
        }

        public List<SelectListItem> getListCateType()
        {
            var _lstType = new List<SelectListItem>() {
                new SelectListItem() { Text = "Nước tẩy trang", Value = Commons.ECateType.NuocTayTrang.ToString("d") },
                new SelectListItem() { Text = "Sữa rửa mặt", Value = Commons.ECateType.SuaRuaMat.ToString("d") },
                new SelectListItem() { Text = "Tẩy tế bào chết", Value = Commons.ECateType.TayTeBaoChet.ToString("d") },
                new SelectListItem() { Text = "Nước hoa hồng", Value = Commons.ECateType.NuocHoaHong.ToString("d") },
                new SelectListItem() { Text = "Kem chống nắng", Value = Commons.ECateType.KemChongNang.ToString("d") },
            };
            return _lstType;
        }
    }
}