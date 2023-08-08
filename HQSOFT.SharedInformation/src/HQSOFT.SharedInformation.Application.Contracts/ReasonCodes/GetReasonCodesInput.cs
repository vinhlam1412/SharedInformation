using Volo.Abp.Application.Dtos;
using System;

namespace HQSOFT.SharedInformation.ReasonCodes
{
    public class GetReasonCodesInput : PagedAndSortedResultRequestDto
    {
        public string? FilterText { get; set; }

        public string? Code { get; set; }
        public string? Type { get; set; }
        public string? Description { get; set; }
        public Guid? AccountId { get; set; }

        public GetReasonCodesInput()
        {

        }
    }
}