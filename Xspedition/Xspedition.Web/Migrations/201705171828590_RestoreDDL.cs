namespace Xspedition.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RestoreDDL : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CaTimeline",
                c => new
                    {
                        CaTimelineId = c.Int(nullable: false, identity: true),
                        CaId = c.Int(nullable: false),
                        ScrubbingStart = c.DateTime(),
                        ScrubbingTarget = c.DateTime(),
                        ScrubbingCritical = c.DateTime(),
                        NotificationStart = c.DateTime(),
                        NotificationTarget = c.DateTime(),
                        NotificationCritical = c.DateTime(),
                        ResponseStart = c.DateTime(),
                        ResponseTarget = c.DateTime(),
                        ResponseCritical = c.DateTime(),
                        InstructionStart = c.DateTime(),
                        InstructionTarget = c.DateTime(),
                        InstructionCritical = c.DateTime(),
                        PaymentStart = c.DateTime(),
                        PaymentTarget = c.DateTime(),
                        PaymentCritical = c.DateTime(),
                    })
                .PrimaryKey(t => t.CaTimelineId);
            
            CreateTable(
                "dbo.CaTypeFieldMap",
                c => new
                    {
                        CaTypeRegistryId = c.Int(nullable: false),
                        FieldRegistryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CaTypeRegistryId, t.FieldRegistryId })
                .ForeignKey("dbo.CaTypeRegistry", t => t.CaTypeRegistryId, cascadeDelete: true)
                .ForeignKey("dbo.FieldRegistry", t => t.FieldRegistryId, cascadeDelete: true)
                .Index(t => t.CaTypeRegistryId)
                .Index(t => t.FieldRegistryId);
            
            CreateTable(
                "dbo.CaTypeRegistry",
                c => new
                    {
                        CaTypeRegistryId = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CaTypeRegistryId);
            
            CreateTable(
                "dbo.FieldRegistry",
                c => new
                    {
                        FieldRegistryId = c.Int(nullable: false, identity: true),
                        FieldDisplay = c.String(nullable: false),
                        FieldType = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.FieldRegistryId);
            
            CreateTable(
                "dbo.CaTypeDateConfiguration",
                c => new
                    {
                        DateConfigurationId = c.Int(nullable: false, identity: true),
                        CaType = c.Int(nullable: false),
                        ProcessType = c.Int(nullable: false),
                        FieldRegistryId = c.Int(nullable: false),
                        DateOffset = c.Int(nullable: false),
                        DateType = c.String(),
                    })
                .PrimaryKey(t => t.DateConfigurationId);
            
            CreateTable(
                "dbo.InstructionInfo",
                c => new
                    {
                        InstructionInfoId = c.Int(nullable: false, identity: true),
                        CaId = c.Int(nullable: false),
                        CaTypeId = c.Int(),
                        VolManCho = c.String(),
                        FieldDisplay = c.String(),
                        AccountNumber = c.String(),
                        ProcessedDate = c.DateTime(),
                        ProcessedDateCategory = c.Int(nullable: false),
                        IsInstructed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.InstructionInfoId);
            
            CreateTable(
                "dbo.NotificationInfo",
                c => new
                    {
                        NotificationInfoId = c.Int(nullable: false, identity: true),
                        CaId = c.Int(nullable: false),
                        CaTypeId = c.Int(),
                        VolManCho = c.String(),
                        FieldDisplay = c.String(),
                        AccountNumber = c.String(),
                        Recipient = c.String(),
                        ProcessedDate = c.DateTime(),
                        ProcessedDateCategory = c.Int(nullable: false),
                        IsSent = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.NotificationInfoId);
            
            CreateTable(
                "dbo.OptionTypeFieldMap",
                c => new
                    {
                        OptionTypeRegistryId = c.Int(nullable: false),
                        FieldRegistryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.OptionTypeRegistryId, t.FieldRegistryId })
                .ForeignKey("dbo.FieldRegistry", t => t.FieldRegistryId, cascadeDelete: true)
                .ForeignKey("dbo.OptionTypeRegistry", t => t.OptionTypeRegistryId, cascadeDelete: true)
                .Index(t => t.OptionTypeRegistryId)
                .Index(t => t.FieldRegistryId);
            
            CreateTable(
                "dbo.OptionTypeRegistry",
                c => new
                    {
                        OptionTypeRegistryId = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.OptionTypeRegistryId);
            
            CreateTable(
                "dbo.PaymentInfo",
                c => new
                    {
                        PaymentInfoId = c.Int(nullable: false, identity: true),
                        CaId = c.Int(nullable: false),
                        CaTypeId = c.Int(),
                        VolManCho = c.String(),
                        FieldDisplay = c.String(),
                        AccountNumber = c.String(),
                        OptionNumber = c.Int(nullable: false),
                        PayoutNumber = c.Int(nullable: false),
                        ProcessedDate = c.DateTime(),
                        ProcessedDateCategory = c.Int(nullable: false),
                        IsSettled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PaymentInfoId);
            
            CreateTable(
                "dbo.PayoutTypeFieldMap",
                c => new
                    {
                        PayoutTypeRegistryId = c.Int(nullable: false),
                        FieldRegistryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PayoutTypeRegistryId, t.FieldRegistryId })
                .ForeignKey("dbo.FieldRegistry", t => t.FieldRegistryId, cascadeDelete: true)
                .ForeignKey("dbo.PayoutTypeRegistry", t => t.PayoutTypeRegistryId, cascadeDelete: true)
                .Index(t => t.PayoutTypeRegistryId)
                .Index(t => t.FieldRegistryId);
            
            CreateTable(
                "dbo.PayoutTypeRegistry",
                c => new
                    {
                        PayoutTypeRegistryId = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.PayoutTypeRegistryId);
            
            CreateTable(
                "dbo.ResponseInfo",
                c => new
                    {
                        ResponseInfoId = c.Int(nullable: false, identity: true),
                        CaId = c.Int(nullable: false),
                        CaTypeId = c.Int(),
                        VolManCho = c.String(),
                        OptionNumber = c.Int(),
                        AccountNumber = c.String(),
                        FieldDisplay = c.String(),
                        ProcessedDate = c.DateTime(),
                        ProcessedDateCategory = c.Int(nullable: false),
                        IsSubmitted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ResponseInfoId);
            
            CreateTable(
                "dbo.ScrubbingInfo",
                c => new
                    {
                        ScrubbingInfoId = c.Int(nullable: false, identity: true),
                        FieldRegistryId = c.Int(nullable: false),
                        CaId = c.Int(nullable: false),
                        CaTypeId = c.Int(),
                        VolManCho = c.String(),
                        OptionNumber = c.Int(),
                        OptionTypeId = c.Int(),
                        PayoutNumber = c.Int(),
                        PayoutTypeId = c.Int(),
                        FieldDisplay = c.String(),
                        ProcessedDate = c.DateTime(),
                        ProcessedDateCategory = c.Int(nullable: false),
                        IsSrubbed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ScrubbingInfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PayoutTypeFieldMap", "PayoutTypeRegistryId", "dbo.PayoutTypeRegistry");
            DropForeignKey("dbo.PayoutTypeFieldMap", "FieldRegistryId", "dbo.FieldRegistry");
            DropForeignKey("dbo.OptionTypeFieldMap", "OptionTypeRegistryId", "dbo.OptionTypeRegistry");
            DropForeignKey("dbo.OptionTypeFieldMap", "FieldRegistryId", "dbo.FieldRegistry");
            DropForeignKey("dbo.CaTypeFieldMap", "FieldRegistryId", "dbo.FieldRegistry");
            DropForeignKey("dbo.CaTypeFieldMap", "CaTypeRegistryId", "dbo.CaTypeRegistry");
            DropIndex("dbo.PayoutTypeFieldMap", new[] { "FieldRegistryId" });
            DropIndex("dbo.PayoutTypeFieldMap", new[] { "PayoutTypeRegistryId" });
            DropIndex("dbo.OptionTypeFieldMap", new[] { "FieldRegistryId" });
            DropIndex("dbo.OptionTypeFieldMap", new[] { "OptionTypeRegistryId" });
            DropIndex("dbo.CaTypeFieldMap", new[] { "FieldRegistryId" });
            DropIndex("dbo.CaTypeFieldMap", new[] { "CaTypeRegistryId" });
            DropTable("dbo.ScrubbingInfo");
            DropTable("dbo.ResponseInfo");
            DropTable("dbo.PayoutTypeRegistry");
            DropTable("dbo.PayoutTypeFieldMap");
            DropTable("dbo.PaymentInfo");
            DropTable("dbo.OptionTypeRegistry");
            DropTable("dbo.OptionTypeFieldMap");
            DropTable("dbo.NotificationInfo");
            DropTable("dbo.InstructionInfo");
            DropTable("dbo.CaTypeDateConfiguration");
            DropTable("dbo.FieldRegistry");
            DropTable("dbo.CaTypeRegistry");
            DropTable("dbo.CaTypeFieldMap");
            DropTable("dbo.CaTimeline");
        }
    }
}
