namespace DosTranV2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dostran_v2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DOSTRAN_DOWNLOAD",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OpID = c.String(maxLength: 10, fixedLength: true, unicode: false),
                        DataSet = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DOSTRAN_LOG",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OpID = c.String(maxLength: 10, fixedLength: true, unicode: false),
                        DataSet = c.String(),
                        TarihSaat = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DOSTRAN_UPLOAD",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OpID = c.String(maxLength: 10, fixedLength: true, unicode: false),
                        DataSet = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DOSTRAN_VERSION",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Version = c.String(maxLength: 10, fixedLength: true, unicode: false),
                        Link = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DOSTRAN_VERSION");
            DropTable("dbo.DOSTRAN_UPLOAD");
            DropTable("dbo.DOSTRAN_LOG");
            DropTable("dbo.DOSTRAN_DOWNLOAD");
        }
    }
}
