using System;

namespace HQSOFT.SharedInformation.ReasonCodes
{
    public class ReasonCodeExcelDto
    {
        public string Code { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public Guid AccountId { get; set; }
    }
}