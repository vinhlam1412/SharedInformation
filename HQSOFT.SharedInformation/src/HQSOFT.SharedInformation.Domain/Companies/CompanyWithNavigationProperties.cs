using HQSOFT.SharedInformation.Countries;
using System;
using System.Collections.Generic;
using System.Text;

namespace HQSOFT.SharedInformation.Companies
{
    public class CompanyWithNavigationProperties
    {
        public Country Country { get; set; }
        public Company Company { get; set; }
    }
}
