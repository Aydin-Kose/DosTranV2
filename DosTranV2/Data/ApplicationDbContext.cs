using DosTranV2.Data.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace DosTranV2.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {

        }
        public DbSet<DOSTRAN_LOG> DOSTRAN_LOG { get; set; }
        public DbSet<DOSTRAN_VERSION> DOSTRAN_VERSION { get; set; }
        public DbSet<DOSTRAN_UPLOAD> DOSTRAN_UPLOAD { get; set; }
        public DbSet<DOSTRAN_DOWNLOAD> DOSTRAN_DOWNLOAD { get; set; }
    }
}
