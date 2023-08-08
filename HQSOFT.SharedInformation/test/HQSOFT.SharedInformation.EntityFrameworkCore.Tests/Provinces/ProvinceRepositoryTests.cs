using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using HQSOFT.SharedInformation.Provinces;
using HQSOFT.SharedInformation.EntityFrameworkCore;
using Xunit;

namespace HQSOFT.SharedInformation.Provinces
{
    public class ProvinceRepositoryTests : SharedInformationEntityFrameworkCoreTestBase
    {
        private readonly IProvinceRepository _provinceRepository;

        public ProvinceRepositoryTests()
        {
            _provinceRepository = GetRequiredService<IProvinceRepository>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _provinceRepository.GetListAsync(
                    countryId: Guid.Parse("d6230a04-7367-4c77-818f-40df4b7b8461"),
                    provinceCode: "0f20dc8093454a19ab2abbbc33c2934",
                    provinceName: "16438b66cd0147e98790391421155b2e00e460c9934f420787"
                );

                // Assert
                result.Count.ShouldBe(1);
                result.FirstOrDefault().ShouldNotBe(null);
                result.First().Id.ShouldBe(Guid.Parse("038e28bb-6c4a-4b8f-8d44-5945dcdd59f2"));
            });
        }

        [Fact]
        public async Task GetCountAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _provinceRepository.GetCountAsync(
                    countryId: Guid.Parse("1f467fd3-db56-4729-93e8-5b524ca269b4"),
                    provinceCode: "4089097f05c14b29a30f304c528c10",
                    provinceName: "61ee27ee6a2a44e5b3cb90bba84513e"
                );

                // Assert
                result.ShouldBe(1);
            });
        }
    }
}