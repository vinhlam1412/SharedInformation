using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HQSOFT.SharedInformation.Blazor.Pages.SharedInformation.Company
{
    public class DefaultCurrency
    {
        public Guid Id { get; set; }
        public string Name { get; set; }


        public DefaultCurrency(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
