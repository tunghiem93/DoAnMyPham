using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_DTO.CMSSendMail
{
    public class SendMailModels
    {
        public string MailTo { get; set; }
        public string MailFrom { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
