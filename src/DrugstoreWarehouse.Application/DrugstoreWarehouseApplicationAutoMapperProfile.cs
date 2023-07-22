using AutoMapper;
using DrugstoreWarehouse.Batches;
using DrugstoreWarehouse.Drugstores;
using DrugstoreWarehouse.Products;
using DrugstoreWarehouse.Warehouses;
using System;

namespace DrugstoreWarehouse;

public class DrugstoreWarehouseApplicationAutoMapperProfile : Profile
{
    public DrugstoreWarehouseApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<Product, Products.ProductDto>();
        CreateMap<CreateUpdateProductDto, Product>();

        CreateMap<Drugstore, DrugstoreDto>();
        CreateMap<CreateUpdateDrugstoreDto, Drugstore>();

        CreateMap<Warehouse, WarehouseDto>();
        CreateMap<CreateUpdateWarehouseDto, Warehouse>();

        CreateMap<Batch, BatchDto>()
            .ForMember(x => x.ProductName, opt => opt.MapFrom(src => ValueOrDefault(src, "Product.Name", string.Empty)));
        CreateMap<CreateUpdateBatchDto, Batch>();

    }

    private static T ValueOrDefault<T, E>(E entity, string propPath, T defaultValue = default)
    {
        var properties = propPath.Split(".");
        object value = entity;
        var type = typeof(E);
        foreach (var propName in properties)
        {
            value = type.GetProperty(propName)?.GetValue(value);
            if (value == null)
            {
                return defaultValue == null ? default : (T)defaultValue;
            }
            type = value.GetType();
        }
        if (value is not T)
        {
            throw new ArgumentException($"Property \"{propPath}\" is not \"{typeof(T).FullName}\"");
        }
        return (T)value;
    }

}
