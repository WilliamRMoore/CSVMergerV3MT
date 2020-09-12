using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CSVMergerV3.Application.Services.Validation
{
    public class DataSetNameValidator : IDataSetNameValidator
    {
        public bool ValidateNewDatasetName(string newDataSetName)
        {
            Regex illegaleCharsRegex = new Regex(@"^[a-zA-Z0-9_]+$");
            if (!illegaleCharsRegex.IsMatch(newDataSetName))
            {
                return false;
            }

            return true;
        }
    }
}
