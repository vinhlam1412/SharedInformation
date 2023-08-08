using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace HQSOFT.SharedInformation.Countries
{
    public class CountryCreateDto
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Description { get; set; }
        public string? DateFormat { get; set; }
        public string? TimeFormat { get; set; }
        public string? TimeZone { get; set; }
        public int Idx { get; set; }
    }
}