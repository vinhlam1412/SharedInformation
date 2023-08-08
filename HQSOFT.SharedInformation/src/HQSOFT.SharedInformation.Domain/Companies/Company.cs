using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace HQSOFT.SharedInformation.Companies
{
    public class Company : AuditedAggregateRoot<Guid>
    {
        [NotNull]
        public virtual string Abbreviation { get; set; }

        [NotNull]
        public virtual string CompanyName { get; set; }

        public virtual Guid DefaultCurrency { get; set; }

        [NotNull]
        public virtual string TaxID { get; set; }

        public virtual Guid CountryId { get; set; }

        public virtual bool IsGroup { get; set; }

        public virtual Guid ParentCompany { get; set; }

        [CanBeNull]
        public virtual string? Address1 { get; set; }

        [CanBeNull]
        public virtual string? Address2 { get; set; }

        [CanBeNull]
        public virtual string? Email { get; set; }

        [CanBeNull]
        public virtual string? Web { get; set; }

        [CanBeNull]
        public virtual string? Phone1 { get; set; }

        [CanBeNull]
        public virtual string? Phone2 { get; set; }

        public virtual Guid StateId { get; set; }

        public virtual Guid ProvinceId { get; set; }

        public Company()
        {

        }

        public Company(Guid id, string abbreviation, string companyName, Guid defaultCurrency, string taxID, Guid countryId, bool isGroup, Guid parentCompany, string address1, string address2, string email, string web, string phone1, string phone2, Guid stateId, Guid provinceId)
        {

            Id = id;
            Check.NotNull(abbreviation, nameof(abbreviation));
            Check.NotNull(companyName, nameof(companyName));
            Check.NotNull(taxID, nameof(taxID));
            Abbreviation = abbreviation;
            CompanyName = companyName;
            DefaultCurrency = defaultCurrency;
            TaxID = taxID;
            CountryId = countryId;
            IsGroup = isGroup;
            ParentCompany = parentCompany;
            Address1 = address1;
            Address2 = address2;
            Email = email;
            Web = web;
            Phone1 = phone1;
            Phone2 = phone2;
            StateId = stateId;
            ProvinceId = provinceId;
        }

    }
}