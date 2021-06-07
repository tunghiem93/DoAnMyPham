using CMS_DTO.CMSBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CMS_DTO.CMSCollection
{
    public class CMSCollectionModels : CMS_BaseModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên")]
        [MaxLength(60, ErrorMessage = "Tên thể loại tối đa 250 kí tự")]
        public string CollectionName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập nội dung Alias")]
        public string Alias { get; set; }
        public bool IsActive { get; set; }
        public string Link { get; set; }
        public int TypeLink { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string sStatus { get; set; }
        public List<string> ListImageUrl { get; set; }
        public List<ImageCollection> ListImg { get; set; }
        public string RawImageUrl { get; set; }
        public CMSCollectionModels()
        {
            ListImg = new List<ImageCollection>();
            IsActive = true;
            ListImageUrl = new List<string>();
        }
    }

    public class ImageCollection
    {
        public int OffSet { get; set; }
        public bool IsDelete { get; set; }
        public string ImageURL { get; set; }
        [DataType(DataType.Upload)]
        public HttpPostedFileBase PictureUpload { get; set; }
        public byte[] PictureByte { get; set; }
    }
}
