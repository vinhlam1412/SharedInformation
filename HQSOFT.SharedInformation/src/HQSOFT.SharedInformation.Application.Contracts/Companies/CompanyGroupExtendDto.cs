using System;
using System.Collections.Generic;
using System.Text;

namespace HQSOFT.SharedInformation.Companies
{
    public class CompanyGroupExtendDto : CompanyDto
    {
        public CompanyDto TopGroup { get; set; }   
        public List<CompanyDto> SubGroups { get; set; }   
    }
}
