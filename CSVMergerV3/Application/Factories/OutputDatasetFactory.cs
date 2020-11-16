using CSVMergerV3.Application.Domain;
using CSVMergerV3.Application.Services.HelperServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSVMergerV3.Application.Factories
{
    public class OutputDatasetFactory : IOutputDatasetFactory
    {
        private readonly IFileStreamProvider _fileStreamProvider;

        public OutputDatasetFactory(IFileStreamProvider fileStreamProvider)
        {
            _fileStreamProvider = fileStreamProvider;
        }

        public OutputDataSet MakeOutputDataset()
        {
            return new OutputDataSet(_fileStreamProvider);
        }

    }
}
