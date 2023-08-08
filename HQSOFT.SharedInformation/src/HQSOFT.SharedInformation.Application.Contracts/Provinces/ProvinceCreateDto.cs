using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace HQSOFT.SharedInformation.Provinces
{
    public class ProvinceCreateDto
    {
        public int Idx { get; set; }
        public Guid CountryId { get; set; }
        [Required]
        public string ProvinceCode { get; set; }
        [Required]
        public string ProvinceName { get; set; }
    }
}