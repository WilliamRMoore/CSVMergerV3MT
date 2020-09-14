namespace CSVMergerV3.Application.Services.Validation
{
    public interface IFileValidator
    {
        bool ValidateFilePath(string path);
    }
}