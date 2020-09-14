using CSVMergerV3.Application.Domain;
using CSVMergerV3.Application.Services.HelperServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSVMergerV3.Application.State
{
    public class ConfiguationState : IConfigurationState
    {
        private Dataset OutputSet = new Dataset();
        private List<Dataset> InputSets = new List<Dataset>();
        private readonly IFileStreamProvider _fileStreamProvider;

        public ConfiguationState(IFileStreamProvider fileStreamProvider)
        {
            _fileStreamProvider = fileStreamProvider;
        }

        public void setOutputSetName(string name)
        {
            OutputSet.DatasetName = name;
        }

        public void setOutputSetColumns(string[] columns)
        {
            OutputSet.Columns = columns;
        }

        public void setOutputPath(string path)
        {
            OutputSet.FilePath = path;
        }

        public void AddInputset(Dataset dataset)
        {
            //get the columns from inputset
            var fileStream = _fileStreamProvider.GetReadStream(dataset.FilePath);
            dataset.Columns = fileStream.ReadLine().Split(",");//get our column names
            dataset.DatasetName = Path.GetFileName(dataset.FilePath);
            InputSets.Add(dataset);
        }
        
        public List<Dataset> GetinputSets()
        {
            return InputSets;
        }
        public string[] GetOutputColumns()
        {
            return OutputSet.Columns;
        }

        public string GetOutputsetName()
        {
            return OutputSet.DatasetName;
        }
    }
}
