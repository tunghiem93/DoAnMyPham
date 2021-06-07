using CMS_DTO.CMSBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CMS_DTO.CMSLocation
{
    public class CMSLocationModels : CMS_BaseModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên khu vực")]
        [MaxLength(60, ErrorMessage = "Tên khu vực tối đa 250 kí tự")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập nội dung Alias")]
        public string Alias { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mã khu vực")]
        [MaxLength(50, ErrorMessage = "Mã thương hiệu tối đa 50 kí tự")]
        public string LocationCode { get; set; }
        public bool IsActive { get; set; }
        public string Short_Description { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string sStatus { get; set; }
        public int NumberOfProduct { get; set; }

        public CMSLocationModels()
        {
            IsActive = true;
        }
    }
}
