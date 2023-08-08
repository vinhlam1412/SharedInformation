using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace HQSOFT.SharedInformation.Wards
{
    public class WardUpdateDto : IHasConcurrencyStamp
    {
        public Guid DistrictId { get; set; }
        public int Idx { get; set; }
        [Required]
        public string WardName { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}