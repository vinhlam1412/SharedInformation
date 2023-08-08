using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using HQSOFT.SharedInformation.Countries;
using HQSOFT.SharedInformation.EntityFrameworkCore;

namespace HQSOFT.SharedInformation.Companies
{
    public class EfCoreCompanyRepository : EfCoreRepository<SharedInformationDbContext, Company, Guid>, ICompanyRepository
    {
        public EfCoreCompanyRepository(IDbContextProvider<SharedInformationDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public async Task<CompanyWithNavigationProperties> GetListNavigationAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            return (await GetDbSetAsync()).Where(b => b.Id == id)
                .Select(salesOrder => new CompanyWithNavigationProperties
                {
                    Company = salesOrder,
                    Country = dbContext.Set<Country>().FirstOrDefault(c => c.Id == salesOrder.CountryId)
                }).FirstOrDefault();
        }
        public async Task<List<Company>> GetListAsync(
            string filterText = null,
            string abbreviation = null,
            string companyName = null,
            Guid? defaultCurrency = null,
            string taxID = null,
            Guid? countryId = null,
            bool? isGroup = null,
            Guid? parentCompany = null,
            string address1 = null,
            string address2 = null,
            string email = null,
            string web = null,
            string phone1 = null,
            string phone2 = null,
            Guid? stateId = null,
            Guid? provinceId = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, abbreviation, companyName, defaultCurrency, taxID, countryId, isGroup, parentCompany, address1, address2, email, web, phone1, phone2, stateId, provinceId);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? CompanyConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public async Task<long> GetCountAsync(
            string filterText = null,
            string abbreviation = null,
            string companyName = null,
            Guid? defaultCurrency = null,
            string taxID = null,
            Guid? countryId = null,
            bool? isGroup = null,
            Guid? parentCompany = null,
            string address1 = null,
            string address2 = null,
            string email = null,
            string web = null,
            string phone1 = null,
            string phone2 = null,
            Guid? stateId = null,
            Guid? provinceId = null,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetDbSetAsync()), filterText, abbreviation, companyName, defaultCurrency, taxID, countryId, isGroup, parentCompany, address1, address2, email, web, phone1, phone2, stateId, provinceId);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<Company> ApplyFilter(
            IQueryable<Company> query,
            string filterText,
            string abbreviation = null,
            string companyName = null,
            Guid? defaultCurrency = null,
            string taxID = null,
            Guid? countryId = null,
            bool? isGroup = null,
            Guid? parentCompany = null,
            string address1 = null,
            string address2 = null,
            string email = null,
            string web = null,
            string phone1 = null,
            string phone2 = null,
            Guid? stateId = null,
            Guid? provinceId = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Abbreviation.ToLower().Contains(filterText.ToLower()) || e.CompanyName.ToLower().Contains(filterText.ToLower()) || e.TaxID.ToLower().Contains(filterText.ToLower()) || e.Address1.ToLower().Contains(filterText.ToLower()) || e.Address2.ToLower().Contains(filterText.ToLower()) || e.Email.ToLower().Contains(filterText.ToLower()) || e.Web.ToLower().Contains(filterText.ToLower()) || e.Phone1.ToLower().Contains(filterText.ToLower()) || e.Phone2.ToLower().Contains(filterText.ToLower()))
                    .WhereIf(!string.IsNullOrWhiteSpace(abbreviation), e => e.Abbreviation.ToLower().Contains(abbreviation.ToLower()))
                    .WhereIf(!string.IsNullOrWhiteSpace(companyName), e => e.CompanyName.ToLower().Contains(companyName.ToLower()))
                    .WhereIf(defaultCurrency.HasValue, e => e.DefaultCurrency == defaultCurrency)
                    .WhereIf(!string.IsNullOrWhiteSpace(taxID), e => e.TaxID.ToLower().Contains(taxID.ToLower()))
                    .WhereIf(countryId.HasValue, e => e.CountryId == countryId)
                    .WhereIf(isGroup.HasValue, e => e.IsGroup == isGroup)
                    .WhereIf(parentCompany.HasValue, e => e.ParentCompany == parentCompany)
                    .WhereIf(!string.IsNullOrWhiteSpace(address1), e => e.Address1.ToLower().Contains(address1.ToLower()))
                    .WhereIf(!string.IsNullOrWhiteSpace(address2), e => e.Address2.ToLower().Contains(address2.ToLower()))
                    .WhereIf(!string.IsNullOrWhiteSpace(email), e => e.Email.ToLower().Contains(email.ToLower()))
                    .WhereIf(!string.IsNullOrWhiteSpace(web), e => e.Web.ToLower().Contains(web.ToLower()))
                    .WhereIf(!string.IsNullOrWhiteSpace(phone1), e => e.Phone1.ToLower().Contains(phone1.ToLower()))
                    .WhereIf(!string.IsNullOrWhiteSpace(phone2), e => e.Phone2.ToLower().Contains(phone2.ToLower()))
                    .WhereIf(stateId.HasValue, e => e.StateId == stateId)
                    .WhereIf(provinceId.HasValue, e => e.ProvinceId == provinceId);
        }
    }
}