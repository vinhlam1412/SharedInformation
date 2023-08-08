using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace HQSOFT.SharedInformation.Districts
{
    public class DistrictUpdateDto : IHasConcurrencyStamp
    {
        public Guid ProvinceId { get; set; }
        public int Idx { get; set; }
        [Required]
        public string DistrictName { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}