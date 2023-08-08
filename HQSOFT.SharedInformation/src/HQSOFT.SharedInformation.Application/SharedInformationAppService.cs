using HQSOFT.SharedInformation.Localization;
using Volo.Abp.Application.Services;

namespace HQSOFT.SharedInformation;

public abstract class SharedInformationAppService : ApplicationService
{
    protected SharedInformationAppService()
    {
        LocalizationResource = typeof(SharedInformationResource);
        ObjectMapperContext = typeof(SharedInformationApplicationModule);
    }
}
