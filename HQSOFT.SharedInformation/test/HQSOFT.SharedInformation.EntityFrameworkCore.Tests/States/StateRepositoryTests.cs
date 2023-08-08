using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using HQSOFT.SharedInformation.States;
using HQSOFT.SharedInformation.EntityFrameworkCore;
using Xunit;

namespace HQSOFT.SharedInformation.States
{
    public class StateRepositoryTests : SharedInformationEntityFrameworkCoreTestBase
    {
        private readonly IStateRepository _stateRepository;

        public StateRepositoryTests()
        {
            _stateRepository = GetRequiredService<IStateRepository>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _stateRepository.GetListAsync(
                    countryId: Guid.Parse("a46f6a43-6fd0-4303-8be1-3f086f96a7ac"),
                    stateCode: "dc25262d011c4425bead6f5",
                    stateName: "ea50b3864bc547668d1c5d87f1d35451c057b63bf37f426ea69cd9486b422e10d63fab4996c548cf85e8c1e2afdcf0bbe"
                );

                // Assert
                result.Count.ShouldBe(1);
                result.FirstOrDefault().ShouldNotBe(null);
                result.First().Id.ShouldBe(Guid.Parse("9888de96-bfe2-4da2-9673-a273fc476c9a"));
            });
        }

        [Fact]
        public async Task GetCountAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _stateRepository.GetCountAsync(
                    countryId: Guid.Parse("11b096cd-3ec3-4414-9291-68534cbf42d4"),
                    stateCode: "66bda961f6984f30a94b2f0ae7421ccd502e0c5398454646bcb3c4502d467d",
                    stateName: "97ed840a4a5843209f67a0323292b45257e630a3a2154f55996e389ee56f422c1e5f7398d1f74e92b03b0590"
                );

                // Assert
                result.ShouldBe(1);
            });
        }
    }
}