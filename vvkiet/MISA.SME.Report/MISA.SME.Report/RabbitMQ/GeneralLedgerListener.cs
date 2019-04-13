using EasyNetQ;
using MISA.SME.Report.Models;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.SME.Report.RabbitMQ
{
    public class GeneralLedgerListener
    {
        //NpgsqlConnection _connection;
        //DataSet dataSet;
        //DataTable dataTable;
        //public static ApplicationDbContext context = new ApplicationDbContext();
        //public GeneralLedgerListener()
        //{
        //    dataSet = new DataSet();
        //    dataTable = new DataTable();
        //    string connectString = String.Format("Server = ca-receipt-master.postgres.database.azure.com; Database = SMEReport; Port = 5432; User Id = azure_fresher@ca-receipt-master; Password = 12345678@Abc; SslMode = Require; ");
        //    _connection = new NpgsqlConnection(connectString);



        //}
        //public void Receive()
        //{
        //    using (var bus = RabbitHutch.CreateBus("host=localhost;virtualHost=/;username=vvkiet;password=12345678@Abc"))
        //    {
        //        //bus.Subscribe<string>("writeGeneralLedger", Handle);
        //        //bus.Subscribe<string>("handleMessObj1", HandleMessObj);    
        //        bus.Subscribe<string>("handleMessObj4", HandleMessObj);
        //    }
        //}
        //public void HandleMessObj(string msg)
        //{
        //    //var messageRequest = JContainer.Parse(msg);   
        //    if (msg != null)
        //    {
        //        var messageRequest = JsonConvert.DeserializeObject<MessageListener>(msg);
        //        var label = messageRequest.Label;
        //        var generalLedger = JsonConvert.DeserializeObject<GeneralLedger>(messageRequest.GeneralLedger.ToString());
        //        if (label.Equals("writeGeneralLedger"))
        //        {
        //            try
        //            {
        //                context.GeneralLedger.Add(generalLedger);
        //                context.SaveChanges();
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }
        //        else if (label.Equals("removeGeneralLedger"))
        //        {
        //            try
        //            {
        //                var genDel = new GeneralLedger { ObjectID = generalLedger.ObjectID };
        //                var findGen = context.GeneralLedger.FirstOrDefault(x => x.ObjectID == genDel.ObjectID);
        //                if (findGen != null)
        //                {

        //                    using (_connection)
        //                    {
        //                        string query = "delete from public.\"GeneralLedger\" where public.\"GeneralLedger\".\"ObjectID\" = '" + findGen.ObjectID + "'";
        //                        //                               DELETE FROM public."GeneralLedger"
        //                        //WHERE public."GeneralLedger"."ObjectID"='13d2ead5-b7aa-4ce8-ad84-cbb1914c5323';
        //                        NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, _connection);
        //                        NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(npgsqlCommand);
        //                        dataSet.Reset();
        //                        dataAdapter.Fill(dataSet);
        //                        _connection.Close();
        //                    }
        //                    //context.GeneralLedger.Remove(genDel);
        //                    //context.SaveChanges();
        //                }
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }
        //        else
        //        {
        //        }
        //        //var generalLedgerJS = messageRequest.Value<string>();
        //        //var label = messageRequest.First.Value<JToken>().First.Value<string>();

        //        //GeneralLedger generalLedger = JsonConvert.DeserializeObject<GeneralLedger>(generalLedgerJS);
        //    }

        //}
    }
}
