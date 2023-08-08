using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using HQSOFT.SharedInformation.Districts;
using HQSOFT.SharedInformation.EntityFrameworkCore;
using Xunit;

namespace HQSOFT.SharedInformation.Districts
{
    public class DistrictRepositoryTests : SharedInformationEntityFrameworkCoreTestBase
    {
        private readonly IDistrictRepository _districtRepository;

        public DistrictRepositoryTests()
        {
            _districtRepository = GetRequiredService<IDistrictRepository>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _districtRepository.GetListAsync(
                    provinceId: Guid.Parse("ab2d7cf5-717f-4fd0-ad33-5f29e0d3e1da"),
                    districtName: "9f8e5f1bbd5f40cc8edea8dd35b1f7aa8a93a8278d6741dd9418539defd03e6df42d373df25645e591bac7e256a1c89f0d"
                );

                // Assert
                result.Count.ShouldBe(1);
                result.FirstOrDefault().ShouldNotBe(null);
                result.First().Id.ShouldBe(Guid.Parse("63828f68-6dfa-4f37-9278-1577d0b63f3f"));
            });
        }

        [Fact]
        public async Task GetCountAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _districtRepository.GetCountAsync(
                    provinceId: Guid.Parse("87e6fe8a-cbb6-47cc-8fa7-da28afd7f448"),
                    districtName: "dca204d0281b4f0db0afa25fe2887f1fd18da976a8014340a2f558b8dfb33ff76b8fc6aacd"
                );

                // Assert
                result.ShouldBe(1);
            });
        }
    }
}