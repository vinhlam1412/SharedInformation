using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace HQSOFT.SharedInformation.ReasonCodes
{
    public class ReasonCodeUpdateDto : IHasConcurrencyStamp
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Description { get; set; }
        public Guid AccountId { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}