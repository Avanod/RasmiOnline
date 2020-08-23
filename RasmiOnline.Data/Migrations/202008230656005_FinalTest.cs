namespace RasmiOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FinalTest : DbMigration
    {
        public override void Up()
        {
            AddColumn("Base.SmsTemplate", "MessagingType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Base.SmsTemplate", "MessagingType");
        }
    }
}
