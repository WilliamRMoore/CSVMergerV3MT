namespace CSVMergerV3.Application.Services.Validation
{
    public interface IDirectoryValidator
    {
        bool DirectoryExists(string path);
    }
}