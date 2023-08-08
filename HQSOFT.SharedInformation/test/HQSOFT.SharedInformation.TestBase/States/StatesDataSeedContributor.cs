using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using HQSOFT.SharedInformation.States;

namespace HQSOFT.SharedInformation.States
{
    public class StatesDataSeedContributor : IDataSeedContributor, ISingletonDependency
    {
        private bool IsSeeded = false;
        private readonly IStateRepository _stateRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public StatesDataSeedContributor(IStateRepository stateRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _stateRepository = stateRepository;
            _unitOfWorkManager = unitOfWorkManager;

        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (IsSeeded)
            {
                return;
            }

            await _stateRepository.InsertAsync(new State
            (
                id: Guid.Parse("9888de96-bfe2-4da2-9673-a273fc476c9a"),
                countryId: Guid.Parse("a46f6a43-6fd0-4303-8be1-3f086f96a7ac"),
                idx: 241947628,
                stateCode: "dc25262d011c4425bead6f5",
                stateName: "ea50b3864bc547668d1c5d87f1d35451c057b63bf37f426ea69cd9486b422e10d63fab4996c548cf85e8c1e2afdcf0bbe"
            ));

            await _stateRepository.InsertAsync(new State
            (
                id: Guid.Parse("30633f22-908a-4bb0-ac29-f5098ef75463"),
                countryId: Guid.Parse("11b096cd-3ec3-4414-9291-68534cbf42d4"),
                idx: 424348778,
                stateCode: "66bda961f6984f30a94b2f0ae7421ccd502e0c5398454646bcb3c4502d467d",
                stateName: "97ed840a4a5843209f67a0323292b45257e630a3a2154f55996e389ee56f422c1e5f7398d1f74e92b03b0590"
            ));

            await _unitOfWorkManager.Current.SaveChangesAsync();

            IsSeeded = true;
        }
    }
}