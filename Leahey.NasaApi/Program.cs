using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Leahey.NasaApi.CodeQuality;
using Leahey.NasaApi.Implementations;
using Leahey.NasaApi.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Leahey.NasaApi
{
    sealed class Program
    {
        #region private fields
        private static IServiceProvider _serviceProvider;
        #endregion

        #region private methods
        [NDependIgnore("Disposal")]
        [ExcludeFromCodeCoverage]
        private static void DisposeServices()
        {
            if (_serviceProvider == null)
                return;

            if (_serviceProvider is IDisposable)
            {
                ((IDisposable)_serviceProvider).Dispose();
            }
        }

        [NDependIgnore("DI service registration")]
        [ExcludeFromCodeCoverage]
        private static void RegisterServices()
        {
            var services = new ServiceCollection();
            services
                .AddSingleton<IConsoleApplication, ConsoleApplication>()
                .AddSingleton<INasaApiClient, NasaApiClient>()
                .AddSingleton<IWebClientProxy, WebClientProxy>() // must be singleton since consumed by singleton
                .AddSingleton<IMarsRoverUtils, MarsRoverUtils>();
            _serviceProvider = services.BuildServiceProvider(true);
        }

        #endregion

        [NDependIgnore("Largely a pass-through method")]
        static async Task Main(string[] args)
        {
            //ncrunch: no coverage start
            RegisterServices();
            IServiceScope scope = _serviceProvider.CreateScope();
            IConsoleApplication consoleApplication = scope.ServiceProvider.GetRequiredService<IConsoleApplication>();
            await consoleApplication
                .Run()
                .ConfigureAwait(true);
            DisposeServices();
            //ncrunch: no coverage end
        }
    }
}
