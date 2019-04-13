using EasyNetQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MISA.SME.Report.Models;
using MISA.SME.Report.RabbitMQ;
using MISA.SME.Report.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Threading;

namespace MISA.SME.Report
{
    public class Startup
    {
        //public ApplicationDbContext context;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Setting.ConnectionString = Configuration.GetSection("ConnectionStrings:DefaultConnection").Value;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            //if(context == null)
            //{
            //    context = new ApplicationDbContext();
            //}
            //Thread thread = new Thread(new ThreadStart(Receive));
            //thread.Start();            
        }

        //public void Receive()
        //{
        //    using (var bus = RabbitHutch.CreateBus("host=localhost;virtualHost=/;username=vvkiet;password=12345678@Abc"))
        //    {
        //        //bus.Subscribe<string>("writeGeneralLedger", Handle);
        //        //bus.Subscribe<string>("handleMessObj1", HandleMessObj);               
        //        bus.Subscribe<string>("handleMessObj3", HandleMessObj);
        //    }
        //}

        //public void Handle(string msg)
        //{
        //    var b = JContainer.Parse(msg);

        //    //Console.WriteLine(header.Trim(new Char[] { ' ', '*', '.' }));
        //    //var msgFormat = msg.Trim(new char[] { '{', ']' });
        //    //var messageRequest = JsonConvert.DeserializeObject<GeneralLedger>(msg);
        //    var messageRequest = JContainer.Parse(msg);
        //    var generalLedgerJS = messageRequest.Value<string>();
        //    GeneralLedger generalLedger = JsonConvert.DeserializeObject<GeneralLedger>(generalLedgerJS);
        //    //GeneralLedger b = JsonConvert.DeserializeObject<GeneralLedger>(messageRequest.GeneralLedger);

        //    //var a = messageRequest.GeneralLedger.ToString();
        //    //MessageListener messageListener = new MessageListener
        //    //{
        //    //    //Label = messageRequest.Label,
        //    //    //GeneralLedger = JsonConvert.DeserializeObject<string>(messageRequest.GeneralLedger.ToString()),
        //    //};

        //    //GeneralLedgerController con = new GeneralLedgerController(context);
        //    //con.GetMess(msg);
        //    //if (messageListener.Label.Equals("writeGeneralLedger"))
        //    //{
        //    //    GeneralLedger generalLedger = messageListener.GeneralLedger;
        //    //    object generalLedger = messageListener.GeneralLedger;
        //    //    context.GeneralLedger.Add(generalLedger);
        //    //    context.SaveChanges();
        //    //}
        //    //context.GeneralLedger.Add(generalLedger);
        //    //context.GeneralLedger.Add(generalLedger);
        //    //context.SaveChanges();
        //}

        //public void HandleMessObj(string msg)
        //{
        //    //var messageRequest = JContainer.Parse(msg);
        //    var messageRequest = JsonConvert.DeserializeObject<MessageListener>(msg);
        //    var label = messageRequest.Label;
        //    var generalLedger = JsonConvert.DeserializeObject<GeneralLedger>(messageRequest.GeneralLedger.ToString());
        //    if (label.Equals("writeGeneralLedger"))
        //    {
        //        try
        //        {
        //            context.GeneralLedger.Add(generalLedger);
        //            context.SaveChanges();
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //    }
        //    else if (label.Equals("removeGeneralLedger"))
        //    {
        //        try
        //        {
        //            var genDel = new GeneralLedger { ObjectID = generalLedger.ObjectID };
        //            context.GeneralLedger.Remove(genDel);
        //            context.SaveChanges();
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //    }
        //    else
        //    {
        //    }
        //    //var generalLedgerJS = messageRequest.Value<string>();
        //    //var label = messageRequest.First.Value<JToken>().First.Value<string>();

        //    //GeneralLedger generalLedger = JsonConvert.DeserializeObject<GeneralLedger>(generalLedgerJS);
        //}
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add Cors
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            // Add framework services.
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("MyPolicy"));
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            // ===== Add our DbContext ========
            services.AddDbContext<ApplicationDbContext>(options =>
                            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            //services.AddSingleton<GeneralLedgerListener>();
            services.AddSingleton<RabbitListener>();
            //services.Configure<Settings>(options =>
            //{
            //    options.ConnectionString = Configuration.GetSection("ConnectionStrings:DefaultConnection").Value;
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("MyPolicy");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            //app.UseRabbitListener();                
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseRabbitListener();
            //lifetime.ApplicationStarted.Register(OnAppStarted);
            //lifetime.ApplicationStopping.Register(OnAppStopping);
            //lifetime.ApplicationStopped.Register(OnAppStopped);
            //var dbContext = app.ApplicationServices.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;
            app.UseHttpsRedirection();
            app.UseMvc();
        }
        //public void OnAppStarted()
        //{
        //    Receive();
        //}
    }
}
