using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Entity.Entity
{
    [Table("Promotions")]
    public class PromotionEntities : CMS_EntityBase
    {
        public string Id { get; set; }
        public bool IsDeleted { get; set; }
        public string PromotionCode { get; set; }
        public decimal Value { get; set; }
    }
}
