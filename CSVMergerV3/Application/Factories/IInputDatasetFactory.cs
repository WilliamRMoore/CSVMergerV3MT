using CSVMergerV3.Application.Domain;

namespace CSVMergerV3.Application.Factories
{
    public interface IInputDatasetFactory
    {
        InputDataSet MakeInputDataset();
    }
}