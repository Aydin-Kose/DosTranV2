namespace DosTranV2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dostran_v1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DOSTRAN_DOWNLOAD", "OpID", c => c.String(maxLength: 10));
            AlterColumn("dbo.DOSTRAN_LOG", "OpID", c => c.String(maxLength: 10));
            AlterColumn("dbo.DOSTRAN_UPLOAD", "OpID", c => c.String(maxLength: 10));
            AlterColumn("dbo.DOSTRAN_VERSION", "Version", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DOSTRAN_VERSION", "Version", c => c.String(maxLength: 10, fixedLength: true, unicode: false));
            AlterColumn("dbo.DOSTRAN_UPLOAD", "OpID", c => c.String(maxLength: 10, fixedLength: true, unicode: false));
            AlterColumn("dbo.DOSTRAN_LOG", "OpID", c => c.String(maxLength: 10, fixedLength: true, unicode: false));
            AlterColumn("dbo.DOSTRAN_DOWNLOAD", "OpID", c => c.String(maxLength: 10, fixedLength: true, unicode: false));
        }
    }
}
