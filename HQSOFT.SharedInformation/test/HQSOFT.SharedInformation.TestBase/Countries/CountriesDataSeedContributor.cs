using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using HQSOFT.SharedInformation.Countries;

namespace HQSOFT.SharedInformation.Countries
{
    public class CountriesDataSeedContributor : IDataSeedContributor, ISingletonDependency
    {
        private bool IsSeeded = false;
        private readonly ICountryRepository _countryRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public CountriesDataSeedContributor(ICountryRepository countryRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _countryRepository = countryRepository;
            _unitOfWorkManager = unitOfWorkManager;

        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (IsSeeded)
            {
                return;
            }

            await _countryRepository.InsertAsync(new Country
            (
                id: Guid.Parse("367f19b7-2215-45a1-9544-084b6bfa72aa"),
                code: "0e6bfa7360334765947",
                description: "f1f90dc226dc40e0b2dff60747176c5bfeb3e2de93fe4b48b673459",
                dateFormat: "fc5fe185ee374f4e93aa1870f024f81e7a64b938919a436a840f421ca558e9e4c12ba1b318ea4e",
                timeFormat: "5c0c12d69d7043108d43aaf6dc6f8d89f180107de17645b",
                timeZone: "257bb6acd23249aa88d1c3b99a42aebf394617d3f8cd4092975f9",
                idx: 1608692313
            ));

            await _countryRepository.InsertAsync(new Country
            (
                id: Guid.Parse("f279e76c-f78a-462e-b3e7-8bcac631de22"),
                code: "4ebb5f355eda4731a972d51ea42062ec3fac224aa57f4f39903efeae90237076660f32c764d141ca81195d9bae40b10b",
                description: "cfa18f2d1aee4163ad30b1a68cb010cad9dcfd1b73ad4cb98c03603b0bf2186712de9682df5041bdae1e33",
                dateFormat: "49bfacfc63d94248a4e5a23967ac2571da0fa33cd2",
                timeFormat: "fe66974461cb4a84a9325bb76cf702684285e059de0c44b8a6f53587810b8d2b774583c60c",
                timeZone: "f4aa7b485a96445399636890d144213cd5cc",
                idx: 902180188
            ));

            await _unitOfWorkManager.Current.SaveChangesAsync();

            IsSeeded = true;
        }
    }
}