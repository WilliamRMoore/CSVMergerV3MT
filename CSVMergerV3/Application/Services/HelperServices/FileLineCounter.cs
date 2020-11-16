using CSVMergerV3.Application.Domain;
using CSVMergerV3.Application.State;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Text;

namespace CSVMergerV3.Application.Services.HelperServices
{
    public class FileLineCounter : IFileLineCounter
    {
        private readonly IFileStreamProvider _fileStreamProvider;

        public FileLineCounter(IFileStreamProvider fileStreamProvider)
        {
            _fileStreamProvider = fileStreamProvider;
        }

        public long RetrieveLineCount(List<InputDataSet> inputSets)//not a good method
        {
            long totalLineCount = 0;

            List<StreamReader> fStreams = new List<StreamReader>();

            foreach (var inputset in inputSets)
            {
                fStreams.Add(_fileStreamProvider.GetReadStream(inputset.FilePath));
            }

            foreach (var fs in fStreams)
            {
                fs.ReadLine();
                using (fs)
                {
                    while (fs.EndOfStream != true)
                    {
                        totalLineCount += 1;
                        fs.ReadLine();
                    }
                }
            }

            return totalLineCount;
        }
    }
}
