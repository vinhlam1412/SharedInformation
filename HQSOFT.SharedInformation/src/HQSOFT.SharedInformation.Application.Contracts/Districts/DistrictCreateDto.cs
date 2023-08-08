using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace HQSOFT.SharedInformation.Districts
{
    public class DistrictCreateDto
    {
        public Guid ProvinceId { get; set; }
        public int Idx { get; set; }
        [Required]
        public string DistrictName { get; set; }
    }
}