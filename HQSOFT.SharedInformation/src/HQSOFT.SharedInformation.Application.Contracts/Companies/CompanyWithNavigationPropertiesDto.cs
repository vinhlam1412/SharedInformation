using HQSOFT.SharedInformation.Countries;
using System;
using System.Collections.Generic;
using System.Text;

namespace HQSOFT.SharedInformation.Companies
{
    public class CompanyWithNavigationPropertiesDto
    {
        public CountryDto  Country { get; set; }
        public CompanyDto Company { get; set; }
    }
}
