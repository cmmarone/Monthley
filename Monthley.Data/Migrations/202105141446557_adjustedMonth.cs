namespace Monthley.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adjustedMonth : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Month", "BeginDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Month", "MonthNum");
            DropColumn("dbo.Month", "YearNum");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Month", "YearNum", c => c.Int(nullable: false));
            AddColumn("dbo.Month", "MonthNum", c => c.Int(nullable: false));
            DropColumn("dbo.Month", "BeginDate");
        }
    }
}
