using DosTranV2.DBModel.Model;
using System.Data.Entity;

namespace DosTranV2.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(string connString): base(connString)
        {

        }
        public DbSet<DOSTRAN_LOG> DOSTRAN_LOG { get; set; }
        public DbSet<DOSTRAN_VERSION> DOSTRAN_VERSION { get; set; }
        public DbSet<DOSTRAN_UPLOAD> DOSTRAN_UPLOAD { get; set; }
        public DbSet<DOSTRAN_DOWNLOAD> DOSTRAN_DOWNLOAD { get; set; }
    }
}
