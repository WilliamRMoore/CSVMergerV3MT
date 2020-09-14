using System;
using System.Collections.Generic;
using System.Text;

namespace CSVMergerV3.Application.Domain
{
    public class Dataset
    {
        public string DatasetName { get; set; }
        public string FilePath { get; set; }
        public string[] Columns { get; set; }

        public List<MapRule> MapRules = new List<MapRule>();
    }
}
