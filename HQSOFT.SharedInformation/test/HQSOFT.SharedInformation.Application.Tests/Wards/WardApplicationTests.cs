using System;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace HQSOFT.SharedInformation.Wards
{
    public class WardsAppServiceTests : SharedInformationApplicationTestBase
    {
        private readonly IWardsAppService _wardsAppService;
        private readonly IRepository<Ward, Guid> _wardRepository;

        public WardsAppServiceTests()
        {
            _wardsAppService = GetRequiredService<IWardsAppService>();
            _wardRepository = GetRequiredService<IRepository<Ward, Guid>>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Act
            var result = await _wardsAppService.GetListAsync(new GetWardsInput());

            // Assert
            result.TotalCount.ShouldBe(2);
            result.Items.Count.ShouldBe(2);
            result.Items.Any(x => x.Id == Guid.Parse("1d50aa83-6390-48a2-8788-e771c93ba130")).ShouldBe(true);
            result.Items.Any(x => x.Id == Guid.Parse("c769d3f7-d8ff-4194-8681-47728c780618")).ShouldBe(true);
        }

        [Fact]
        public async Task GetAsync()
        {
            // Act
            var result = await _wardsAppService.GetAsync(Guid.Parse("1d50aa83-6390-48a2-8788-e771c93ba130"));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(Guid.Parse("1d50aa83-6390-48a2-8788-e771c93ba130"));
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var input = new WardCreateDto
            {
                DistrictId = Guid.Parse("12fd775b-ca14-417a-9956-e5e025593e7e"),
                Idx = 907535979,
                WardName = "ad501207c03a4c579e3ed8b6884e8b82db70bcf1dc0e4e3281084c44e1757b108cb5303dc79a43599"
            };

            // Act
            var serviceResult = await _wardsAppService.CreateAsync(input);

            // Assert
            var result = await _wardRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.DistrictId.ShouldBe(Guid.Parse("12fd775b-ca14-417a-9956-e5e025593e7e"));
            result.Idx.ShouldBe(907535979);
            result.WardName.ShouldBe("ad501207c03a4c579e3ed8b6884e8b82db70bcf1dc0e4e3281084c44e1757b108cb5303dc79a43599");
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Arrange
            var input = new WardUpdateDto()
            {
                DistrictId = Guid.Parse("beb2d9de-2ac8-4c82-8f24-c1c04295cdde"),
                Idx = 1294311328,
                WardName = "e2775ab3cba1407fa6c45ff164172563c9504270b2994c9d9513461bd67bed94a76e011397fb4828a462e8"
            };

            // Act
            var serviceResult = await _wardsAppService.UpdateAsync(Guid.Parse("1d50aa83-6390-48a2-8788-e771c93ba130"), input);

            // Assert
            var result = await _wardRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.DistrictId.ShouldBe(Guid.Parse("beb2d9de-2ac8-4c82-8f24-c1c04295cdde"));
            result.Idx.ShouldBe(1294311328);
            result.WardName.ShouldBe("e2775ab3cba1407fa6c45ff164172563c9504270b2994c9d9513461bd67bed94a76e011397fb4828a462e8");
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Act
            await _wardsAppService.DeleteAsync(Guid.Parse("1d50aa83-6390-48a2-8788-e771c93ba130"));

            // Assert
            var result = await _wardRepository.FindAsync(c => c.Id == Guid.Parse("1d50aa83-6390-48a2-8788-e771c93ba130"));

            result.ShouldBeNull();
        }
    }
}