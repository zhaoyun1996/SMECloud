using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MISA.SME.Report.Models
{
    public class ReceiptDetail
    {
        [Key]
        public Guid ObjectID { get; set; }

        public string RefDetailID { get; set; }

        public Guid RefObjectID { get; set; }

        public Guid RefDetailUnitID { get; set; }

        public string RefDetailDescription { get; set; }

        public decimal? RefDetailAmount { get; set; }

        public decimal? RefDetailAmountOC { get; set; }

        public string RefDetailConstruction { get; set; }

        public string RefDetailSaleContract { get; set; }

        public string RefDetailStatisticsCode { get; set; }

        public decimal? RefDetailDebtAccount { get; set; }

        public decimal? RefDetailAccountAvailable { get; set; }

        [NotMapped]
        public Unit Unit { get; set; }
    }
}
