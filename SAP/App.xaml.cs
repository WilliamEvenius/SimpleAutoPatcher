using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SAP.Application;
using SAP.Application.Factories;
using SAP.Application.Services;
using SAP.Application.State;
using SAP.Interfaces;
using SAP.Interfaces.Factories;
using SAP.Interfaces.Services;
using SAP.Interfaces.State;
using System.Configuration;
using System.Data;
using System.IO;
using System.Reactive.Subjects;
using System.Text;
using System.Windows;

namespace SAP
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public IServiceProvider? ServiceProvider { get; private set; }
        public IConfiguration? Configuration { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            ServiceProvider = new ServiceCollection()
                .ConfigureViewModels()
                .ConfigureViews()
                .ConfigureFactories()
                .ConfigureServices()
                .BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }

    public static class AppBootstrapper
    {
        public static ServiceCollection ConfigureViewModels(this ServiceCollection collection)
        {
            collection.AddTransient<MainWindowViewModel>();
            return collection;
        }

        public static ServiceCollection ConfigureViews(this ServiceCollection collection)
        {
            collection.AddTransient<MainWindow>();
            return collection;
        }

        public static ServiceCollection ConfigureFactories(this ServiceCollection collection)
        {
            collection.AddSingleton<IStateFactory, StateFactory>();
            return collection;
        }

        public static ServiceCollection ConfigureServices(this ServiceCollection collection)
        {
            collection.AddSingleton<IApplicationStateMachine, ApplicationStateMachine>();
            collection.AddSingleton<IProgressReporter<float>, ProgressReporter<float>>();
            collection.AddSingleton<IPatcherService, PatcherService>();
            return collection;
        }
    }
}
