using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using MISA.SME.ReceiptAndPayment.RabbitMQ;
using MISA.SME.Report.Models;
using MISA.SME.Report.RabbitMQ;
using MISA.SME.Report.Test;
using Npgsql;

namespace MISA.SME.Report.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [EnableCors("MyPolicy")]
    public class GeneralLedgerController : ControllerBase
    {
        NpgsqlConnection _connection;
        public ApplicationDbContext _dbContext;

        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="dbContext">Lớp dẫn xuất</param>
        /// Created by nnanh - 06-09-2018
        public GeneralLedgerController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            // Xong xóa
            string connectString = String.Format("Server = ca-receipt-master.postgres.database.azure.com; Database = SMEReport; Port = 5432; User Id = azure_fresher@ca-receipt-master; Password = 12345678@Abc; SslMode = Require; ");
            _connection = new NpgsqlConnection(connectString);
        }

        /// <summary>
        /// Nhận thông điệp
        /// </summary>
        /// <param name="msg">Thông điệp</param>
        /// <returns>Thông điệp</returns>
        /// Created by nnanh - 06-09-2018
        public string GetMess(string msg)
        {
            return msg;
        }

        /// <summary>
        /// Lấy tất cả các báo cáo với điều kiện
        /// </summary>
        /// <param name="fromDate">Bắt đầu từ ngày</param>
        /// <param name="toDate">Đến ngày</param>
        /// <param name="page">Số trang</param>
        /// <param name="totalRecord">Tổng số bản ghi</param>
        /// <param name="companyId">Id công ty</param>
        /// <returns>Danh sách bản ghi báo cáo</returns>
        /// Created by vvkiet - 06-09-2018
        [HttpGet]
        public ActionResult GetAllGeneralLedger([FromHeader] string fromDate, [FromHeader] string toDate, [FromHeader]int page, [FromHeader]int totalRecord, [FromHeader] string companyId)
        {
            DateTime convertFromDate = Convert.ToDateTime(fromDate);
            DateTime convertToDate = Convert.ToDateTime(toDate);
            decimal changeTotalAmount = 0;
            if(page > 1)
            {
                changeTotalAmount = _dbContext.GeneralLedger.Where(x => x.CompanyID.ToString() == companyId && x.RefPostedDate >= convertFromDate).OrderBy(x => x.RefPostedDate).Skip((page-1)*totalRecord).Take(totalRecord).Sum(s => s.RefDetailAmount);
            }
            else
            {
                changeTotalAmount = _dbContext.GeneralLedger.Where(x => x.CompanyID.ToString() == companyId && x.RefPostedDate < convertFromDate).OrderBy(x => x.RefPostedDate).Skip((page-1)*totalRecord).Take(100).Sum(s => s.RefDetailAmount);
            }
            List<GeneralLedger> result = _dbContext.GeneralLedger.Where(x => x.CompanyID.ToString() == companyId && x.RefPostedDate >= convertFromDate && x.RefPostedDate <= convertToDate).OrderBy(x=>x.RefPostedDate).Skip((page-1)*totalRecord).Take(totalRecord).ToList();
            return Ok(new { Code = 200, Success = true, Data = result, ChangeTotalAmount = changeTotalAmount });
        }

        /// <summary>
        /// Lấy tổng số bản ghi trong DB báo cáo
        /// </summary>
        /// <param name="companyId">Id công ty</param>
        /// <param name="fromDate">Bắt đầu từ ngày</param>
        /// <param name="toDate">Đến ngày</param>
        /// <returns>Tổng số bản ghi</returns>
        /// Created by vvkiet - 06-09-2018
        [HttpGet]
        public ActionResult GetTotalReportByCompany([FromHeader] string companyId, [FromHeader] string fromDate, [FromHeader] string toDate)
        {
            DateTime convertFromDate = Convert.ToDateTime(fromDate);
            DateTime convertToDate = Convert.ToDateTime(toDate);
            var totalRecord = _dbContext.GeneralLedger.Count(x => x.CompanyID.ToString() == companyId && x.RefPostedDate >= convertFromDate && x.RefPostedDate <= convertToDate);
            return Ok(new { Code = 200, Success = true, Data = totalRecord });
        }

        /// <summary>
        /// Ghi bản ghi trong DB báo cáo
        /// </summary>
        /// <param name="generalLedger">Bản ghi báo cáo</param>
        /// <returns>1: Thành công</returns>
        /// <returns>0: Thất bại</returns>
        /// Created by nnanh - 06-09-2018
        public int AddGeneralLedger(GeneralLedger generalLedger)
        {
            try
            {
                var result = _dbContext.GeneralLedger.Add(generalLedger);
            }
            catch (Exception)
            {
                return 0;
            }
            _dbContext.SaveChanges();
            return 1;
        }

        /// <summary>
        /// Xóa bản ghi trong DB báo cáo
        /// </summary>
        /// <param name="generalLedgerId">Id báo cáo</param>
        /// <returns>Thông báo</returns>
        /// Created by vvkiet - 06-09-2018
        [HttpDelete]
        public IActionResult DeleteGeneralLedger([FromHeader] string generalLedgerId)
        {
            try
            {
                GeneralLedger generalLedger = _dbContext.GeneralLedger.First(i => i.ObjectID.ToString() == generalLedgerId);
                _dbContext.GeneralLedger.Remove(generalLedger);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return Ok("Lỗi khi xóa trong database");
            }
            _dbContext.SaveChanges();
            return Ok(new { Code = 200, Success = true, Data = generalLedgerId });
        }

        // Xong xóa
        [HttpPost]
        public async Task<int> InsertGeneralLedger([FromBody] List<GeneralLedger> generalLedgers)
        {
            //BulkGeneralLedger bulk;
            try
            {
                _connection.Open();
                foreach (GeneralLedger generalLedger in generalLedgers)
                {
                    for(int i = 0; i < 2; i++)
                    {
                        //bulk = new BulkGeneralLedger();
                        //bulk.BulkGeneral(generalLedger, i, _connection);
                        string insertGeneralLedger = "INSERT INTO public.\"GeneralLedger\"(\"ObjectID\", \"CompanyID\", \"CompanyCode\", \"CompanyName\", \"EmployeeID\"," +
                                " \"EmployeeCode\", \"EmployeeName\", \"AccountObjectID\", \"AccountObjectCode\", \"AccountObjectName\", \"AccountObjectAddress\", \"OrganizationUnitID\"," +
                                " \"OrganizationUnitCode\", \"OrganizationUnitName\", \"BankAccountID\", \"BankAccountCode\", \"BankAccountName\", \"RefID\", \"RefTypeName\", \"RefOrder\"," +
                                " \"RefDate\", \"RefPostedDate\", \"RefDetailDescription\", \"CurrencyID\", \"CurrencyCode\", \"CurrencyName\"," +
                                " \"RefDetailAmount\", \"RefDetailAmountOC\", \"RefDetailAccountAvailable\", \"RefDetailDebitAccount\",\"RefObjectID\")" +
                                                                            "VALUES(@ObjectID, @CompanyID, @CompanyCode, @CompanyName, @EmployeeID, @EmployeeCode, " +
                                                                            "@EmployeeName, @AccountObjectID, @AccountObjectCode, @AccountObjectName, " +
                                                                            "@AccountObjectAddress, @OrganizationUnitID, @OrganizationUnitCode, " +
                                                                            "@OrganizationUnitName, @BankAccountID, @BankAccountCode, @BankAccountName, " +
                                                                            "@RefID, @RefTypeName, @RefOrder, @RefDate, @RefPostedDate, " +
                                                                            "@RefDetailDescription, @CurrencyID, @CurrencyCode, " +
                                                                            "@CurrencyName, @RefDetailAmount, @RefDetailAmountOC, @RefDetailAccountAvailable, @RefDetailDebitAccount, @RefObjectID)";
                        NpgsqlCommand npgsqlCommand = new NpgsqlCommand(insertGeneralLedger, _connection);
                        npgsqlCommand.Parameters.AddWithValue("ObjectID", Guid.NewGuid());
                        npgsqlCommand.Parameters.AddWithValue("CompanyID", generalLedger.CompanyID);
                        npgsqlCommand.Parameters.AddWithValue("CompanyCode", "MISACloud");
                        npgsqlCommand.Parameters.AddWithValue("CompanyName", "Công ty cổ phần MISA");
                        npgsqlCommand.Parameters.AddWithValue("EmployeeID", generalLedger.EmployeeID);
                        npgsqlCommand.Parameters.AddWithValue("EmployeeCode", generalLedger.EmployeeCode == null ? "" : generalLedger.EmployeeCode);
                        npgsqlCommand.Parameters.AddWithValue("EmployeeName", "Vũ Văn Kiệt");
                        npgsqlCommand.Parameters.AddWithValue("AccountObjectID", generalLedger.AccountObjectID);
                        npgsqlCommand.Parameters.AddWithValue("AccountObjectCode", "UET14020248");
                        npgsqlCommand.Parameters.AddWithValue("AccountObjectName", generalLedger.AccountObjectName);
                        npgsqlCommand.Parameters.AddWithValue("AccountObjectAddress", generalLedger.AccountObjectAddress);
                        npgsqlCommand.Parameters.AddWithValue("OrganizationUnitID", generalLedger.OrganizationUnitID);
                        npgsqlCommand.Parameters.AddWithValue("OrganizationUnitCode", generalLedger.OrganizationUnitCode == null ? "" : generalLedger.OrganizationUnitCode);
                        npgsqlCommand.Parameters.AddWithValue("OrganizationUnitName", generalLedger.OrganizationUnitName == null ? "" : generalLedger.OrganizationUnitName);
                        npgsqlCommand.Parameters.AddWithValue("BankAccountID", generalLedger.BankAccountID);
                        npgsqlCommand.Parameters.AddWithValue("BankAccountCode", generalLedger.BankAccountCode == null ? "" : generalLedger.BankAccountCode);
                        npgsqlCommand.Parameters.AddWithValue("BankAccountName", generalLedger.BankAccountName == null ? "" : generalLedger.BankAccountName);
                        npgsqlCommand.Parameters.AddWithValue("RefID", generalLedger.RefID);
                        npgsqlCommand.Parameters.AddWithValue("RefTypeName", generalLedger.RefTypeName);
                        npgsqlCommand.Parameters.AddWithValue("RefOrder", generalLedger.RefOrder);
                        npgsqlCommand.Parameters.AddWithValue("RefDate", generalLedger.RefDate);
                        npgsqlCommand.Parameters.AddWithValue("RefPostedDate", generalLedger.RefPostedDate == null ? new DateTime() : generalLedger.RefPostedDate);
                        npgsqlCommand.Parameters.AddWithValue("RefDetailDescription", generalLedger.RefDetailDescription == null ? "" : generalLedger.RefDetailDescription);
                        npgsqlCommand.Parameters.AddWithValue("CurrencyID", generalLedger.CurrencyID);
                        npgsqlCommand.Parameters.AddWithValue("CurrencyCode", generalLedger.CurrencyCode == null ? "" : generalLedger.CurrencyCode);
                        npgsqlCommand.Parameters.AddWithValue("CurrencyName", generalLedger.CurrencyName == null ? "" : generalLedger.CurrencyName);
                        npgsqlCommand.Parameters.AddWithValue("RefDetailAmount", generalLedger.RefDetailAmount);
                        npgsqlCommand.Parameters.AddWithValue("RefDetailAmountOC", generalLedger.RefDetailAmountOC);
                        if (i == 0)
                        {
                            npgsqlCommand.Parameters.AddWithValue("RefDetailAccountAvailable", generalLedger.RefDetailAccountAvailable);
                            npgsqlCommand.Parameters.AddWithValue("RefDetailDebitAccount", generalLedger.RefDetailDebitAccount);
                        }
                        else
                        {
                            npgsqlCommand.Parameters.AddWithValue("RefDetailAccountAvailable", generalLedger.RefDetailDebitAccount);
                            npgsqlCommand.Parameters.AddWithValue("RefDetailDebitAccount", generalLedger.RefDetailAccountAvailable);
                        }
                        npgsqlCommand.Parameters.AddWithValue("RefObjectID", generalLedger.RefObjectID);
                        var isInsert = await npgsqlCommand.ExecuteNonQueryAsync();
                    }
                    //var result = _dbContext.GeneralLedger.Add(generalLedger);
                }
                _connection.Close();
            }
            catch (Exception ex)
            {
                _connection.Close();
                return 0;
            }
            return 1;
        }

        /// <summary>
        /// Lấy tổng số tiền trong DB báo cáo
        /// </summary>
        /// <param name="companyId">Id của công ty</param>
        /// <returns>Tổng số tiền</returns>
        /// Created by vvkiet - 06-09-2018
        //[HttpGet]
        //public decimal GetTotalAmount([FromHeader] string companyId, [FromHeader] string fromDate)
        //{
        //    DateTime convertFromDate = Convert.ToDateTime(fromDate);
        //    var totalAmount = _dbContext.GeneralLedger.Where(x => x.CompanyID.ToString() == companyId && x.RefPostedDate < convertFromDate).Sum(s => s.RefDetailAmount);
        //    return totalAmount;
        //}
    }
}