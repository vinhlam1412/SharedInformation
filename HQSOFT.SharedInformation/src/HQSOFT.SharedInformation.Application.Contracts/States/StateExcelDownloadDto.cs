using Volo.Abp.Application.Dtos;
using System;

namespace HQSOFT.SharedInformation.States
{
    public class StateExcelDownloadDto
    {
        public string DownloadToken { get; set; }

        public string? FilterText { get; set; }

        public Guid? CountryId { get; set; }
        public int? IdxMin { get; set; }
        public int? IdxMax { get; set; }
        public string? StateCode { get; set; }
        public string? StateName { get; set; }

        public StateExcelDownloadDto()
        {

        }
    }
}