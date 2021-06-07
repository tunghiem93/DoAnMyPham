using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Entity.Entity
{
    public class CMS_CustomersInfor : CMS_EntityBase
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ZipCode { get; set; }
        public int ReceiveType { get; set; }
        public DateTime PreferredtDate { get; set; }
        public TimeSpan PreferredtTime { get; set; }
        public decimal PriceAmong { get; set; }
        public int FinancingRequired { get; set; }
        public string EmailFriend { get; set; }
        public string Message { get; set; }
        public bool Checked { get; set; }
        public bool CheckedMail { get; set; }
        public bool CheckedPhone { get; set; }
        public string Subject { get; set; }
    }
}
