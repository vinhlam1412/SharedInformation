using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using HQSOFT.SharedInformation.Provinces;

namespace HQSOFT.SharedInformation.Provinces
{
    public class ProvincesDataSeedContributor : IDataSeedContributor, ISingletonDependency
    {
        private bool IsSeeded = false;
        private readonly IProvinceRepository _provinceRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public ProvincesDataSeedContributor(IProvinceRepository provinceRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _provinceRepository = provinceRepository;
            _unitOfWorkManager = unitOfWorkManager;

        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (IsSeeded)
            {
                return;
            }

            await _provinceRepository.InsertAsync(new Province
            (
                id: Guid.Parse("038e28bb-6c4a-4b8f-8d44-5945dcdd59f2"),
                idx: 1775639826,
                countryId: Guid.Parse("d6230a04-7367-4c77-818f-40df4b7b8461"),
                provinceCode: "0f20dc8093454a19ab2abbbc33c2934",
                provinceName: "16438b66cd0147e98790391421155b2e00e460c9934f420787"
            ));

            await _provinceRepository.InsertAsync(new Province
            (
                id: Guid.Parse("6963acd8-3ff9-43d1-a289-3face004d144"),
                idx: 1914030532,
                countryId: Guid.Parse("1f467fd3-db56-4729-93e8-5b524ca269b4"),
                provinceCode: "4089097f05c14b29a30f304c528c10",
                provinceName: "61ee27ee6a2a44e5b3cb90bba84513e"
            ));

            await _unitOfWorkManager.Current.SaveChangesAsync();

            IsSeeded = true;
        }
    }
}