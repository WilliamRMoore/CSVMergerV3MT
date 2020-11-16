using CSVMergerV3.Application.Domain;
using System.Collections.Generic;

namespace CSVMergerV3.Application.Services.HelperServices
{
    public interface IFileLineCounter
    {
        long RetrieveLineCount(List<InputDataSet> inputSets);
    }
}