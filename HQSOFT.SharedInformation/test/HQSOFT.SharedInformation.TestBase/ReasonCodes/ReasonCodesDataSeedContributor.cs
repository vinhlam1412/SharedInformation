using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using HQSOFT.SharedInformation.ReasonCodes;

namespace HQSOFT.SharedInformation.ReasonCodes
{
    public class ReasonCodesDataSeedContributor : IDataSeedContributor, ISingletonDependency
    {
        private bool IsSeeded = false;
        private readonly IReasonCodeRepository _reasonCodeRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public ReasonCodesDataSeedContributor(IReasonCodeRepository reasonCodeRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _reasonCodeRepository = reasonCodeRepository;
            _unitOfWorkManager = unitOfWorkManager;

        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (IsSeeded)
            {
                return;
            }

            await _reasonCodeRepository.InsertAsync(new ReasonCode
            (
                id: Guid.Parse("8f681074-7180-44ba-8923-0d36be08c35e"),
                code: "3686071b820b4ec9989e43b9fc27aa136bd781851e9f4e66960a53fb11af3d91a3",
                type: "ba6d8dd76fb34a8cbe46cfb25f06b127bdcc06087b88451",
                description: "6215e80031bc4b3bbbc0641c99fd9893392081234594410ebaf9a71edba3f4699516990aa0fd49adbd2",
                accountId: Guid.Parse("be5cbb3b-1a21-477e-baa2-8efc82836d91")
            ));

            await _reasonCodeRepository.InsertAsync(new ReasonCode
            (
                id: Guid.Parse("0a537bdf-a3c3-48af-928e-9d481d1c663e"),
                code: "d732c3539dba4b208e6c75685bb24e99e02f5f9993c2400188006f3575f2d702f6c472a23964",
                type: "169214fa6f044ddabc6e6309ad433601088f03d976084dc9a4bf285fb0e650f012a4d639b24e420ab53e53b1253957b39f",
                description: "1b7207cea09c4689b702fcb5a7c",
                accountId: Guid.Parse("711e1eb9-10c1-4577-9ef7-6554df1d4ef5")
            ));

            await _unitOfWorkManager.Current.SaveChangesAsync();

            IsSeeded = true;
        }
    }
}