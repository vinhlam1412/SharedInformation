using Volo.Abp.Application.Dtos;
using System;

namespace HQSOFT.SharedInformation.Countries
{
    public class CountryExcelDownloadDto
    {
        public string DownloadToken { get; set; }

        public string? FilterText { get; set; }

        public string? Code { get; set; }
        public string? Description { get; set; }
        public string? DateFormat { get; set; }
        public string? TimeFormat { get; set; }
        public string? TimeZone { get; set; }
        public int? IdxMin { get; set; }
        public int? IdxMax { get; set; }

        public CountryExcelDownloadDto()
        {

        }
    }
}