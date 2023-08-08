using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace HQSOFT.SharedInformation.Wards
{
    public class WardCreateDto
    {
        public Guid DistrictId { get; set; }
        public int Idx { get; set; }
        [Required]
        public string WardName { get; set; }
    }
}