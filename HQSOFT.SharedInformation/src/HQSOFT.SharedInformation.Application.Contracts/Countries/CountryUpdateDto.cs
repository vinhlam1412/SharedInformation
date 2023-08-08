using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace HQSOFT.SharedInformation.Countries
{
    public class CountryUpdateDto : IHasConcurrencyStamp
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Description { get; set; }
        public string? DateFormat { get; set; }
        public string? TimeFormat { get; set; }
        public string? TimeZone { get; set; }
        public int Idx { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}