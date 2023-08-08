using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace HQSOFT.SharedInformation.Companies
{
    public class CompanyManager : DomainService
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyManager(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<Company> CreateAsync(
        string abbreviation, string companyName, Guid defaultCurrency, string taxID, Guid countryId, bool isGroup, Guid parentCompany, string address1, string address2, string email, string web, string phone1, string phone2, Guid stateId, Guid provinceId)
        {
            Check.NotNullOrWhiteSpace(abbreviation, nameof(abbreviation));
            Check.NotNullOrWhiteSpace(companyName, nameof(companyName));
            Check.NotNullOrWhiteSpace(taxID, nameof(taxID));

            var company = new Company(
             GuidGenerator.Create(),
             abbreviation, companyName, defaultCurrency, taxID, countryId, isGroup, parentCompany, address1, address2, email, web, phone1, phone2, stateId, provinceId
             );

            return await _companyRepository.InsertAsync(company);
        }

        public async Task<Company> UpdateAsync(
            Guid id,
            string abbreviation, string companyName, Guid defaultCurrency, string taxID, Guid countryId, bool isGroup, Guid parentCompany, string address1, string address2, string email, string web, string phone1, string phone2, Guid stateId, Guid provinceId, [CanBeNull] string concurrencyStamp = null
        )
        {
            Check.NotNullOrWhiteSpace(abbreviation, nameof(abbreviation));
            Check.NotNullOrWhiteSpace(companyName, nameof(companyName));
            Check.NotNullOrWhiteSpace(taxID, nameof(taxID));

            var company = await _companyRepository.GetAsync(id);

            company.Abbreviation = abbreviation;
            company.CompanyName = companyName;
            company.DefaultCurrency = defaultCurrency;
            company.TaxID = taxID;
            company.CountryId = countryId;
            company.IsGroup = isGroup;
            company.ParentCompany = parentCompany;
            company.Address1 = address1;
            company.Address2 = address2;
            company.Email = email;
            company.Web = web;
            company.Phone1 = phone1;
            company.Phone2 = phone2;
            company.StateId = stateId;
            company.ProvinceId = provinceId;

            company.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _companyRepository.UpdateAsync(company);
        }

    }
}