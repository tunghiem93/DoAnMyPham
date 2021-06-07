using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Entity.Entity
{
    public class CMS_Collections : CMS_EntityBase
    {
        public string Id { get; set; }
        public string CollectionName { get; set; }
        public string Alias { get; set; }
        public string Link { get; set; }
        public int TypeLink { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public virtual List<CMS_Images> Images { get; set; }
        //public virtual List<CMS_Products> Products { get; set; }
    }
}
