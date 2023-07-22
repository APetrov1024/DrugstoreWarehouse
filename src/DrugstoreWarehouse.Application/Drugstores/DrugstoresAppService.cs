using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using DrugstoreWarehouse.Localization;
using Volo.Abp.ObjectMapping;
using DrugstoreWarehouse.Products;

namespace DrugstoreWarehouse.Drugstores
{
    public class DrugstoresAppService: DrugstoreWarehouseAppService, IDrugstoresAppService
    {
        private readonly IRepository<Drugstore, Guid> _drugstoresRepository;

        public DrugstoresAppService(IRepository<Drugstore, Guid> drugstoresRepository)
        {
            _drugstoresRepository = drugstoresRepository;
        }

        public async Task<DrugstoreDto> GetAsync(Guid id)
        {
            try
            {
                var drugstore = await _drugstoresRepository.GetAsync(id);
                return ObjectMapper.Map<Drugstore, DrugstoreDto>(drugstore);
            }
            catch (EntityNotFoundException ex)
            {
                throw new UserFriendlyException(L[LocalizerKeys.Errors.DrugstoreNotFound], innerException: ex);
            }
        }

        public async Task<List<DrugstoreDto>> GetListAsync()
        {
            var drugstores = await _drugstoresRepository.GetListAsync();
            return ObjectMapper.Map<List<Drugstore>, List<DrugstoreDto>>(drugstores);
        }

        public async Task<List<ProductDto>> GetProductsAsync(Guid drugstoreId)
        {
            var query = (await _drugstoresRepository.GetQueryableAsync())
                .Where(x => x.Id == drugstoreId)
                .SelectMany(x => x.Warehouses)
                .SelectMany(x => x.Batches)
                .GroupBy(x => x.Product.Name)
                .Select(g => new ProductDto
                {
                    Name = g.Key,
                    Quantity = g.Sum(x => x.Quantity),
                });
            var result = await AsyncExecuter.ToListAsync(query);
            return result;
        }

        public async Task<DrugstoreDto> CreateAsync(CreateUpdateDrugstoreDto dto)
        {
            var drugstore = ObjectMapper.Map<CreateUpdateDrugstoreDto, Drugstore>(dto);
            drugstore = await _drugstoresRepository.InsertAsync(drugstore);
            return ObjectMapper.Map<Drugstore, DrugstoreDto>(drugstore);
        }

        public async Task<DrugstoreDto> UpdateAsync(Guid id, CreateUpdateDrugstoreDto dto)
        {
            try
            {
                var drugstore = await _drugstoresRepository.GetAsync(id);
                ObjectMapper.Map(dto, drugstore);
                await _drugstoresRepository.UpdateAsync(drugstore);
                return ObjectMapper.Map<Drugstore, DrugstoreDto>(drugstore);
            }
            catch (EntityNotFoundException ex)
            {
                throw new UserFriendlyException(L[LocalizerKeys.Errors.DrugstoreNotFound], innerException: ex);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            await _drugstoresRepository.DeleteAsync(id);
        }


    }
}
