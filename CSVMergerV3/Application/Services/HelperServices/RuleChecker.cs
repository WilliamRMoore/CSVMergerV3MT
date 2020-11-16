using CSVMergerV3.Application.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CSVMergerV3.Application.Services.HelperServices
{
    public class RuleChecker : IRuleChecker
    {
        public bool DoesRuleExist(InputDataSet dataSet, int targetIndex)
        {
            return dataSet.MapRules.Any(r => r.TargetIndex == targetIndex);
        }
    }
}
