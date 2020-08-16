namespace RasmiOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSmsTemplateEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Base.SmsTemplate",
                c => new
                    {
                        SmsTemplateId = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        InsertDateMi = c.DateTime(nullable: false),
                        InsertDateSh = c.String(maxLength: 10, fixedLength: true, unicode: false),
                        Key = c.String(nullable: false, maxLength: 50, unicode: false),
                        Title = c.String(nullable: false, maxLength: 70),
                        Text = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.SmsTemplateId);
            
        }
        
        public override void Down()
        {
            DropTable("Base.SmsTemplate");
        }
    }
}
