using System;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace HQSOFT.SharedInformation.Districts
{
    public class DistrictsAppServiceTests : SharedInformationApplicationTestBase
    {
        private readonly IDistrictsAppService _districtsAppService;
        private readonly IRepository<District, Guid> _districtRepository;

        public DistrictsAppServiceTests()
        {
            _districtsAppService = GetRequiredService<IDistrictsAppService>();
            _districtRepository = GetRequiredService<IRepository<District, Guid>>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Act
            var result = await _districtsAppService.GetListAsync(new GetDistrictsInput());

            // Assert
            result.TotalCount.ShouldBe(2);
            result.Items.Count.ShouldBe(2);
            result.Items.Any(x => x.Id == Guid.Parse("63828f68-6dfa-4f37-9278-1577d0b63f3f")).ShouldBe(true);
            result.Items.Any(x => x.Id == Guid.Parse("1c449594-4652-4e79-9e1a-4c83e17fd0b9")).ShouldBe(true);
        }

        [Fact]
        public async Task GetAsync()
        {
            // Act
            var result = await _districtsAppService.GetAsync(Guid.Parse("63828f68-6dfa-4f37-9278-1577d0b63f3f"));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(Guid.Parse("63828f68-6dfa-4f37-9278-1577d0b63f3f"));
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var input = new DistrictCreateDto
            {
                ProvinceId = Guid.Parse("93349c34-1d4c-4319-97f5-46cb748e5df2"),
                Idx = 375314537,
                DistrictName = "b5fce875863f4e49ab704aba038695537ed0fec729544e6fa80d84ad566b12ab1a5b57ea9a5c4c10a65b913945183434f"
            };

            // Act
            var serviceResult = await _districtsAppService.CreateAsync(input);

            // Assert
            var result = await _districtRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.ProvinceId.ShouldBe(Guid.Parse("93349c34-1d4c-4319-97f5-46cb748e5df2"));
            result.Idx.ShouldBe(375314537);
            result.DistrictName.ShouldBe("b5fce875863f4e49ab704aba038695537ed0fec729544e6fa80d84ad566b12ab1a5b57ea9a5c4c10a65b913945183434f");
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Arrange
            var input = new DistrictUpdateDto()
            {
                ProvinceId = Guid.Parse("640d5f3e-db6b-48ea-9e32-c8584b5ed672"),
                Idx = 1042215284,
                DistrictName = "0e70ec61305a4af7bef961d3476b1508ab26591047874677bfac60069a989cb2a789a3fb0534495b92c8c5bd8395aad97"
            };

            // Act
            var serviceResult = await _districtsAppService.UpdateAsync(Guid.Parse("63828f68-6dfa-4f37-9278-1577d0b63f3f"), input);

            // Assert
            var result = await _districtRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.ProvinceId.ShouldBe(Guid.Parse("640d5f3e-db6b-48ea-9e32-c8584b5ed672"));
            result.Idx.ShouldBe(1042215284);
            result.DistrictName.ShouldBe("0e70ec61305a4af7bef961d3476b1508ab26591047874677bfac60069a989cb2a789a3fb0534495b92c8c5bd8395aad97");
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Act
            await _districtsAppService.DeleteAsync(Guid.Parse("63828f68-6dfa-4f37-9278-1577d0b63f3f"));

            // Assert
            var result = await _districtRepository.FindAsync(c => c.Id == Guid.Parse("63828f68-6dfa-4f37-9278-1577d0b63f3f"));

            result.ShouldBeNull();
        }
    }
}