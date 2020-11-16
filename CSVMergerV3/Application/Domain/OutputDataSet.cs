using CSVMergerV3.Application.Services.HelperServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSVMergerV3.Application.Domain
{
    public class OutputDataSet : Dataset
    {
        private readonly IFileStreamProvider _fileStreamProvider;

        public OutputDataSet(IFileStreamProvider fileStreamProvider)
        {
            _fileStreamProvider = fileStreamProvider;
        }
    }
}
