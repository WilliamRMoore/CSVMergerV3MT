using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSVMergerV3.Application.Services.Validation
{
    public class FileValidator : IFileValidator
    {
        public bool ValidateFilePath(string path)
        {
            return File.Exists(path);
        }
    }
}
