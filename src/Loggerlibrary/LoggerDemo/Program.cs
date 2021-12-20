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
        /// <summary>
        /// DI Config
        /// </summary>
        /// <returns></returns>
        static IHost Configuration()
        {
            IHost host = Host.CreateDefaultBuilder()
            .ConfigureServices(
            (_, services) => services
                .AddTransient<Loggerlibrary.IConfiguration, Configuration>()
                .AddTransient<Loggerlibrary.ILogger, Loggerlibrary.LoggerAsync>()
                .AddTransient<Loggerlibrary.LogTarget.ILogTarget, Loggerlibrary.LogTarget.LogTargetConsole>()
            ).Build();

            return host;
        }

        static void Main(string[] args)
        {

            var host = Configuration();

            Console.WriteLine("Hello from logger demo");

            //Get logger instance from DI
            var logger = host.Services.GetRequiredService<Loggerlibrary.ILogger>();


            var stopw = Stopwatch.StartNew();
        
            logger.WriteLog("0 Hello árvíztűrő tükörfúrógép -----------------------");
            logger.WriteLog("1 Hello info", LogLevel.info);
            logger.WriteLog("2 Hello debug", LogLevel.debug);
            logger.WriteLog("3 Hello error", LogLevel.error);

            //backround task for demo
            Task t = Task.Factory.StartNew<Task>( async () =>
               {

                   for (int i = 0; i < 100; i++)
                   {
                       await logger.WriteLog($"{i} Hello info await",i%5==0?LogLevel.debug:LogLevel.info);
                       await Task.Delay(100);
                   }

               });
         
            stopw.Stop();

            Console.WriteLine($"--------end ----- {stopw.ElapsedMilliseconds} ms");
            Console.ReadKey();

        }
    }
}
