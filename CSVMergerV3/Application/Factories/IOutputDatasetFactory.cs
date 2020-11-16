using CSVMergerV3.Application.Domain;

namespace CSVMergerV3.Application.Factories
{
    public interface IOutputDatasetFactory
    {
        OutputDataSet MakeOutputDataset();
    }
}