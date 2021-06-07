using CMS_DTO.CMSBanner;
using CMS_DTO.CMSBrand;
using CMS_DTO.CMSCategories;
using CMS_DTO.CMSCollection;
using CMS_DTO.CMSCompany;
using CMS_DTO.CMSLocation;
using CMS_DTO.CMSNews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_DTO.CMSProduct
{
    public class ProductViewModels
    {
        public CMS_ProductsModels ProductModel { get; set; }
        public List<CMSBannerModels> ListBanner { get; set; }
        public List<CMS_ProductsModels> ListProduct { get; set; }
        public List<CMS_ProductsModels> ListProductTopSales { get; set; }
        public List<CMS_ProductsModels> ListSameProduct { get; set; }
        public CMS_CompanyModels Company { get; set; }
        public List<CMS_NewsModels> ListNews { get; set; }
        public List<CMS_NewsModels> ListNewsOld { get; set; }
        public List<CMSLocationModels> ListLocation { get; set; }

        public CMSCategoriesModels CateModel { get; set; }
        public CMSBrandsModels BrandModel { get; set; }
        public string GroupCateID { get; set; }
        public List<CMSCategoriesModels> ListGroupCate { get; set; }
        public string CateID { get; set; }
        public List<CMSCategoriesModels> ListCate { get; set; }
        public string BrandID { get; set; }
        public List<CMSBrandsModels> ListBrand { get; set; }
        public string CollectionID { get; set; }
        public List<CMSCollectionModels> ListCollect { get; set; }
        public int RangeType { get; set; }
        public int TotalProduct { get; set; }
        public bool IsAddMore { get; set; }
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public string CurrentCateAlias { get; set; }
        public string CurrentCategoryName { get; set; }
        public string CurrentSortNew { get; set; }
        public string CurrentSortPrice { get; set; }
        public string Key { get; set; }
        public bool IsOrther { get; set; }
        public ProductViewModels()
        {
            ProductModel = new CMS_ProductsModels();
            ListBanner = new List<CMSBannerModels>();
            BrandModel = new CMSBrandsModels();
            CateModel = new CMSCategoriesModels();
            ListCate = new List<CMSCategoriesModels>();
            ListBrand = new List<CMSBrandsModels>();
            ListCollect = new List<CMSCollectionModels>();
            ListProduct = new List<CMS_ProductsModels>();
            ListProductTopSales = new List<CMS_ProductsModels>();
            ListSameProduct = new List<CMS_ProductsModels>();
            Company = new CMS_CompanyModels();
            ListNews = new List<CMS_NewsModels>();
            ListNewsOld = new List<CMS_NewsModels>();
            ListLocation = new List<CMSLocationModels>();
        }
    }
}
