﻿using System;
using System.Diagnostics;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DrugstoreWarehouse.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class DrugstoreWarehouseDbContextFactory : IDesignTimeDbContextFactory<DrugstoreWarehouseDbContext>
{
    public DrugstoreWarehouseDbContext CreateDbContext(string[] args)
    {
        // https://www.npgsql.org/efcore/release-notes/6.0.html#opting-out-of-the-new-timestamp-mapping-logic
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        DrugstoreWarehouseEfCoreEntityExtensionMappings.Configure();

        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<DrugstoreWarehouseDbContext>()
            .UseNpgsql(configuration.GetConnectionString("Default"));

        return new DrugstoreWarehouseDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../DrugstoreWarehouse.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false)
            .AddUserSecrets(System.Reflection.Assembly.Load("DrugstoreWarehouse.DbMigrator"));

        return builder.Build();
    }
}
