using DrugstoreWarehouse.Batches;
using DrugstoreWarehouse.Drugstores;
using DrugstoreWarehouse.Products;
using DrugstoreWarehouse.Warehouses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace DrugstoreWarehouse.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class DrugstoreWarehouseDbContext :
    AbpDbContext<DrugstoreWarehouseDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    //Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }

    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    public DbSet<Batch> Batches { get; set; }
    public DbSet<Drugstore> Drugstores { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Warehouse> Warehouses { get; set; }


    public DrugstoreWarehouseDbContext(DbContextOptions<DrugstoreWarehouseDbContext> options)
        : base(options)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { 
        optionsBuilder.LogTo(message => Debug.WriteLine(message));
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        /* Configure your own tables/entities inside here */

        builder.Entity<Batch>(b =>
        {
            b.ToTable(
                "Batches", 
                t => t.HasCheckConstraint("CK_Batches_Quantity_Range", $"\"Quantity\" >= {BatchConsts.MinQuantity} AND \"Quantity\" <= {BatchConsts.MaxQuantity}")
                );
            b.HasOne(x => x.Product).WithMany(x => x.Batches).HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.NoAction); 
        });

        builder.Entity<Drugstore>(b =>
        {
            b.ToTable("Drugstores");
            b.Property(x => x.Name).HasMaxLength(DrugstoreConsts.MaxNameLength).IsRequired();
            b.Property(x => x.Address).HasMaxLength(DrugstoreConsts.MaxAdressLength).IsRequired();
            b.Property(x => x.TelNumber).HasMaxLength(DrugstoreConsts.MaxTelNumberLength).IsRequired();
            b.HasMany(x => x.Warehouses).WithOne(y => y.Drugstore).HasForeignKey(y => y.DrugstoreId).OnDelete(DeleteBehavior.NoAction);
        });

        builder.Entity<Product>(b =>
        {
            b.ToTable("Products");
            b.Property(x => x.Name).HasMaxLength(ProductConsts.MaxNameLength).IsRequired();
        });

        builder.Entity<Warehouse>(b =>
        {
            b.ToTable("Warehouses");
            b.Property(x => x.Name).HasMaxLength(WarehouseConsts.MaxNameLength).IsRequired();
            b.HasMany(x => x.Batches).WithOne(y => y.Warehouse).HasForeignKey(y => y.WarehouseId).OnDelete(DeleteBehavior.NoAction); 
        });


    }
}
