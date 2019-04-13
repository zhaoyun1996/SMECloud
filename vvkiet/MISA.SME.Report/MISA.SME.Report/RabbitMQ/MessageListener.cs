using EasyNetQ;
using MISA.SME.Report.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.SME.Report.RabbitMQ
{
    public class MessageListener
    {
        public string Label { get; set; }
        public object GeneralLedger { get; set; }        
    }
}
