namespace DosTranV2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dostran_v3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DOSTRAN_LOG", "IslemTipi", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DOSTRAN_LOG", "IslemTipi");
        }
    }
}
