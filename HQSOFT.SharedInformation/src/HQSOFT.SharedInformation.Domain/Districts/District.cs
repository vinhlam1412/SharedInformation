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

namespace HQSOFT.SharedInformation.Districts
{
    public class District : AuditedEntity<Guid>, IHasConcurrencyStamp
    {
        public virtual Guid ProvinceId { get; set; }

        public virtual int Idx { get; set; }

        [NotNull]
        public virtual string DistrictName { get; set; }

        public string ConcurrencyStamp { get; set; }

        public District()
        {

        }

        public District(Guid id, Guid provinceId, int idx, string districtName)
        {
            ConcurrencyStamp = Guid.NewGuid().ToString("N");
            Id = id;
            Check.NotNull(districtName, nameof(districtName));
            ProvinceId = provinceId;
            Idx = idx;
            DistrictName = districtName;
        }

    }
}