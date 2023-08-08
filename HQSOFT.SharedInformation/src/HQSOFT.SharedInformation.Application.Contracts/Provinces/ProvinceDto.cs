using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace HQSOFT.SharedInformation.Provinces
{
    public class ProvinceDto : AuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public int Idx { get; set; }
        public Guid CountryId { get; set; }
        [Required]
        public string ProvinceCode { get; set; }
        [Required]
        public string ProvinceName { get; set; }
        public string ConcurrencyStamp { get; set; }
        public bool IsChanged { get; set; }
    }
}