using HQSOFT.SharedInformation.Localization;
using Volo.Abp.AspNetCore.Components;

namespace HQSOFT.SharedInformation.Blazor;

public abstract class SharedInformationComponentBase : AbpComponentBase
{
    protected SharedInformationComponentBase()
    {
        LocalizationResource = typeof(SharedInformationResource);
    }
}
