namespace Xspedition.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDateValueField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ScrubbingInfo", "DateFieldValue", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ScrubbingInfo", "DateFieldValue");
        }
    }
}
