using HQSOFT.eBiz.GeneralLedger.Accounts;
using System;
using System.Collections.Generic;
using System.Text;

namespace HQSOFT.SharedInformation.ReasonCodes
{
    public class ReasonCodeWithNavigationPropertiesDto
    {
        public ReasonCodeDto ReasonCode { get; set; }

        public AccountDto Account { get; set; }
    }
}
