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
        public DbSet<SCC_SITATEX> SITATEX_FILES { get; set; }
        public DbSet<DestinationTypeB> Destinations { get; set; }
        public DbSet<SchedulesMessage> schedulesMessages { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=SCC;Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<SCC_SITATEX>()
            //    .HasIndex(s => s.MessageId)
            //    .IsUnique();
        }
    }
}
