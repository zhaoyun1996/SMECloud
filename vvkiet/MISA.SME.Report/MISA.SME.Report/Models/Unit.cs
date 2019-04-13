using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.SME.Report.Models
{
    /// <summary>
    /// Các đơn gi khác nhau trong AccountObject
    /// </summary>
    public class Unit
    {
        public Guid UnitID { get; set; }
        public string UnitName { get; set; }
        public string UnitCode { get; set; }
    }
}
