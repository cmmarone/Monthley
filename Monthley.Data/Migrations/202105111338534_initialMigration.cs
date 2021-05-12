namespace Monthley.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Type = c.Int(nullable: false),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Expense",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExpenseFreqType = c.Int(nullable: false),
                        FrequencyFactor = c.Int(nullable: false),
                        InitialDueDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Category", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.DueDate",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExpenseId = c.Int(nullable: false),
                        MonthId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Expense", t => t.ExpenseId, cascadeDelete: true)
                .ForeignKey("dbo.Month", t => t.MonthId, cascadeDelete: true)
                .Index(t => t.ExpenseId)
                .Index(t => t.MonthId);
            
            CreateTable(
                "dbo.Month",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MonthNum = c.Int(nullable: false),
                        YearNum = c.Int(nullable: false),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PayDay",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IncomeId = c.Int(nullable: false),
                        MonthId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Income", t => t.IncomeId, cascadeDelete: true)
                .ForeignKey("dbo.Month", t => t.MonthId, cascadeDelete: true)
                .Index(t => t.IncomeId)
                .Index(t => t.MonthId);
            
            CreateTable(
                "dbo.Income",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PayFreqType = c.Int(nullable: false),
                        FrequencyFactor = c.Int(nullable: false),
                        InitialPayDate = c.DateTime(nullable: false),
                        LastPayDate = c.DateTime(nullable: false),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Source", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Source",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Type = c.Int(nullable: false),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PaymentReceived",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SourceId = c.Int(nullable: false),
                        MonthId = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaymentDate = c.DateTime(nullable: false),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Month", t => t.MonthId, cascadeDelete: true)
                .ForeignKey("dbo.Source", t => t.SourceId, cascadeDelete: true)
                .Index(t => t.SourceId)
                .Index(t => t.MonthId);
            
            CreateTable(
                "dbo.PaymentMade",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoryId = c.Int(nullable: false),
                        MonthId = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaymentDate = c.DateTime(nullable: false),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Category", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Month", t => t.MonthId, cascadeDelete: true)
                .Index(t => t.CategoryId)
                .Index(t => t.MonthId);
            
            CreateTable(
                "dbo.IdentityRole",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IdentityUserRole",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(),
                        IdentityRole_Id = c.String(maxLength: 128),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.IdentityRole", t => t.IdentityRole_Id)
                .ForeignKey("dbo.ApplicationUser", t => t.ApplicationUser_Id)
                .Index(t => t.IdentityRole_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.ApplicationUser",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IdentityUserClaim",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUser", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.IdentityUserLogin",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(),
                        ProviderKey = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.ApplicationUser", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IdentityUserRole", "ApplicationUser_Id", "dbo.ApplicationUser");
            DropForeignKey("dbo.IdentityUserLogin", "ApplicationUser_Id", "dbo.ApplicationUser");
            DropForeignKey("dbo.IdentityUserClaim", "ApplicationUser_Id", "dbo.ApplicationUser");
            DropForeignKey("dbo.IdentityUserRole", "IdentityRole_Id", "dbo.IdentityRole");
            DropForeignKey("dbo.DueDate", "MonthId", "dbo.Month");
            DropForeignKey("dbo.PaymentMade", "MonthId", "dbo.Month");
            DropForeignKey("dbo.PaymentMade", "CategoryId", "dbo.Category");
            DropForeignKey("dbo.PayDay", "MonthId", "dbo.Month");
            DropForeignKey("dbo.PayDay", "IncomeId", "dbo.Income");
            DropForeignKey("dbo.Income", "Id", "dbo.Source");
            DropForeignKey("dbo.PaymentReceived", "SourceId", "dbo.Source");
            DropForeignKey("dbo.PaymentReceived", "MonthId", "dbo.Month");
            DropForeignKey("dbo.DueDate", "ExpenseId", "dbo.Expense");
            DropForeignKey("dbo.Expense", "Id", "dbo.Category");
            DropIndex("dbo.IdentityUserLogin", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserClaim", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserRole", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserRole", new[] { "IdentityRole_Id" });
            DropIndex("dbo.PaymentMade", new[] { "MonthId" });
            DropIndex("dbo.PaymentMade", new[] { "CategoryId" });
            DropIndex("dbo.PaymentReceived", new[] { "MonthId" });
            DropIndex("dbo.PaymentReceived", new[] { "SourceId" });
            DropIndex("dbo.Income", new[] { "Id" });
            DropIndex("dbo.PayDay", new[] { "MonthId" });
            DropIndex("dbo.PayDay", new[] { "IncomeId" });
            DropIndex("dbo.DueDate", new[] { "MonthId" });
            DropIndex("dbo.DueDate", new[] { "ExpenseId" });
            DropIndex("dbo.Expense", new[] { "Id" });
            DropTable("dbo.IdentityUserLogin");
            DropTable("dbo.IdentityUserClaim");
            DropTable("dbo.ApplicationUser");
            DropTable("dbo.IdentityUserRole");
            DropTable("dbo.IdentityRole");
            DropTable("dbo.PaymentMade");
            DropTable("dbo.PaymentReceived");
            DropTable("dbo.Source");
            DropTable("dbo.Income");
            DropTable("dbo.PayDay");
            DropTable("dbo.Month");
            DropTable("dbo.DueDate");
            DropTable("dbo.Expense");
            DropTable("dbo.Category");
        }
    }
}
