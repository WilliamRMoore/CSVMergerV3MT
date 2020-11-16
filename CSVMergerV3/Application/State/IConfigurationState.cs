using CSVMergerV3.Application.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CSVMergerV3.Application.State
{
    public interface IConfigurationState
    {
        //void setOutputSetName(string name);
        //void setOutputSetColumns(string[] columns);
        //void setOutputPath(string path);
        //void AddInputset(Dataset dataset);
        //public List<Dataset> GetinputSets();
        //public string[] GetOutputColumns();
        //public string GetOutputsetName();
        //string GetOutputDirectory();
        int GetCPUCount();
        void SetAppThreadCount(int threads);
        int GetAppThreadCount();
        Regex GetRegex();
    }
}
