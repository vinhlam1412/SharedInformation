using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace HQSOFT.SharedInformation.States
{
    public class StateCreateDto
    {
        public Guid CountryId { get; set; }
        public int Idx { get; set; }
        [Required]
        public string StateCode { get; set; }
        [Required]
        public string StateName { get; set; }
    }
}