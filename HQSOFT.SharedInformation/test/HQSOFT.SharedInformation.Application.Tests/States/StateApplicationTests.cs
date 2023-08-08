using System;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace HQSOFT.SharedInformation.States
{
    public class StatesAppServiceTests : SharedInformationApplicationTestBase
    {
        private readonly IStatesAppService _statesAppService;
        private readonly IRepository<State, Guid> _stateRepository;

        public StatesAppServiceTests()
        {
            _statesAppService = GetRequiredService<IStatesAppService>();
            _stateRepository = GetRequiredService<IRepository<State, Guid>>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Act
            var result = await _statesAppService.GetListAsync(new GetStatesInput());

            // Assert
            result.TotalCount.ShouldBe(2);
            result.Items.Count.ShouldBe(2);
            result.Items.Any(x => x.Id == Guid.Parse("9888de96-bfe2-4da2-9673-a273fc476c9a")).ShouldBe(true);
            result.Items.Any(x => x.Id == Guid.Parse("30633f22-908a-4bb0-ac29-f5098ef75463")).ShouldBe(true);
        }

        [Fact]
        public async Task GetAsync()
        {
            // Act
            var result = await _statesAppService.GetAsync(Guid.Parse("9888de96-bfe2-4da2-9673-a273fc476c9a"));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(Guid.Parse("9888de96-bfe2-4da2-9673-a273fc476c9a"));
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var input = new StateCreateDto
            {
                CountryId = Guid.Parse("432bd324-c3b9-42c0-a63f-ac1d97113b2c"),
                Idx = 348947531,
                StateCode = "c099adc4e56f4f8a85aefd58c9c8e9f603f839914",
                StateName = "918b1c1d17d248d49a638c0f01fbd32f5c3788d8ab89479c95170c06ecc7da417e879a0e18a049b59e7bc60e5ab7dd4"
            };

            // Act
            var serviceResult = await _statesAppService.CreateAsync(input);

            // Assert
            var result = await _stateRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.CountryId.ShouldBe(Guid.Parse("432bd324-c3b9-42c0-a63f-ac1d97113b2c"));
            result.Idx.ShouldBe(348947531);
            result.StateCode.ShouldBe("c099adc4e56f4f8a85aefd58c9c8e9f603f839914");
            result.StateName.ShouldBe("918b1c1d17d248d49a638c0f01fbd32f5c3788d8ab89479c95170c06ecc7da417e879a0e18a049b59e7bc60e5ab7dd4");
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Arrange
            var input = new StateUpdateDto()
            {
                CountryId = Guid.Parse("9f8eb088-2e41-4dc9-93cd-def2d34e8570"),
                Idx = 2114383144,
                StateCode = "773c46fdbe00445f94b18083254c1612fc90db652b9a4616980a3e4c90f879662f348e",
                StateName = "a7105f07c0814da997b5bf86577f8b00fe2f88705edd40e9a5005d21bb"
            };

            // Act
            var serviceResult = await _statesAppService.UpdateAsync(Guid.Parse("9888de96-bfe2-4da2-9673-a273fc476c9a"), input);

            // Assert
            var result = await _stateRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.CountryId.ShouldBe(Guid.Parse("9f8eb088-2e41-4dc9-93cd-def2d34e8570"));
            result.Idx.ShouldBe(2114383144);
            result.StateCode.ShouldBe("773c46fdbe00445f94b18083254c1612fc90db652b9a4616980a3e4c90f879662f348e");
            result.StateName.ShouldBe("a7105f07c0814da997b5bf86577f8b00fe2f88705edd40e9a5005d21bb");
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Act
            await _statesAppService.DeleteAsync(Guid.Parse("9888de96-bfe2-4da2-9673-a273fc476c9a"));

            // Assert
            var result = await _stateRepository.FindAsync(c => c.Id == Guid.Parse("9888de96-bfe2-4da2-9673-a273fc476c9a"));

            result.ShouldBeNull();
        }
    }
}