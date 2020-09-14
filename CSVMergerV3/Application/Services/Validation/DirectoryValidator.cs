using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using System.Text;

namespace CSVMergerV3.Application.Services.Validation
{
    public class DirectoryValidator : IDirectoryValidator
    {
        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        //COME BACK TO THIS.

        //public bool HasWriteAccess(string path)
        //{
        //    var fileInfo = new FileInfo(path);

        //    FileSecurity fileSecurity = fileInfo.GetAccessControl();
        //}
    }
}
