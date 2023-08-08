using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace HQSOFT.SharedInformation.ReasonCodes
{
    public class ReasonCodeCreateDto
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Description { get; set; }
        public Guid AccountId { get; set; }
    }
}