using MISA.SME.Report.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.SME.Report.Models
{
    public class MessageReceive : GeneralLedgerController
    {
        public MessageReceive(ApplicationDbContext dbContext) :base(dbContext)
        {
            DbContext = dbContext;
        }

        private new ApplicationDbContext _dbContext;
        public string Label { get; set; }
        public object Receipt { get; set; }
        public ApplicationDbContext DbContext { get => _dbContext; set => _dbContext = value; }
    }
}
