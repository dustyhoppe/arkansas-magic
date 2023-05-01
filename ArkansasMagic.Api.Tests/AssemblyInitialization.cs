using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Http;

namespace ArkansasMagic.Api.Tests
{
    [TestClass]
    public class AssemblyInitialization
    {
        private static string BaseAddress;

        public static IHost Host { get; private set; }
        public static HttpClient Client { get; private set; }

        public static T Get<T>() where T : class => (T)Host.Services.GetService(typeof(T));

        [AssemblyInitialize]
        public static void Initialize(TestContext context)
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "servicetest");

            BaseAddress = "http://127.0.0.1:5010/";
            Host = Program.CreateHostBuilder(BaseAddress);
            Host.Start();

            Client = new HttpClient(new HttpClientHandler
            {
                AllowAutoRedirect = false,
                UseCookies = true,
                CookieContainer = new CookieContainer()
            })
            {
                BaseAddress = new Uri(BaseAddress)
            };
        }

        [AssemblyCleanup]
        public static void Cleanup()
        {
            Host?.Dispose();
        }
    }
}
