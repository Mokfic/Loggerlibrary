using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using Loggerlibrary.Model;
using System.Threading.Tasks;
using System.Diagnostics;

namespace LoggerDemo
{
    class Program
    {

        static IHost Configuration()
        {
            IHost host = Host.CreateDefaultBuilder()
            .ConfigureServices(
            (_, services) => services
                .AddTransient<Loggerlibrary.IConfiguration, Configuration>()
                .AddTransient<Loggerlibrary.ILogger, Loggerlibrary.LoggerQueue>()
                .AddTransient<Loggerlibrary.LogTarget.ILogTarget, Loggerlibrary.LogTarget.LogTargetFile>()
            ).Build();

            return host;
        }

        static void Main(string[] args)
        {

            var host = Configuration();

            Console.WriteLine("Hello from logger demo");

            var logger = host.Services.GetRequiredService<Loggerlibrary.ILogger>();

            var stopw = Stopwatch.StartNew();

            logger.WriteLog("0 Hello árvíztűrő tükörfúrógép -----------------------");
            logger.WriteLog("1 Hello info", LogLevel.info);
            logger.WriteLog("2 Hello debug", LogLevel.debug);
            logger.WriteLog("3 Hello error", LogLevel.error);

            Task t = Task.Factory.StartNew( async () =>
               {
                   await logger.WriteLog("1 Hello info await", LogLevel.info);
                   await logger.WriteLog("2 Hello info await", LogLevel.debug);
                   await logger.WriteLog("3 Hello info await", LogLevel.info);
                   await logger.WriteLog("4 Hello info await", LogLevel.info);
                   await logger.WriteLog("5 Hello info await", LogLevel.debug);
                   await logger.WriteLog("6 Hello info await", LogLevel.info);

               });
            t.Wait();
            stopw.Stop();

            Console.WriteLine($"--------end ----- {stopw.ElapsedMilliseconds} ms");
            Console.ReadKey();

        }
    }
}
