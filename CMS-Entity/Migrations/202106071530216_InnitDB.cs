namespace CMS_Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InnitDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CMS_Banners",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 60, unicode: false),
                        Name = c.String(nullable: false, maxLength: 250),
                        Alias = c.String(nullable: false, maxLength: 250),
                        Link = c.String(maxLength: 250, unicode: false),
                        ImageURL = c.String(maxLength: 60, unicode: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(maxLength: 60, unicode: false),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 60, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CMS_Brands",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 60, unicode: false),
                        BrandName = c.String(nullable: false, maxLength: 250),
                        Alias = c.String(nullable: false, maxLength: 250),
                        Title = c.String(),
                        BrandCode = c.String(nullable: false, maxLength: 50, unicode: false),
                        Short_Description = c.String(maxLength: 2000),
                        Description = c.String(storeType: "ntext"),
                        ImageURL = c.String(maxLength: 60, unicode: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(maxLength: 60, unicode: false),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 60, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CMS_Products",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 60, unicode: false),
                        ProductCode = c.String(nullable: false, maxLength: 50, unicode: false),
                        ProductName = c.String(nullable: false, maxLength: 250),
                        Short_Description = c.String(maxLength: 2000),
                        Description = c.String(storeType: "ntext"),
                        Information = c.String(maxLength: 2000),
                        Vendor = c.String(maxLength: 1000),
                        LinkVideo = c.String(maxLength: 500),
                        TypeSize = c.Int(),
                        TypeState = c.Int(),
                        ProductPrice = c.Decimal(precision: 18, scale: 2),
                        ProductExtraPrice = c.Decimal(precision: 18, scale: 2),
                        CategoryId = c.String(nullable: false, maxLength: 60, unicode: false),
                        BrandId = c.String(nullable: false, maxLength: 60, unicode: false),
                        LocationId = c.String(nullable: false, maxLength: 60, unicode: false),
                        ImageURL = c.String(maxLength: 60, unicode: false),
                        Alias = c.String(nullable: false, maxLength: 250),
                        Year = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(maxLength: 60, unicode: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 60, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CMS_Brands", t => t.BrandId, cascadeDelete: true)
                .ForeignKey("dbo.CMS_Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.CMS_Locations", t => t.LocationId, cascadeDelete: true)
                .Index(t => t.CategoryId)
                .Index(t => t.BrandId)
                .Index(t => t.LocationId);
            
            CreateTable(
                "dbo.CMS_Categories",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 60, unicode: false),
                        CategoryName = c.String(nullable: false, maxLength: 250),
                        Alias = c.String(nullable: false, maxLength: 250),
                        CategoryCode = c.String(nullable: false, maxLength: 50, unicode: false),
                        Short_Description = c.String(maxLength: 2000),
                        Description = c.String(storeType: "ntext"),
                        ImageURL = c.String(maxLength: 60, unicode: false),
                        ParentId = c.String(maxLength: 60, unicode: false),
                        Type = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(maxLength: 60, unicode: false),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 60, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CMS_Images",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 60, unicode: false),
                        ImageURL = c.String(maxLength: 60, unicode: false),
                        ProductId = c.String(maxLength: 60, unicode: false),
                        CategoryId = c.String(maxLength: 60, unicode: false),
                        CollectionId = c.String(maxLength: 60, unicode: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(maxLength: 60, unicode: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 60, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CMS_Categories", t => t.CategoryId)
                .ForeignKey("dbo.CMS_Collections", t => t.CollectionId)
                .ForeignKey("dbo.CMS_Products", t => t.ProductId)
                .Index(t => t.ProductId)
                .Index(t => t.CategoryId)
                .Index(t => t.CollectionId);
            
            CreateTable(
                "dbo.CMS_Collections",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 60, unicode: false),
                        CollectionName = c.String(nullable: false, maxLength: 250),
                        Alias = c.String(nullable: false, maxLength: 250),
                        Link = c.String(maxLength: 100, unicode: false),
                        TypeLink = c.Int(),
                        Description = c.String(storeType: "ntext"),
                        ImageURL = c.String(maxLength: 60, unicode: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(maxLength: 60, unicode: false),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 60, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CMS_Locations",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 60, unicode: false),
                        Name = c.String(nullable: false, maxLength: 250),
                        Alias = c.String(nullable: false, maxLength: 250),
                        Title = c.String(),
                        LocationCode = c.String(nullable: false, maxLength: 50, unicode: false),
                        Short_Description = c.String(maxLength: 2000),
                        Description = c.String(storeType: "ntext"),
                        ImageURL = c.String(maxLength: 60, unicode: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(maxLength: 60, unicode: false),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 60, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CMS_Companies",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 60, unicode: false),
                        Name = c.String(nullable: false, maxLength: 250),
                        Alias = c.String(maxLength: 250),
                        Description = c.String(nullable: false, maxLength: 2000),
                        Email = c.String(nullable: false, maxLength: 250),
                        Phone = c.String(nullable: false, maxLength: 250),
                        Address = c.String(nullable: false, maxLength: 250),
                        LinkBlog = c.String(maxLength: 250, unicode: false),
                        LinkFB = c.String(maxLength: 250, unicode: false),
                        LinkTwiter = c.String(maxLength: 250, unicode: false),
                        LinkInstagram = c.String(maxLength: 250, unicode: false),
                        ImageURL = c.String(maxLength: 60, unicode: false),
                        BusinessHour = c.String(maxLength: 250),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(maxLength: 60, unicode: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 60, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CMS_Customers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 60, unicode: false),
                        FbID = c.String(maxLength: 60, unicode: false),
                        GoogleID = c.String(maxLength: 60, unicode: false),
                        CompanyName = c.String(maxLength: 250),
                        Alias = c.String(maxLength: 250),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 100),
                        Email = c.String(),
                        Password = c.String(nullable: false, maxLength: 250, unicode: false),
                        Phone = c.String(maxLength: 15, unicode: false),
                        BirthDate = c.DateTime(nullable: false),
                        Gender = c.Boolean(nullable: false),
                        Address = c.String(nullable: false, maxLength: 250),
                        MaritalStatus = c.Boolean(nullable: false),
                        Street = c.String(maxLength: 250),
                        City = c.String(),
                        Country = c.String(),
                        AnswerType = c.Int(),
                        AnwersSecurity = c.String(maxLength: 500),
                        ZipCode = c.String(maxLength: 15),
                        Description = c.String(),
                        ImageURL = c.String(),
                        Status = c.Byte(nullable: false),
                        UserLogin = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(maxLength: 60, unicode: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 60, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CMS_CustomersInfor",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 60, unicode: false),
                        CustomerId = c.String(maxLength: 60),
                        Name = c.String(maxLength: 250),
                        Email = c.String(maxLength: 250, unicode: false),
                        Phone = c.String(maxLength: 15, unicode: false),
                        ZipCode = c.String(maxLength: 15),
                        ReceiveType = c.Int(),
                        PreferredtDate = c.DateTime(),
                        PreferredtTime = c.Time(precision: 7),
                        PriceAmong = c.Decimal(precision: 18, scale: 2),
                        FinancingRequired = c.Int(),
                        EmailFriend = c.String(maxLength: 250, unicode: false),
                        Message = c.String(maxLength: 1000),
                        Checked = c.Boolean(nullable: false),
                        CheckedMail = c.Boolean(nullable: false),
                        CheckedPhone = c.Boolean(nullable: false),
                        Subject = c.String(maxLength: 250),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(maxLength: 60, unicode: false),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 60, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CMS_Employee",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 60, unicode: false),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 20),
                        Alias = c.String(maxLength: 250, unicode: false),
                        Level = c.String(),
                        Employee_Address = c.String(maxLength: 250),
                        Employee_Phone = c.String(nullable: false, maxLength: 11, unicode: false),
                        Employee_Email = c.String(nullable: false, maxLength: 250, unicode: false),
                        Employee_IDCard = c.String(maxLength: 50, unicode: false),
                        BirthDate = c.DateTime(),
                        Password = c.String(nullable: false, maxLength: 250, unicode: false),
                        LinkBlog = c.String(maxLength: 250, unicode: false),
                        LinkFB = c.String(maxLength: 250, unicode: false),
                        LinkTwiter = c.String(maxLength: 250, unicode: false),
                        LinkInstagram = c.String(maxLength: 250, unicode: false),
                        ImageURL = c.String(maxLength: 60, unicode: false),
                        IsSupperAdmin = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(maxLength: 60, unicode: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 60, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CMS_News",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 60, unicode: false),
                        Title = c.String(nullable: false, maxLength: 250),
                        Alias = c.String(nullable: false, maxLength: 250),
                        Publisher = c.String(maxLength: 100),
                        Type = c.Int(),
                        Short_Description = c.String(nullable: false, maxLength: 2000),
                        Description = c.String(storeType: "ntext"),
                        ImageURL = c.String(maxLength: 60, unicode: false),
                        ImageURLAuthor = c.String(maxLength: 60, unicode: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(maxLength: 60, unicode: false),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 60, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CMS_Pages",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 60, unicode: false),
                        Name = c.String(nullable: false, maxLength: 250),
                        Alias = c.String(nullable: false, maxLength: 250),
                        Description = c.String(storeType: "ntext"),
                        Type = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(maxLength: 60, unicode: false),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 60, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CMS_Policy",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 60, unicode: false),
                        Description = c.String(storeType: "ntext"),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(maxLength: 60, unicode: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 60, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CMS_Products", "LocationId", "dbo.CMS_Locations");
            DropForeignKey("dbo.CMS_Products", "CategoryId", "dbo.CMS_Categories");
            DropForeignKey("dbo.CMS_Images", "ProductId", "dbo.CMS_Products");
            DropForeignKey("dbo.CMS_Images", "CollectionId", "dbo.CMS_Collections");
            DropForeignKey("dbo.CMS_Images", "CategoryId", "dbo.CMS_Categories");
            DropForeignKey("dbo.CMS_Products", "BrandId", "dbo.CMS_Brands");
            DropIndex("dbo.CMS_Images", new[] { "CollectionId" });
            DropIndex("dbo.CMS_Images", new[] { "CategoryId" });
            DropIndex("dbo.CMS_Images", new[] { "ProductId" });
            DropIndex("dbo.CMS_Products", new[] { "LocationId" });
            DropIndex("dbo.CMS_Products", new[] { "BrandId" });
            DropIndex("dbo.CMS_Products", new[] { "CategoryId" });
            DropTable("dbo.CMS_Policy");
            DropTable("dbo.CMS_Pages");
            DropTable("dbo.CMS_News");
            DropTable("dbo.CMS_Employee");
            DropTable("dbo.CMS_CustomersInfor");
            DropTable("dbo.CMS_Customers");
            DropTable("dbo.CMS_Companies");
            DropTable("dbo.CMS_Locations");
            DropTable("dbo.CMS_Collections");
            DropTable("dbo.CMS_Images");
            DropTable("dbo.CMS_Categories");
            DropTable("dbo.CMS_Products");
            DropTable("dbo.CMS_Brands");
            DropTable("dbo.CMS_Banners");
        }
    }
}
