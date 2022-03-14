using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using Topshelf;

namespace Host
{
    public class Program
    {

        private class RestService
        {
            private IDisposable _app;
            private IHost _host;
            private IHostBuilder _HostBuilde;

            public void Start()
            {
                _HostBuilde = CreateHostBuilder(null);
                _host = _HostBuilde.Build();
                _host.RunAsync();
            }

            public void Stop()
            {
                if (_host != null)
                {
                    var task = _host.StopAsync();
                    //System.Threading.Tasks.Task.WaitAll(task);
                    if (_host != null)
                    {
                        _host.Dispose();
                    }
                }
            }
        }

        public static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<RestService>(s =>
                {
                    s.ConstructUsing(() => new RestService());
                    s.WhenStarted(rs => rs.Start());
                    s.WhenStopped(rs => rs.Stop());
                    s.WhenShutdown(rs => rs.Stop());
                });
                x.RunAsLocalSystem();
                x.StartAutomatically();

                x.SetServiceName("QuarteUIService");
                x.SetDisplayName("QuarteUIService");
                x.SetDescription("后台任务");
            });
            //CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
                Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls("http://localhost:8100");
                });
    }
}
