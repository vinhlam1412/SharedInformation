using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using HQSOFT.SharedInformation.Districts;

namespace HQSOFT.SharedInformation.Districts
{
    public class DistrictsDataSeedContributor : IDataSeedContributor, ISingletonDependency
    {
        private bool IsSeeded = false;
        private readonly IDistrictRepository _districtRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public DistrictsDataSeedContributor(IDistrictRepository districtRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _districtRepository = districtRepository;
            _unitOfWorkManager = unitOfWorkManager;

        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (IsSeeded)
            {
                return;
            }

            await _districtRepository.InsertAsync(new District
            (
                id: Guid.Parse("63828f68-6dfa-4f37-9278-1577d0b63f3f"),
                provinceId: Guid.Parse("ab2d7cf5-717f-4fd0-ad33-5f29e0d3e1da"),
                idx: 1908462590,
                districtName: "9f8e5f1bbd5f40cc8edea8dd35b1f7aa8a93a8278d6741dd9418539defd03e6df42d373df25645e591bac7e256a1c89f0d"
            ));

            await _districtRepository.InsertAsync(new District
            (
                id: Guid.Parse("1c449594-4652-4e79-9e1a-4c83e17fd0b9"),
                provinceId: Guid.Parse("87e6fe8a-cbb6-47cc-8fa7-da28afd7f448"),
                idx: 378012205,
                districtName: "dca204d0281b4f0db0afa25fe2887f1fd18da976a8014340a2f558b8dfb33ff76b8fc6aacd"
            ));

            await _unitOfWorkManager.Current.SaveChangesAsync();

            IsSeeded = true;
        }
    }
}