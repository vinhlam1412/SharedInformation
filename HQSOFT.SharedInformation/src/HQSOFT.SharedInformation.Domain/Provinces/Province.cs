using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;

using Volo.Abp;

namespace HQSOFT.SharedInformation.Provinces
{
    public class Province : AuditedEntity<Guid>, IHasConcurrencyStamp
    {
        public virtual int Idx { get; set; }

        public virtual Guid CountryId { get; set; }

        [NotNull]
        public virtual string ProvinceCode { get; set; }

        [NotNull]
        public virtual string ProvinceName { get; set; }

        public string ConcurrencyStamp { get; set; }

        public Province()
        {

        }

        public Province(Guid id, int idx, Guid countryId, string provinceCode, string provinceName)
        {
            ConcurrencyStamp = Guid.NewGuid().ToString("N");
            Id = id;
            Check.NotNull(provinceCode, nameof(provinceCode));
            Check.NotNull(provinceName, nameof(provinceName));
            Idx = idx;
            CountryId = countryId;
            ProvinceCode = provinceCode;
            ProvinceName = provinceName;
        }

    }
}