using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MISA.SME.Report.RabbitMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.SME.Report.Services
{
    public static class ApplicationBuilderExtention
    {
        //private static GeneralLedgerListener _generalLedgerListener { get; set; }
        private static RabbitListener _rabbitListener { get; set; }
        public static IApplicationBuilder UseRabbitListener(this IApplicationBuilder app)
        {
            //_generalLedgerListener = app.ApplicationServices.GetService<GeneralLedgerListener>();
            _rabbitListener = app.ApplicationServices.GetService<RabbitListener>();
            //_listener = app.ApplicationServices.GetService<RabbitListener>();

            var lifetime = app.ApplicationServices.GetService<IApplicationLifetime>();

            lifetime.ApplicationStarted.Register(OnStarted);
            lifetime.ApplicationStopping.Register(OnStopping);
            //press Ctrl+C to reproduce if your app runs in Kestrel as a console app
            //lifetime.ApplicationStopping.Register(() => _listener.Deregister());
            // OnStopping();
            return app;
        }
        private static void OnStarted() {
            //_generalLedgerListener.Receive();
            _rabbitListener.Register();
        }

        private static void OnStopping()
        {
            //_generalLedgerListener.Receive();
            _rabbitListener.Deregister();
        }
    }
}
