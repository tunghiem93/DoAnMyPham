using CMS_Entity.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Entity.Mapping
{
    public class CMS_PagesMap : EntityTypeConfiguration<CMS_Pages>
    {
        public CMS_PagesMap()
        {
            this.HasKey(x => x.Id);
            this.Property(x => x.Id).HasMaxLength(60).HasColumnType("varchar");
            this.Property(x => x.Name).HasMaxLength(250).IsRequired().HasColumnType("nvarchar");
            this.Property(x => x.Alias).HasMaxLength(250).IsRequired().HasColumnType("nvarchar");
            this.Property(x => x.Description).HasColumnType("ntext").IsOptional();
            this.Property(x => x.Type).HasColumnType("int").IsOptional();
            this.Property(x => x.UpdatedBy).HasMaxLength(60).IsOptional().HasColumnType("varchar");
            this.Property(x => x.CreatedBy).HasMaxLength(60).IsOptional().HasColumnType("varchar");
            this.Property(x => x.UpdatedDate).IsOptional();
        }
    }
}
