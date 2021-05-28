using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadFiles.Data
{
    public class SCCContext : DbContext
    {
        public DbSet<SC_SITATEX> SC_SITATEXes { get; set; }
        public DbSet<SCSubMessage> SCSubMessages { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=SCC;Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<SCC_SITATEX>()
            //    .HasIndex(s => s.MessageId)
            //    .IsUnique();
            builder.Entity<SC_SITATEX>().HasIndex(s => s.FileName).IsUnique();
        }
    }
}
