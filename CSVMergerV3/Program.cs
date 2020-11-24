using CSVMergerV3.Application.Factories;
using CSVMergerV3.Application.FileProcessor;
using CSVMergerV3.Application.Services.HelperServices;
using CSVMergerV3.Application.Services.Orchestrators;
using CSVMergerV3.Application.Services.Validation;
using CSVMergerV3.Application.State;
using CSVMergerV3.UI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;


namespace CSVMergerV3
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            Log.Logger.Information("Application starting");

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<ITUIRoutine, TUIRoutine>();
                    services.AddSingleton<IDataSetNameValidator, DataSetNameValidator>();
                    services.AddSingleton<IConfigurationState, ConfiguationState>();
                    services.AddScoped<IDirectoryValidator, DirectoryValidator>();
                    services.AddScoped<IFileValidator, FileValidator>();
                    services.AddTransient<IFileStreamProvider, FileStreamProvider>();
                    services.AddScoped<IRuleChecker, RuleChecker>();
                    services.AddSingleton<IJobState, JobState>();
                    services.AddScoped<IFileLineCounter, FileLineCounter>();
                    services.AddScoped<ILineProducerConsumerOrechestrator, LineProducerConsumerOrechestrator>();
                    //services.AddScoped<IOutputDatasetFactory, OutputDatasetFactory>();
                    services.AddScoped<IInputDatasetFactory, InputDatasetFactory>();
                    services.AddScoped<IFileProcessor, FileProcessor>();
                    services.AddSingleton<IPercentageCalculator, PercentageCalculator>();
                })
                .UseSerilog()
                .Build();

            var svc = ActivatorUtilities.CreateInstance<TUIRoutine>(host.Services);
            svc.Run();
        }

        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();
        }
    }
}
