using System;
using System.Threading.Tasks;
using Leahey.NasaApi.Implementations;
using Leahey.NasaApi.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Leahey.NasaApi
{
    class Program
    {
        #region private fields
        private static IServiceProvider _serviceProvider;
        #endregion

        #region private methods
        private static void DisposeServices()
        {
            if (_serviceProvider == null)
                return;

            if (_serviceProvider is IDisposable)
            {
                ((IDisposable)_serviceProvider).Dispose();
            }
        }

        private static void RegisterServices()
        {
            var services = new ServiceCollection();
            services
                .AddSingleton<IConsoleApplication, ConsoleApplication>()
                .AddSingleton<INasaApiClient, NasaApiClient>();
            _serviceProvider = services.BuildServiceProvider(true);
        }
        
        #endregion

        static async Task Main(string[] args)
        {
            RegisterServices();
            IServiceScope scope = _serviceProvider.CreateScope();
            IConsoleApplication consoleApplication = scope.ServiceProvider.GetRequiredService<IConsoleApplication>();
            await consoleApplication
                .Run()
                .ConfigureAwait(true);
            DisposeServices();
        }
    }
}
