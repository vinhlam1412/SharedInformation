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

namespace HQSOFT.SharedInformation.States
{
    public class State : AuditedEntity<Guid>, IHasConcurrencyStamp
    {
        public virtual Guid CountryId { get; set; }

        public virtual int Idx { get; set; }

        [NotNull]
        public virtual string StateCode { get; set; }

        [NotNull]
        public virtual string StateName { get; set; }

        public string ConcurrencyStamp { get; set; }

        public State()
        {

        }

        public State(Guid id, Guid countryId, int idx, string stateCode, string stateName)
        {
            ConcurrencyStamp = Guid.NewGuid().ToString("N");
            Id = id;
            Check.NotNull(stateCode, nameof(stateCode));
            Check.NotNull(stateName, nameof(stateName));
            CountryId = countryId;
            Idx = idx;
            StateCode = stateCode;
            StateName = stateName;
        }

    }
}