using CSVMergerV3.Application.Domain;
using CSVMergerV3.Application.Factories;
using CSVMergerV3.Application.Services.HelperServices;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSVMergerV3.Application.State
{
    public class JobState : IJobState
    {
        private long TotalLinesToPreccess = 0;
        private long ProccessedLines = 0;
        private Dataset OutputSet;
        private List<InputDataSet> InputSets = new List<InputDataSet>();
        private readonly IFileStreamProvider _fileStreamProvider;
        private readonly IOutputDatasetFactory _outputDatasetFactory;

        public JobState(IFileStreamProvider fileStreamProvider, IOutputDatasetFactory outputDatasetFactory)
        {
            _fileStreamProvider = fileStreamProvider;
            _outputDatasetFactory = outputDatasetFactory;
            //_configurationState = configurationState;

            OutputSet = _outputDatasetFactory.MakeOutputDataset();
        }

        public void SetOutputSetName(string name)
        {
            OutputSet.DatasetName = name + ".csv";
        }

        public void SetOutputSetColumns(string[] columns)
        {
            OutputSet.Columns = columns;
        }

        public void SetOutputPath(string path)
        {
            OutputSet.FilePath = path + "\\" + OutputSet.DatasetName;
        }

        public void AddInputset(InputDataSet dataset)
        {
            //get the columns from inputset
            var fileStream = _fileStreamProvider.GetReadStream(dataset.FilePath);
            using (fileStream)
            {
                dataset.Columns = fileStream.ReadLine().Split(",");//get our column names
                dataset.DatasetName = Path.GetFileName(dataset.FilePath);
                InputSets.Add(dataset);
            }
        }

        public List<InputDataSet> GetinputSets()
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
        public string GetOutputDirectory()
        {
            return OutputSet.FilePath;
        }

        public void SetTotalLines(long lineCount)
        {
            TotalLinesToPreccess = lineCount;
        }

        public long GetTotalLines()
        {
            return TotalLinesToPreccess;
        }

        public void SetTotalLines()
        {
            throw new NotImplementedException();
        }

        public void ExecuteJob()
        {
            throw new NotImplementedException();
        }
    }
}
