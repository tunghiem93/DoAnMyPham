using CMS_DTO.CMSBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CMS_DTO.CMSPage
{
    public class CMS_PageModes : CMS_BaseModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tiêu đề trang")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập nội dung Alias")]
        public string Alias { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public int Type { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string sStatus { get; set; }
    }
}
