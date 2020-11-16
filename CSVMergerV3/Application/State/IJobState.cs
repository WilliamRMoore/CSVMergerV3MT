using CSVMergerV3.Application.Domain;
using System.Collections.Generic;

namespace CSVMergerV3.Application.State
{
    public interface IJobState
    {
        void AddInputset(InputDataSet dataset);
        List<InputDataSet> GetinputSets();
        string[] GetOutputColumns();
        string GetOutputDirectory();
        string GetOutputsetName();
        long GetTotalLines();
        void SetOutputPath(string path);
        void SetOutputSetColumns(string[] columns);
        void SetOutputSetName(string name);
        void SetTotalLines();
        void ExecuteJob();
    }
}