using System;
using System.Collections.Generic;
using System.Text;

namespace CSVMergerV3.Application.Services.Validation
{
    public interface IDataSetNameValidator
    {
        bool ValidateNewDatasetName(string newDataSetName);
    }
}
