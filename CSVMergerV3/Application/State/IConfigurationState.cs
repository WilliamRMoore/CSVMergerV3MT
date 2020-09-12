using System;
using System.Collections.Generic;
using System.Text;

namespace CSVMergerV3.Application.State
{
    public interface IConfigurationState
    {
        void setOutputSetName(string name);
        void setOutputSetColumns(string[] columns);
    }
}
