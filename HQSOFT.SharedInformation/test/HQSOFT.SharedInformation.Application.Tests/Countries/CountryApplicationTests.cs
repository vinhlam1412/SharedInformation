using System;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace HQSOFT.SharedInformation.Countries
{
    public class CountriesAppServiceTests : SharedInformationApplicationTestBase
    {
        private readonly ICountriesAppService _countriesAppService;
        private readonly IRepository<Country, Guid> _countryRepository;

        public CountriesAppServiceTests()
        {
            _countriesAppService = GetRequiredService<ICountriesAppService>();
            _countryRepository = GetRequiredService<IRepository<Country, Guid>>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Act
            var result = await _countriesAppService.GetListAsync(new GetCountriesInput());

            // Assert
            result.TotalCount.ShouldBe(2);
            result.Items.Count.ShouldBe(2);
            result.Items.Any(x => x.Id == Guid.Parse("367f19b7-2215-45a1-9544-084b6bfa72aa")).ShouldBe(true);
            result.Items.Any(x => x.Id == Guid.Parse("f279e76c-f78a-462e-b3e7-8bcac631de22")).ShouldBe(true);
        }

        [Fact]
        public async Task GetAsync()
        {
            // Act
            var result = await _countriesAppService.GetAsync(Guid.Parse("367f19b7-2215-45a1-9544-084b6bfa72aa"));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(Guid.Parse("367f19b7-2215-45a1-9544-084b6bfa72aa"));
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var input = new CountryCreateDto
            {
                Code = "eac17c203aca439284f7718b090a1d82b128268c3f7d481a90",
                Description = "8a143adc3143406db21",
                DateFormat = "0e0c89b89b4a4f7b984",
                TimeFormat = "2c67684f33ee495d8f44dd22cf00822c525d1",
                TimeZone = "7609a0dba235443ca1daa343499f332e",
                Idx = 100561348
            };

            // Act
            var serviceResult = await _countriesAppService.CreateAsync(input);

            // Assert
            var result = await _countryRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.Code.ShouldBe("eac17c203aca439284f7718b090a1d82b128268c3f7d481a90");
            result.Description.ShouldBe("8a143adc3143406db21");
            result.DateFormat.ShouldBe("0e0c89b89b4a4f7b984");
            result.TimeFormat.ShouldBe("2c67684f33ee495d8f44dd22cf00822c525d1");
            result.TimeZone.ShouldBe("7609a0dba235443ca1daa343499f332e");
            result.Idx.ShouldBe(100561348);
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Arrange
            var input = new CountryUpdateDto()
            {
                Code = "3f061ba4b2d54060829d62af2e5fdc42cd9f0d69178142408b4ffe180407742479048e3bbb",
                Description = "0c4cc759ca9645858e7c74e62c531061c993187f98ac42c9b3405676",
                DateFormat = "10a7236fd4d241f099c1dc5cac5ae1e8bd400b4d0be3452d906776eaae7f3ee8668636bd72",
                TimeFormat = "9730359bde9",
                TimeZone = "d8f6a0225cec4f799683d9b8877e2bd1bca6538f8be74b819752de9d27bd45f5357b7d1e68be47",
                Idx = 197686505
            };

            // Act
            var serviceResult = await _countriesAppService.UpdateAsync(Guid.Parse("367f19b7-2215-45a1-9544-084b6bfa72aa"), input);

            // Assert
            var result = await _countryRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.Code.ShouldBe("3f061ba4b2d54060829d62af2e5fdc42cd9f0d69178142408b4ffe180407742479048e3bbb");
            result.Description.ShouldBe("0c4cc759ca9645858e7c74e62c531061c993187f98ac42c9b3405676");
            result.DateFormat.ShouldBe("10a7236fd4d241f099c1dc5cac5ae1e8bd400b4d0be3452d906776eaae7f3ee8668636bd72");
            result.TimeFormat.ShouldBe("9730359bde9");
            result.TimeZone.ShouldBe("d8f6a0225cec4f799683d9b8877e2bd1bca6538f8be74b819752de9d27bd45f5357b7d1e68be47");
            result.Idx.ShouldBe(197686505);
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Act
            await _countriesAppService.DeleteAsync(Guid.Parse("367f19b7-2215-45a1-9544-084b6bfa72aa"));

            // Assert
            var result = await _countryRepository.FindAsync(c => c.Id == Guid.Parse("367f19b7-2215-45a1-9544-084b6bfa72aa"));

            result.ShouldBeNull();
        }
    }
}