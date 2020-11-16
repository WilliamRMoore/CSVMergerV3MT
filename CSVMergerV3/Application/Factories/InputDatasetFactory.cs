using CSVMergerV3.Application.Domain;
using CSVMergerV3.Application.Services.HelperServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSVMergerV3.Application.Factories
{
    public class InputDatasetFactory : IInputDatasetFactory
    {
        private readonly IFileStreamProvider _fileStreamProvider;

        public InputDatasetFactory(IFileStreamProvider fileStreamProvider)
        {
            _fileStreamProvider = fileStreamProvider;
        }

        public InputDataSet MakeInputDataset()
        {
            return new InputDataSet(_fileStreamProvider);
        }
    }
}
