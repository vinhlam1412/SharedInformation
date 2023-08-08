using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace HQSOFT.SharedInformation.Countries
{
    public class Country : AuditedAggregateRoot<Guid>
    {
        [NotNull]
        public virtual string Code { get; set; }

        [NotNull]
        public virtual string Description { get; set; }

        [CanBeNull]
        public virtual string? DateFormat { get; set; }

        [CanBeNull]
        public virtual string? TimeFormat { get; set; }

        [CanBeNull]
        public virtual string? TimeZone { get; set; }

        public virtual int Idx { get; set; }

        public Country()
        {

        }

        public Country(Guid id, string code, string description, string dateFormat, string timeFormat, string timeZone, int idx)
        {

            Id = id;
            Check.NotNull(code, nameof(code));
            Check.NotNull(description, nameof(description));
            Code = code;
            Description = description;
            DateFormat = dateFormat;
            TimeFormat = timeFormat;
            TimeZone = timeZone;
            Idx = idx;
        }

    }
}