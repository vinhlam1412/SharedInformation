using Volo.Abp.Application.Dtos;
using System;

namespace HQSOFT.SharedInformation.Provinces
{
    public class ProvinceExcelDownloadDto
    {
        public string DownloadToken { get; set; }

        public string? FilterText { get; set; }

        public int? IdxMin { get; set; }
        public int? IdxMax { get; set; }
        public Guid? CountryId { get; set; }
        public string? ProvinceCode { get; set; }
        public string? ProvinceName { get; set; }

        public ProvinceExcelDownloadDto()
        {

        }
    }
}