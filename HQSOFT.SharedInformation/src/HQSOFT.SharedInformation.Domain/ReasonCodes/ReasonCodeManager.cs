using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace HQSOFT.SharedInformation.ReasonCodes
{
    public class ReasonCodeManager : DomainService
    {
        private readonly IReasonCodeRepository _reasonCodeRepository;

        public ReasonCodeManager(IReasonCodeRepository reasonCodeRepository)
        {
            _reasonCodeRepository = reasonCodeRepository;
        }

        public async Task<ReasonCode> CreateAsync(
        string code, string type, string description, Guid accountId)
        {
            Check.NotNullOrWhiteSpace(code, nameof(code));
            Check.NotNullOrWhiteSpace(type, nameof(type));
            Check.NotNullOrWhiteSpace(description, nameof(description));

            var reasonCode = new ReasonCode(
             GuidGenerator.Create(),
             code, type, description, accountId
             );

            return await _reasonCodeRepository.InsertAsync(reasonCode);
        }

        public async Task<ReasonCode> UpdateAsync(
            Guid id,
            string code, string type, string description, Guid accountId, [CanBeNull] string concurrencyStamp = null
        )
        {
            Check.NotNullOrWhiteSpace(code, nameof(code));
            Check.NotNullOrWhiteSpace(type, nameof(type));
            Check.NotNullOrWhiteSpace(description, nameof(description));

            var reasonCode = await _reasonCodeRepository.GetAsync(id);

            reasonCode.Code = code;
            reasonCode.Type = type;
            reasonCode.Description = description;
            reasonCode.AccountId = accountId;

            reasonCode.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _reasonCodeRepository.UpdateAsync(reasonCode);
        }

    }
}