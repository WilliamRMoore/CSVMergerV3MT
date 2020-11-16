using CSVMergerV3.Application.Services.HelperServices;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace CSVMergerV3.Application.Domain
{
    public abstract class Dataset
    {
        public string DatasetName { get; set; }
        public string FilePath { get; set; }
        public string[] Columns { get; set; }      
    }
}
