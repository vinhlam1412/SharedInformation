using System;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace HQSOFT.SharedInformation.Provinces
{
    public class ProvincesAppServiceTests : SharedInformationApplicationTestBase
    {
        private readonly IProvincesAppService _provincesAppService;
        private readonly IRepository<Province, Guid> _provinceRepository;

        public ProvincesAppServiceTests()
        {
            _provincesAppService = GetRequiredService<IProvincesAppService>();
            _provinceRepository = GetRequiredService<IRepository<Province, Guid>>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Act
            var result = await _provincesAppService.GetListAsync(new GetProvincesInput());

            // Assert
            result.TotalCount.ShouldBe(2);
            result.Items.Count.ShouldBe(2);
            result.Items.Any(x => x.Id == Guid.Parse("038e28bb-6c4a-4b8f-8d44-5945dcdd59f2")).ShouldBe(true);
            result.Items.Any(x => x.Id == Guid.Parse("6963acd8-3ff9-43d1-a289-3face004d144")).ShouldBe(true);
        }

        [Fact]
        public async Task GetAsync()
        {
            // Act
            var result = await _provincesAppService.GetAsync(Guid.Parse("038e28bb-6c4a-4b8f-8d44-5945dcdd59f2"));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(Guid.Parse("038e28bb-6c4a-4b8f-8d44-5945dcdd59f2"));
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var input = new ProvinceCreateDto
            {
                Idx = 89002544,
                CountryId = Guid.Parse("b7eb3838-0a14-4412-9937-9de2cbfffad6"),
                ProvinceCode = "89588625c45e4433a4f562c6eb4e785ff3a7519b62b2428e851f0be4f41597a309e7599099694ff487cbc4143a",
                ProvinceName = "fb7ebbadebc64ce09dd19"
            };

            // Act
            var serviceResult = await _provincesAppService.CreateAsync(input);

            // Assert
            var result = await _provinceRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.Idx.ShouldBe(89002544);
            result.CountryId.ShouldBe(Guid.Parse("b7eb3838-0a14-4412-9937-9de2cbfffad6"));
            result.ProvinceCode.ShouldBe("89588625c45e4433a4f562c6eb4e785ff3a7519b62b2428e851f0be4f41597a309e7599099694ff487cbc4143a");
            result.ProvinceName.ShouldBe("fb7ebbadebc64ce09dd19");
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Arrange
            var input = new ProvinceUpdateDto()
            {
                Idx = 1223328116,
                CountryId = Guid.Parse("dfa2c17c-f758-4ff7-b200-cab3c57a12db"),
                ProvinceCode = "40f599bb107d4d31b02db96a99dcf5dda9b83a58939d43698d23886dbdd7c33d96b97d3e83ff47c0ae740e8eb8c8b5d51",
                ProvinceName = "b23f0172f715440a8e827b907e3be2c31f8bfec6dfc8409aaf29983ef"
            };

            // Act
            var serviceResult = await _provincesAppService.UpdateAsync(Guid.Parse("038e28bb-6c4a-4b8f-8d44-5945dcdd59f2"), input);

            // Assert
            var result = await _provinceRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.Idx.ShouldBe(1223328116);
            result.CountryId.ShouldBe(Guid.Parse("dfa2c17c-f758-4ff7-b200-cab3c57a12db"));
            result.ProvinceCode.ShouldBe("40f599bb107d4d31b02db96a99dcf5dda9b83a58939d43698d23886dbdd7c33d96b97d3e83ff47c0ae740e8eb8c8b5d51");
            result.ProvinceName.ShouldBe("b23f0172f715440a8e827b907e3be2c31f8bfec6dfc8409aaf29983ef");
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Act
            await _provincesAppService.DeleteAsync(Guid.Parse("038e28bb-6c4a-4b8f-8d44-5945dcdd59f2"));

            // Assert
            var result = await _provinceRepository.FindAsync(c => c.Id == Guid.Parse("038e28bb-6c4a-4b8f-8d44-5945dcdd59f2"));

            result.ShouldBeNull();
        }
    }
}