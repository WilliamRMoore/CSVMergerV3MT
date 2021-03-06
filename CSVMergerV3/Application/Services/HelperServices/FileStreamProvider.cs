﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSVMergerV3.Application.Services.HelperServices
{
    public class FileStreamProvider : IFileStreamProvider
    {
        public FileStreamProvider()
        {

        }
        public StreamReader GetReadStream(string filePath)
        {
            return new StreamReader(filePath);
        }

        public StreamWriter GetWriteStream(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new StreamWriter(filePath);
            }
            else
            {
                return File.AppendText(filePath);
            }
        }

        public bool IsFilePathValid(string filePath)
        {
            return File.Exists(filePath);
        }
    }
}

