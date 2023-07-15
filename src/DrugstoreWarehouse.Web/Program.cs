using System;
using System.IO;
using System.Threading.Tasks;
using Castle.Core.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace DrugstoreWarehouse.Web;

public class Program
{
    public async static Task<int> Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json")
              .AddUserSecrets(System.Reflection.Assembly.GetExecutingAssembly())
              .Build();

        var logLevel = configuration.GetValue<string>("Serilog:MinimumLevel"); // read level from appsettings.json
        if (!Enum.TryParse<LogEventLevel>(logLevel, true, out var level))
        {
            level = LogEventLevel.Information; // or set default value
        }

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Is(level)
            .Enrich.FromLogContext()
            .WriteTo.PostgreSQL(
                    connectionString: configuration.GetSection("ConnectionStrings:Default").Value,
                    schemaName: configuration.GetSection("Serilog:SchemaName").Value,
                    tableName: configuration.GetSection("Serilog:TableName").Value,
                    needAutoCreateTable: true
                )
            .CreateLogger();

        try
        {
            Log.Information("Starting web host.");
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.AddAppSettingsSecretsJson()
                .UseAutofac()
                .UseSerilog();
            await builder.AddApplicationAsync<DrugstoreWarehouseWebModule>();
            var app = builder.Build();
            await app.InitializeApplicationAsync();
            await app.RunAsync();
            return 0;
        }
        catch (Exception ex)
        {
            if (ex is HostAbortedException)
            {
                throw;
            }

            Log.Fatal(ex, "Host terminated unexpectedly!");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
