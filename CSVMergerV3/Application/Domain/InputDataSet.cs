using CSVMergerV3.Application.Services.HelperServices;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace CSVMergerV3.Application.Domain
{
    public class InputDataSet
    {
        public string DatasetName { get; set; }
        public string FilePath { get; set; }
        public string[] Columns { get; set; }
        public BlockingCollection<string> UnProcessedLines = new BlockingCollection<string>(boundedCapacity: 100000);
        public BlockingCollection<string> ProcessedLines = new BlockingCollection<string>(boundedCapacity: 200000);
        public List<MapRule> MapRules = new List<MapRule>();
        private static Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
        public int OutputColumnCount = 0;

        public InputDataSet()
        {

        }
    }
}

