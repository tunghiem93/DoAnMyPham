using CMS_Entity.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Entity.Mapping
{
    class CMS_BannersMap : EntityTypeConfiguration<CMS_Banners>
    {
        public CMS_BannersMap()
        {
            this.HasKey(x => x.Id);
            this.Property(x => x.Id).HasMaxLength(60).HasColumnType("varchar");
            this.Property(x => x.Name).HasMaxLength(250).IsRequired().HasColumnType("nvarchar");
            this.Property(x => x.Alias).HasMaxLength(250).IsRequired().HasColumnType("nvarchar");
            this.Property(x => x.Link).HasMaxLength(250).HasColumnType("varchar").IsOptional();
            this.Property(x => x.UpdatedBy).HasMaxLength(60).IsOptional().HasColumnType("varchar");
            this.Property(x => x.CreatedBy).HasMaxLength(60).IsOptional().HasColumnType("varchar");
            this.Property(x => x.UpdatedDate).IsOptional();
            this.Property(x => x.ImageURL).IsOptional().HasColumnType("varchar").HasMaxLength(60);
        }
    }
}
