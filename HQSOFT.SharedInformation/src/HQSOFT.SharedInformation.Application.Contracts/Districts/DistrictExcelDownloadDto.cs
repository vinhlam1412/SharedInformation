using Volo.Abp.Application.Dtos;
using System;

namespace HQSOFT.SharedInformation.Districts
{
    public class DistrictExcelDownloadDto
    {
        public string DownloadToken { get; set; }

        public string? FilterText { get; set; }

        public Guid? ProvinceId { get; set; }
        public int? IdxMin { get; set; }
        public int? IdxMax { get; set; }
        public string? DistrictName { get; set; }

        public DistrictExcelDownloadDto()
        {

        }
    }
}