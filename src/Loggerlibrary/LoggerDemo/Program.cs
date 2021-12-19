using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using Loggerlibrary.Model;

namespace LoggerDemo
{
    class Program
    {

        static void Main(string[] args)
        {
            using IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(
                (_, services) => services
                    .AddTransient<Loggerlibrary.ILogger, Loggerlibrary.Logger>()
                    .AddTransient<Loggerlibrary.LogTarget.ILogTarget, Loggerlibrary.LogTarget.LogTargetConsole>()
                ).Build();

            Console.WriteLine("Hello World!");



            var logger = host.Services.GetRequiredService<Loggerlibrary.ILogger>();

            logger.WriteLog("Hello default");
            logger.WriteLog("Hello info", LogLevel.info);
            logger.WriteLog("Hello debug", LogLevel.debug);
            logger.WriteLog("Hello error", LogLevel.error);
            Console.ReadKey();
        }
    }
}
