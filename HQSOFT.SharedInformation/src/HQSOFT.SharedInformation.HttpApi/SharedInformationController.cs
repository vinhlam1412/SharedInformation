using HQSOFT.SharedInformation.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace HQSOFT.SharedInformation;

public abstract class SharedInformationController : AbpControllerBase
{
    protected SharedInformationController()
    {
        LocalizationResource = typeof(SharedInformationResource);
    }
}
