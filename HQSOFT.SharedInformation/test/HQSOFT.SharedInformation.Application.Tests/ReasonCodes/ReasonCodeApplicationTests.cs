using System;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace HQSOFT.SharedInformation.ReasonCodes
{
    public class ReasonCodesAppServiceTests : SharedInformationApplicationTestBase
    {
        private readonly IReasonCodesAppService _reasonCodesAppService;
        private readonly IRepository<ReasonCode, Guid> _reasonCodeRepository;

        public ReasonCodesAppServiceTests()
        {
            _reasonCodesAppService = GetRequiredService<IReasonCodesAppService>();
            _reasonCodeRepository = GetRequiredService<IRepository<ReasonCode, Guid>>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Act
            var result = await _reasonCodesAppService.GetListAsync(new GetReasonCodesInput());

            // Assert
            result.TotalCount.ShouldBe(2);
            result.Items.Count.ShouldBe(2);
            result.Items.Any(x => x.Id == Guid.Parse("8f681074-7180-44ba-8923-0d36be08c35e")).ShouldBe(true);
            result.Items.Any(x => x.Id == Guid.Parse("0a537bdf-a3c3-48af-928e-9d481d1c663e")).ShouldBe(true);
        }

        [Fact]
        public async Task GetAsync()
        {
            // Act
            var result = await _reasonCodesAppService.GetAsync(Guid.Parse("8f681074-7180-44ba-8923-0d36be08c35e"));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(Guid.Parse("8f681074-7180-44ba-8923-0d36be08c35e"));
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var input = new ReasonCodeCreateDto
            {
                Code = "ca84891b8db1489d9f713056fc16b5976139764cc0834274b337308d967be10e1da005bf720f4aa0a353c42a9e984bc5eff",
                Type = "ccaa022cbc5145a69c1",
                Description = "a80f54222e924b3d909b55b3ec7fd28e75cafb5bb9b0407ca974",
                AccountId = Guid.Parse("0610f49a-04d2-46ef-b554-76f638e344fb")
            };

            // Act
            var serviceResult = await _reasonCodesAppService.CreateAsync(input);

            // Assert
            var result = await _reasonCodeRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.Code.ShouldBe("ca84891b8db1489d9f713056fc16b5976139764cc0834274b337308d967be10e1da005bf720f4aa0a353c42a9e984bc5eff");
            result.Type.ShouldBe("ccaa022cbc5145a69c1");
            result.Description.ShouldBe("a80f54222e924b3d909b55b3ec7fd28e75cafb5bb9b0407ca974");
            result.AccountId.ShouldBe(Guid.Parse("0610f49a-04d2-46ef-b554-76f638e344fb"));
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Arrange
            var input = new ReasonCodeUpdateDto()
            {
                Code = "5f4d2ff2",
                Type = "06864639614a4",
                Description = "53f8930b78b145dbb96e3a",
                AccountId = Guid.Parse("12e4627d-b9cc-44dc-9c1d-3bbb9f4ac299")
            };

            // Act
            var serviceResult = await _reasonCodesAppService.UpdateAsync(Guid.Parse("8f681074-7180-44ba-8923-0d36be08c35e"), input);

            // Assert
            var result = await _reasonCodeRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.Code.ShouldBe("5f4d2ff2");
            result.Type.ShouldBe("06864639614a4");
            result.Description.ShouldBe("53f8930b78b145dbb96e3a");
            result.AccountId.ShouldBe(Guid.Parse("12e4627d-b9cc-44dc-9c1d-3bbb9f4ac299"));
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Act
            await _reasonCodesAppService.DeleteAsync(Guid.Parse("8f681074-7180-44ba-8923-0d36be08c35e"));

            // Assert
            var result = await _reasonCodeRepository.FindAsync(c => c.Id == Guid.Parse("8f681074-7180-44ba-8923-0d36be08c35e"));

            result.ShouldBeNull();
        }
    }
}