using CMS_DTO.CMSBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CMS_DTO.CMSBanner
{
    public class CMSBannerModels : CMS_BaseModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên thương hiệu")]
        [MaxLength(60, ErrorMessage = "Tên thể loại tối đa 250 kí tự")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập nội dung Alias")]
        public string Alias { get; set; }
        public string Link { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string sStatus { get; set; }

        public CMSBannerModels()
        {
            IsActive = true;
        }
    }
}
