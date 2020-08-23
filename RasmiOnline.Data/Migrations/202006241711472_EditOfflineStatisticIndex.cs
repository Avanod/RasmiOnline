namespace RasmiOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditOfflineStatisticIndex : DbMigration
    {
        public override void Up()
        {
            DropIndex("Statistic.OfflineStatistics", new[] { "Type" });
        }
        
        public override void Down()
        {
            CreateIndex("Statistic.OfflineStatistics", "Type", unique: true);
        }
    }
}
