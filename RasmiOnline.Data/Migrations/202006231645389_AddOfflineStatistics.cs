namespace RasmiOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOfflineStatistics : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Statistic.OfflineStatistics",
                c => new
                    {
                        OfflineStatisticsId = c.Int(nullable: false, identity: true),
                        Type = c.Byte(nullable: false),
                        Value = c.Int(nullable: false),
                        InsertDateMi = c.DateTime(nullable: false),
                        InsertDateSh = c.String(maxLength: 10, fixedLength: true, unicode: false),
                        Name = c.String(nullable: false, maxLength: 30),
                        Description = c.String(nullable: false, maxLength: 40),
                        ExtraData = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.OfflineStatisticsId)
                .Index(t => t.Type, unique: true);
            
        }
        
        public override void Down()
        {
            DropIndex("Statistic.OfflineStatistics", new[] { "Type" });
            DropTable("Statistic.OfflineStatistics");
        }
    }
}
