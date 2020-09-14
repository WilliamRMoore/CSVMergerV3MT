using System.IO;

namespace CSVMergerV3.Application.Services.HelperServices
{
    public interface IFileStreamProvider
    {
        StreamReader GetReadStream(string filePath);
        StreamWriter GetWriteStream(string filePath);
        bool IsFilePathValid(string filePath);
    }
}