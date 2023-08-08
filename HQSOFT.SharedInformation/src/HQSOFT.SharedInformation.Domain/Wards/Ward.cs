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

namespace HQSOFT.SharedInformation.Wards
{
    public class Ward : AuditedEntity<Guid>, IHasConcurrencyStamp
    {
        public virtual Guid DistrictId { get; set; }

        public virtual int Idx { get; set; }

        [NotNull]
        public virtual string WardName { get; set; }

        public string ConcurrencyStamp { get; set; }

        public Ward()
        {

        }

        public Ward(Guid id, Guid districtId, int idx, string wardName)
        {
            ConcurrencyStamp = Guid.NewGuid().ToString("N");
            Id = id;
            Check.NotNull(wardName, nameof(wardName));
            DistrictId = districtId;
            Idx = idx;
            WardName = wardName;
        }

    }
}