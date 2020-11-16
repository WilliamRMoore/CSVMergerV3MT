using CSVMergerV3.Application.Domain;

namespace CSVMergerV3.Application.FileProcessor
{
    public interface IFileProcessor
    {
        void processConfig(string[] outputColumns, int outputColumnCount, InputDataSet dataSet, string outputPath);
    }
}