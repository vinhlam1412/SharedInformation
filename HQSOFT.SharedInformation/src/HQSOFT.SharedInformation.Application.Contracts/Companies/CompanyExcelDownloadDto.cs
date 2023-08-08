using Volo.Abp.Application.Dtos;
using System;

namespace HQSOFT.SharedInformation.Companies
{
    public class CompanyExcelDownloadDto
    {
        public string DownloadToken { get; set; }

        public string? FilterText { get; set; }

        public string? Abbreviation { get; set; }
        public string? CompanyName { get; set; }
        public Guid? DefaultCurrency { get; set; }
        public string? TaxID { get; set; }
        public Guid? CountryId { get; set; }
        public bool? IsGroup { get; set; }
        public Guid? ParentCompany { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? Email { get; set; }
        public string? Web { get; set; }
        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }
        public Guid? StateId { get; set; }
        public Guid? ProvinceId { get; set; }

        public CompanyExcelDownloadDto()
        {

        }
    }
}