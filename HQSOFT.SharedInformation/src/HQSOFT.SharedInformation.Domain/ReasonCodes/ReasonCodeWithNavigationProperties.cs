using HQSOFT.eBiz.GeneralLedger.Accounts;
using System;
using System.Collections.Generic;
using System.Text;

namespace HQSOFT.SharedInformation.ReasonCodes
{
    public class ReasonCodeWithNavigationProperties
    {
        public ReasonCode ReasonCode { get; set; }

        public Account Account { get; set; }
    }
}
