using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;

namespace ArkansasMagic.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder("http://0.0.0.0:5555/", args).Run();
        }

        public static IHost CreateHostBuilder(string baseUri, params string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? Environments.Production;

            var currentDir = Directory.GetCurrentDirectory();

            var config = new ConfigurationBuilder()
                .SetBasePath(currentDir)
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();

            var urls = new List<string>() { baseUri };

            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseConfiguration(config)
                    .UseContentRoot(currentDir)
                    .UseStartup<Startup>()
                    .UseUrls(urls.ToArray());
                })
                .UseSerilog()
                .Build();
        }
    }
}
