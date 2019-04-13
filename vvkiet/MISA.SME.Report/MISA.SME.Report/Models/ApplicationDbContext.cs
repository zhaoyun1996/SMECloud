using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
//using MISA.SME.ReceiptAndPayment.RabbitMQ;
using MISA.SME.Report.RabbitMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.SME.Report.Models
{
    public class ApplicationDbContext : DbContext
    {
        private static ApplicationDbContext instace;

        public static ApplicationDbContext Instace
        {
            get
            {
                if (instace == null)
                    instace = new ApplicationDbContext();
                return instace;
            }
            private set
            {
                instace = value;
            }
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(Setting.ConnectionString);
            }
        }

        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<DebitMonth> DebitMonth { get; set; }
        public DbSet<DebitPrecious> DebitPrecious { get; set; }
        public DbSet<GeneralLedger> GeneralLedger { get; set; }
    }
}
