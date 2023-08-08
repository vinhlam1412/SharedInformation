using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using HQSOFT.SharedInformation.Wards;

namespace HQSOFT.SharedInformation.Wards
{
    public class WardsDataSeedContributor : IDataSeedContributor, ISingletonDependency
    {
        private bool IsSeeded = false;
        private readonly IWardRepository _wardRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public WardsDataSeedContributor(IWardRepository wardRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _wardRepository = wardRepository;
            _unitOfWorkManager = unitOfWorkManager;

        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (IsSeeded)
            {
                return;
            }

            await _wardRepository.InsertAsync(new Ward
            (
                id: Guid.Parse("1d50aa83-6390-48a2-8788-e771c93ba130"),
                districtId: Guid.Parse("dd1278ff-e860-437d-861a-2efe6cc3351f"),
                idx: 348171534,
                wardName: "703314c0a1f74024baf8d62eb6bb8688c3c9551e542e4df187bda854436e10"
            ));

            await _wardRepository.InsertAsync(new Ward
            (
                id: Guid.Parse("c769d3f7-d8ff-4194-8681-47728c780618"),
                districtId: Guid.Parse("ad9b0814-59f3-4ab1-9fa3-fd324d0e889c"),
                idx: 134806523,
                wardName: "14e2b38cbcca49d2af13ddb43aff29a0397f196"
            ));

            await _unitOfWorkManager.Current.SaveChangesAsync();

            IsSeeded = true;
        }
    }
}