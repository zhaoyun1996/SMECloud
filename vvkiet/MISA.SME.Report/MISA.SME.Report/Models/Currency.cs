using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.SME.Report.Models
{
    public class Currency
    {
        public Guid CurrencyID { get; set; }

        public string CurrencyName { get; set; }

        public string CurrencyCode { get; set; }
    }
}
