using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace HQSOFT.SharedInformation.Companies
{
    public class CompanyDto : AuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string Abbreviation { get; set; }
        public string CompanyName { get; set; }
        public Guid DefaultCurrency { get; set; }
        public string TaxID { get; set; }
        public Guid CountryId { get; set; }
        public bool IsGroup { get; set; }
        public Guid ParentCompany { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? Email { get; set; }
        public string? Web { get; set; }
        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }
        public Guid StateId { get; set; }
        public Guid ProvinceId { get; set; }
        public string ConcurrencyStamp { get; set; }
        public bool IsChanged { get; set; }
    }
}