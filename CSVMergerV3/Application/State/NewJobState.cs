using CSVMergerV3.Application.Domain;
using CSVMergerV3.Application.Factories;
using CSVMergerV3.Application.FileProcessor;
using CSVMergerV3.Application.Services.HelperServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSVMergerV3.Application.State
{
    class NewJobState : IJobState
    {
        private long TotalLinesToPreccess = 0;
        private long ProccessedLines = 0;
        private string OutputPath;
        private string OutputSetName;
        private string[] OutputColumns;
        private int OutputColumnCount;
        private List<InputDataSet> InputSets = new List<InputDataSet>();
        private readonly IOutputDatasetFactory _outputDatasetFactory;
        private readonly IFileStreamProvider _fileStreamProvider;
        private readonly IFileLineCounter _fileLineCounter;
        private readonly IFileProcessor _fileProcessor;

        public NewJobState(IOutputDatasetFactory outputDatasetFactory, IFileStreamProvider fileStreamProvider, IFileLineCounter fileLineCounter, IFileProcessor fileProcessor)
        {
            _outputDatasetFactory = outputDatasetFactory;
            _fileStreamProvider = fileStreamProvider;
            _fileLineCounter = fileLineCounter;
            _fileProcessor = fileProcessor;
        }

        public void SetOutputSetName(string fileName)
        {
            OutputSetName = fileName + ".csv";
        }

        public void SetOutputSetColumns(string[] columns)
        {
            OutputColumns = columns;
            OutputColumnCount = columns.Length;
        }

        public void SetOutputPath(string path)
        {
            OutputPath = path + "\\" + OutputSetName;
        }

        public void AddInputset(InputDataSet dataset)
        {
            var fileStream = _fileStreamProvider.GetReadStream(dataset.FilePath);//add the file stream to the data set so it can be accessed in the FileProcessor
            using (fileStream)
            {
                dataset.Columns = fileStream.ReadLine().Split(",");
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
            return OutputColumns;
        }

        public string GetOutputDirectory()
        {
            return OutputPath;
        }

        public string GetOutputsetName()
        {
            return OutputSetName;
        }

        public long GetTotalLines()
        {
            return TotalLinesToPreccess;
        }

        public void SetTotalLines()
        {
            TotalLinesToPreccess = _fileLineCounter.RetrieveLineCount(InputSets);
        }

        public void ExecuteJob()
        {
            var writeStream = _fileStreamProvider.GetWriteStream(OutputPath);
            using (writeStream)
            {
                writeStream.WriteLine(String.Join(",", OutputColumns));
            }

            foreach (var inputSet in InputSets)
            {
                _fileProcessor.processConfig(OutputColumns, OutputColumnCount, inputSet, OutputPath);
            }
        }
    }
}
