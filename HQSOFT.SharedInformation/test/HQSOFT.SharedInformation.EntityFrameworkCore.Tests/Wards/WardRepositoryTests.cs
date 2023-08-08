using HQSOFT.SharedInformation.EntityFrameworkCore;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HQSOFT.SharedInformation.Wards
{
    public class WardRepositoryTests : SharedInformationEntityFrameworkCoreTestBase
    {
        private readonly IWardRepository _wardRepository;

        public WardRepositoryTests()
        {
            _wardRepository = GetRequiredService<IWardRepository>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _wardRepository.GetListAsync(
                    districtId: Guid.Parse("dd1278ff-e860-437d-861a-2efe6cc3351f"),
                    wardName: "703314c0a1f74024baf8d62eb6bb8688c3c9551e542e4df187bda854436e10"
                );

                // Assert
                result.Count.ShouldBe(1);
                result.FirstOrDefault().ShouldNotBe(null);
                result.First().Id.ShouldBe(Guid.Parse("1d50aa83-6390-48a2-8788-e771c93ba130"));
            });
        }

        [Fact]
        public async Task GetCountAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _wardRepository.GetCountAsync(
                    districtId: Guid.Parse("ad9b0814-59f3-4ab1-9fa3-fd324d0e889c"),
                    wardName: "14e2b38cbcca49d2af13ddb43aff29a0397f196"
                );

                // Assert
                result.ShouldBe(1);
            });
        }
    }
}