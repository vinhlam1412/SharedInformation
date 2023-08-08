using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace HQSOFT.SharedInformation.Companies
{
    public interface ICompanyRepository : IRepository<Company, Guid>
    {
    
        Task<List<Company>> GetListAsync(
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
            CancellationToken cancellationToken = default
        );

        Task<long> GetCountAsync(
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
            CancellationToken cancellationToken = default);
    }
}