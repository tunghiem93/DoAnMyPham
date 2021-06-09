using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_DTO.Promotions
{
    public class PromotionDTO
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Thông tin bắt buộc.")]
        public string PromotionCode { get; set; }
        [Required(ErrorMessage = "Thông tin bắt buộc.")]
        public decimal Value { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public string sStatus { get; set; }
    }
}
