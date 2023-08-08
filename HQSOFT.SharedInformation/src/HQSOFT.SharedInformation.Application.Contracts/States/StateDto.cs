using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace HQSOFT.SharedInformation.States
{
    public class StateDto : AuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public Guid CountryId { get; set; }
        public int Idx { get; set; }
        [Required]
        public string StateCode { get; set; }
        [Required]
        public string StateName { get; set; }

        public string ConcurrencyStamp { get; set; }
        public bool IsChanged { get; set; }
    }
}