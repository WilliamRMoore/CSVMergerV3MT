using CSVMergerV3.Application.Domain;
using CSVMergerV3.Application.Services.HelperServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace CSVMergerV3.Application.State
{
    public class ConfiguationState : IConfigurationState
    {
        //private Dataset OutputSet = new Dataset();
        //private List<Dataset> InputSets = new List<Dataset>();
       // private readonly IFileStreamProvider _fileStreamProvider;
        private int ProccessorCount = Environment.ProcessorCount;
        private int AppThreadCount = 3;
        private static Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

        public ConfiguationState()
        {
            //_fileStreamProvider = fileStreamProvider;
        }

        //public void setOutputSetName(string name)
        //{
        //    OutputSet.DatasetName = name + ".csv";
        //}

        //public void setOutputSetColumns(string[] columns)
        //{
        //    OutputSet.Columns = columns;
        //}

        //public void setOutputPath(string path)
        //{
        //    OutputSet.FilePath = path + "\\" + OutputSet.DatasetName;
        //}

        //public void AddInputset(Dataset dataset)
        //{
        //    //get the columns from inputset
        //    var fileStream = _fileStreamProvider.GetReadStream(dataset.FilePath);
        //    using (fileStream)
        //    {
        //        dataset.Columns = fileStream.ReadLine().Split(",");//get our column names
        //        dataset.DatasetName = Path.GetFileName(dataset.FilePath);
        //        InputSets.Add(dataset);
        //    }
        //}
        
        //public List<Dataset> GetinputSets()
        //{
        //    return InputSets;
        //}
        //public string[] GetOutputColumns()
        //{
        //    return OutputSet.Columns;
        //}

        //public string GetOutputsetName()
        //{
        //    return OutputSet.DatasetName;
        //}
        //public string GetOutputDirectory()
        //{
        //    return OutputSet.FilePath;
        //}
        public int GetCPUCount()
        {
            return ProccessorCount;
        }
        public void SetAppThreadCount(int threads)
        {
            AppThreadCount = threads;
        }
        public int GetAppThreadCount()
        {
            return AppThreadCount;
        }

        public Regex GetRegex()
        {
            return CSVParser;
        }
    }
}
