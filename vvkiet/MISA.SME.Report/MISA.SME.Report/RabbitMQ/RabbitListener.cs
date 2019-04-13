using MISA.SME.Report.Models;
using Newtonsoft.Json;
using Npgsql;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MISA.SME.Report.RabbitMQ
{
    public class RabbitListener
    {
        NpgsqlConnection _connection;
        ConnectionFactory factory { get; set; }
        IConnection connection { get; set; }
        IModel channel { get; set; }
        private readonly ApplicationDbContext _context = null;

        /// <summary>
        /// Đăng ký sử dụng rabbit
        /// </summary>
        /// Created by nnanh - 06-09-2018
        public void Register()
        {
            try
            {
                channel.QueueDeclare(queue: "generalLedgerListener", durable: false, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    HandleMessObj(message);
                };
                channel.BasicConsume(queue: "generalLedgerListener", autoAck: true, consumer: consumer);
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        /// <summary>
        /// Đóng kết nối
        /// </summary>
        /// Created by nnanh - 06-09-2018
        public void Deregister()
        {
            this.connection.Close();
            _connection.Close();
        }

        /// <summary>
        /// Cấu hình kết nối rabbit
        /// </summary>
        /// Created by nnanh - 06-09-2018
        public RabbitListener()
        {            
            this.factory = new ConnectionFactory() { HostName = "13.83.22.129", VirtualHost = "/", UserName = "admin", Password = "12345678@Abc", Port = 5672 };
            this.connection = factory.CreateConnection();
            this.channel = connection.CreateModel();
            _context = new ApplicationDbContext();
            string connectString = String.Format("Server = ca-receipt-master.postgres.database.azure.com; Database = SMEReport; Port = 5432; User Id = azure_fresher@ca-receipt-master; Password = 12345678@Abc; SslMode = Require; ");
            _connection = new NpgsqlConnection(connectString);            
        }

        /// <summary>
        /// Nhận thông điệp gửi về và xử lí
        /// </summary>
        /// <param name="message">Thông điệp nhận về</param>
        /// Created by nnanh - 06-09-2018
        public void HandleMessObj(string message)
        {
            if (message != null)
            {
                var messageRequest = JsonConvert.DeserializeObject<MessageListener>(message);
                var label = messageRequest.Label;
                var generalLedgers = JsonConvert.DeserializeObject<List<GeneralLedger>>(messageRequest.GeneralLedger.ToString());
                if (label.Equals("writeGeneralLedger"))
                {
                    try
                    {
                        _connection.Open();
                        foreach(GeneralLedger generalLedger in generalLedgers) {
                            for(var i = 0; i < 2; i++)
                            {
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
                                if(i == 0)
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
                                var isInsert = npgsqlCommand.ExecuteNonQuery();
                            }
                        }
                        _connection.Close();
                    }
                    catch (Exception)
                    {
                        _connection.Close();
                        throw;
                    }
                }
                else if (label.Equals("removeGeneralLedger"))
                {
                    try
                    {
                        var genDel = new GeneralLedger { RefID = generalLedgers[0].RefID };
                        //List<GeneralLedger> findGen = _context.GeneralLedger.Select(r => r).ToList();
                        var findGen = _context.GeneralLedger.FirstOrDefault(x => x.RefID == genDel.RefID);
                        //var findGen = _context.GeneralLedger.Select(x => x.RefID == genDel.RefID);
                        if (findGen != null)
                        {
                            _connection.Open();
                            string query = "delete from public.\"GeneralLedger\" where public.\"GeneralLedger\".\"RefID\" = '" + findGen.RefID + "'";
                            NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, _connection);
                            npgsqlCommand.ExecuteNonQuery();
                            _connection.Close();
                        }
                    }
                    catch (Exception)
                    {
                        _connection.Close();
                        throw;
                    }
                }
                else
                {
                }
            }

        }
    }
}
