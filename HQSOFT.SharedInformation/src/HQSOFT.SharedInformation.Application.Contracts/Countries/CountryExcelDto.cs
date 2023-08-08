using System;

namespace HQSOFT.SharedInformation.Countries
{
    public class CountryExcelDto
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string? DateFormat { get; set; }
        public string? TimeFormat { get; set; }
        public string? TimeZone { get; set; }
        public int Idx { get; set; }
    }
}