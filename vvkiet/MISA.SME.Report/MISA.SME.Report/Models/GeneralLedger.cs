//using MISA.SME.ReceiptAndPayment.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.SME.Report.RabbitMQ
{
    public class GeneralLedger
    {
        [Key]
        public Guid ObjectID { get; set; }
        public Guid CompanyID { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public Guid EmployeeID { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public Guid AccountObjectID { get; set; }
        public string AccountObjectCode { get; set; }
        public string AccountObjectName { get; set; }
        public string AccountObjectAddress { get; set; }
        public Guid OrganizationUnitID { get; set; }
        public string OrganizationUnitCode { get; set; }
        public string OrganizationUnitName { get; set; }
        public Guid BankAccountID { get; set; }
        public string BankAccountCode { get; set; }
        public string BankAccountName { get; set; }
        public Guid RefObjectID { get; set; }
        public string RefID { get; set; }
        //public string RefDetailID { get; set; }
        public string RefTypeName { get; set; }
        public string RefOrder { get; set; }
        public DateTime? RefDate { get; set; }
        public DateTime? RefPostedDate { get; set; }
        public string RefDetailDescription { get; set; }
        public decimal RefDetailAmount { get; set; }
        public decimal RefDetailAmountOC { get; set; }
        public decimal RefDetailDebitAccount { get; set; }
        public decimal RefDetailAccountAvailable { get; set; }
        public Guid CurrencyID { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public double ExchangeRate { get; set; }



        //[NotMapped]
        //public List<ReceiptDetail> ReceiptDetails { get; set; }

        //[NotMapped]
        //public Currency Currency { get; set; }
    }
}
