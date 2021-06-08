using CMS_DTO.CMSImage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CMS_DTO.CMSProduct
{
    public class CMS_ProductsModels
    {
        public string Id { get; set; }
        [Required(ErrorMessage ="Vui lòng nhập mã sản phẩm")]
        [MaxLength(50,ErrorMessage ="Mã sản phẩm tối đa 50 kí tự")]
        public string ProductCode { get; set; }
        [Required(ErrorMessage ="Vui lòng nhập tên sản phẩm")]
        [MaxLength(250,ErrorMessage ="Tên sản phẩm tối đa 250 kí tự")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập nội dung Alias")]
        public string Alias { get; set; }
        [Required(ErrorMessage ="Vui lòng nhập giá sản phẩm")]
        [Range(0,Int64.MaxValue,ErrorMessage ="Giá sản phẩm phải lớn hơn 0")]
        public decimal ProductPrice { get; set; }
        public decimal ProductExtraPrice { get; set; }
        [Required(ErrorMessage ="Vui lòng chọn thể loại")]
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int CategoryType { get; set; }
        //[Required(ErrorMessage = "Vui lòng chọn thương hiệu")]
        public string BrandId { get; set; }
        public string BrandName { get; set; }
        //[Required(ErrorMessage = "Vui lòng chọn khu vực")]
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public string StyleId { get; set; }
        public string StyleName { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public string AliasLoca { get; set; }
        public string AliasCate { get; set; }
        public string AliasBrand { get; set; }
        public string AliasStyle { get; set; }
        public string sStatus { get; set; }
        public string LinkVideo { get; set; }

        public string ImageURL { get; set; }
        [DataType(DataType.Upload)]
        public HttpPostedFileBase PictureUpload { get; set; }
        public byte[] PictureByte { get; set; }
        
        public int TypeSize { get; set; }
        public int TypeState { get; set; }
        public string Short_Description { get; set; }
        public string Information { get; set; }
        public string Vendor { get; set; }
        public int Year { get; set; }
        //Image
        public List<string> ListImageUrl { get; set; }
        public List<ImageProduct> ListImg { get; set; }
        public string RawImageUrl { get; set; }
        public string sPrice { get; set; }

        public CMS_ProductsModels()
        {
            Year = DateTime.Now.Year;
            IsActive = true;
            ProductPrice = 0;
            ProductExtraPrice = 0;
            ListImageUrl = new List<string>();
            ListImg = new List<ImageProduct>();
        }
    }

    public class ImageProduct
    {
        public int OffSet { get; set; }
        public bool IsDelete { get; set; }
        public string ImageURL { get; set; }
        [DataType(DataType.Upload)]
        public HttpPostedFileBase PictureUpload { get; set; }
        public byte[] PictureByte { get; set; }
    }
}
