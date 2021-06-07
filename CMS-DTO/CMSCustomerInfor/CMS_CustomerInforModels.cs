using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_DTO.CMSCustomerInfor
{
    public class CMS_CustomerInforModels
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
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public string sStatus { get; set; }
        public string Body { get; set; }

        public CMS_CustomerInforModels()
        {
            IsActive = true;
        }
    }
}
