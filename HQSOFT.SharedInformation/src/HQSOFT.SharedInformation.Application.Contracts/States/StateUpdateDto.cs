using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace HQSOFT.SharedInformation.States
{
    public class StateUpdateDto : IHasConcurrencyStamp
    {
        public Guid CountryId { get; set; }
        public int Idx { get; set; }
        [Required]
        public string StateCode { get; set; }
        [Required]
        public string StateName { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}