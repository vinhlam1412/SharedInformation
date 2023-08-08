using System;

namespace HQSOFT.SharedInformation.Provinces
{
    public class ProvinceExcelDto
    {
        public int Idx { get; set; }
        public Guid CountryId { get; set; }
        public string ProvinceCode { get; set; }
        public string ProvinceName { get; set; }
    }
}