using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace HQSOFT.SharedInformation.Districts
{
    public class DistrictDto : AuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public Guid ProvinceId { get; set; }
        public int Idx { get; set; }
        [Required]
        public string DistrictName { get; set; }

        public string ConcurrencyStamp { get; set; }
        public bool IsChanged { get; set; }
    }
}