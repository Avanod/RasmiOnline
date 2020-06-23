namespace RasmiOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSurvey : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Base.Survey",
                c => new
                    {
                        SurveyId = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        InsertDateMi = c.DateTime(nullable: false),
                        InsertDateSh = c.String(maxLength: 10, fixedLength: true, unicode: false),
                        Subject = c.String(nullable: false, maxLength: 70),
                        Text = c.String(),
                    })
                .PrimaryKey(t => t.SurveyId);
            
            CreateTable(
                "Base.SurveyOption",
                c => new
                    {
                        SurveyOptionId = c.Int(nullable: false, identity: true),
                        SurveyId = c.Int(nullable: false),
                        SelectedOption = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        InsertDateMi = c.DateTime(nullable: false),
                        InsertDateSh = c.String(maxLength: 10, fixedLength: true, unicode: false),
                        Text = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.SurveyOptionId)
                .ForeignKey("Base.Survey", t => t.SurveyId, cascadeDelete: true)
                .Index(t => t.SurveyId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Base.SurveyOption", "SurveyId", "Base.Survey");
            DropIndex("Base.SurveyOption", new[] { "SurveyId" });
            DropTable("Base.SurveyOption");
            DropTable("Base.Survey");
        }
    }
}
