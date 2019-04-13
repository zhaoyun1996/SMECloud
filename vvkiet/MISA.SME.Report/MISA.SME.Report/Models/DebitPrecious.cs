using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.SME.Report.Models
{
    public class DebitPrecious
    {
        [Key]
        public Guid ObjectID { get; set; }

        public Guid PreciousID { get; set; }

        public string PreciousName { get; set; }

        public Guid AccountObjectID { get; set; }

        public string AccountObjectName { get; set; }

        public float Debit { get; set; }

        public float Credit { get; set; }
    }
}
