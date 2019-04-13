using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.SME.Report.Models
{
    public class DebitMonth
    {
        [Key]
        public Guid ObjectID { get; set; }

        public Guid MonthID { get; set; }

        public string MonthName { get; set; }

        public Guid AccountObjectID { get; set; }

        public string AccountObjectName { get; set; }

        public float Debit { get; set; }

        public float Credit { get; set; }
    }
}
