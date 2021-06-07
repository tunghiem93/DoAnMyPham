using CMS_Entity.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Entity.Mapping
{
    public class CMS_CustomerInforMap : EntityTypeConfiguration<CMS_CustomersInfor>
    {
        public CMS_CustomerInforMap()
        {
            this.HasKey(x => x.Id);
            this.Property(x => x.Id).HasMaxLength(60).HasColumnType("varchar");
            this.Property(x => x.Name).HasMaxLength(250).IsOptional().HasColumnType("nvarchar");
            this.Property(x => x.Email).HasMaxLength(250).IsOptional().HasColumnType("varchar");
            this.Property(x => x.CustomerId).HasMaxLength(60).IsOptional().HasColumnType("nvarchar");
            this.Property(x => x.ReceiveType).HasColumnType("int").IsOptional();
            this.Property(x => x.Phone).HasMaxLength(15).IsOptional().HasColumnType("varchar");
            this.Property(x => x.ZipCode).HasMaxLength(15).IsOptional().HasColumnType("nvarchar");
            this.Property(x => x.PreferredtDate).IsOptional();
            this.Property(x => x.PreferredtTime).IsOptional().HasColumnType("Time");
            this.Property(x => x.PriceAmong).HasColumnType("decimal").IsOptional();
            this.Property(x => x.FinancingRequired).HasColumnType("int").IsOptional();
            this.Property(x => x.EmailFriend).HasMaxLength(250).IsOptional().HasColumnType("varchar");
            this.Property(x => x.Message).HasMaxLength(1000).IsOptional().HasColumnType("nvarchar");
            this.Property(x => x.Subject).HasMaxLength(250).IsOptional().HasColumnType("nvarchar");
            this.Property(x => x.UpdatedBy).HasMaxLength(60).IsOptional().HasColumnType("varchar");
            this.Property(x => x.CreatedBy).HasMaxLength(60).IsOptional().HasColumnType("varchar");
            this.Property(x => x.UpdatedDate).IsOptional();
        }
    }
}
