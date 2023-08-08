using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace HQSOFT.SharedInformation.Countries
{
    public class CountryDto : AuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string? DateFormat { get; set; }
        public string? TimeFormat { get; set; }
        public string? TimeZone { get; set; }
        public int Idx { get; set; }
        public string ConcurrencyStamp { get; set; }
        public bool IsChanged { get; set; }
    }
}