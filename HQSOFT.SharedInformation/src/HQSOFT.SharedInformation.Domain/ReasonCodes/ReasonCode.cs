using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace HQSOFT.SharedInformation.ReasonCodes
{
    public class ReasonCode : AuditedAggregateRoot<Guid>
    {
        [NotNull]
        public virtual string Code { get; set; }

        [NotNull]
        public virtual string Type { get; set; }

        [NotNull]
        public virtual string Description { get; set; }

        public virtual Guid AccountId { get; set; }

        public ReasonCode()
        {

        }

        public ReasonCode(Guid id, string code, string type, string description, Guid accountId)
        {

            Id = id;
            Check.NotNull(code, nameof(code));
            Check.NotNull(type, nameof(type));
            Check.NotNull(description, nameof(description));
            Code = code;
            Type = type;
            Description = description;
            AccountId = accountId;
        }

    }
}