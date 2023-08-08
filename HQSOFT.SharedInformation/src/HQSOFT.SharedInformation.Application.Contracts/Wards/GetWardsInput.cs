using Volo.Abp.Application.Dtos;
using System;

namespace HQSOFT.SharedInformation.Wards
{
    public class GetWardsInput : PagedAndSortedResultRequestDto
    {
        public string? FilterText { get; set; }

        public Guid? DistrictId { get; set; }
        public int? IdxMin { get; set; }
        public int? IdxMax { get; set; }
        public string? WardName { get; set; }

        public GetWardsInput()
        {

        }
    }
}