using System;
using System.Collections.Generic;
using System.Text;

namespace HQSOFT.SharedInformation.ReasonCodes
{
    public enum ReasonCodeType
    {
        Issue = 1,
        Receipt = 2,
        Adjustment = 3,
        Transfer = 4,
        VendorReturn = 5,
        AssemblyDisassembly = 6,
        SFADontbuy = 7,
        SFACheckin = 8

    }
    public class ReasonCodeTypeList
    {
        public string Value { get; set; }
        public string DisplayName { get; set; }
    }
}
