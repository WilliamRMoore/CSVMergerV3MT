using CSVMergerV3.Application.Domain;

namespace CSVMergerV3.Application.Services.HelperServices
{
    public interface IRuleChecker
    {
        bool DoesRuleExist(Dataset dataSet, int targetIndex);
    }
}