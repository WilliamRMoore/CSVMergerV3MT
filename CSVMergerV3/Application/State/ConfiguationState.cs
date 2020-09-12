using CSVMergerV3.Application.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSVMergerV3.Application.State
{
    public class ConfiguationState : IConfigurationState
    {
        private Dataset OutputSet = new Dataset();

        public void setOutputSetName(string name)
        {
            OutputSet.DatasetName = name;
        }

        public void setOutputSetColumns(string[] columns)
        {
            OutputSet.Columns = columns;
        }
        
    }
}
