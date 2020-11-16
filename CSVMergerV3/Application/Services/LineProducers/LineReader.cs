using CSVMergerV3.Application.Services.HelperServices;
using CSVMergerV3.Application.State;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CSVMergerV3.Application.Services.LineProducers
{
    public class LineReader : ILineReader
    {
        //private readonly IConfigurationState _configurationState;
        private readonly IJobState _jobState;
        private readonly IFileStreamProvider _fileStreamProvider;

        public LineReader(IConfigurationState configurationState, IJobState jobState, IFileStreamProvider fileStreamProvider)
        {
            //_configurationState = configurationState;
            _jobState = jobState;
            _fileStreamProvider = fileStreamProvider;
        }

        public void ReadLinesIntoCollection()
        {
            foreach (var inputset in _jobState.GetinputSets())
            {
                using (var fs = _fileStreamProvider.GetReadStream(inputset.FilePath))
                {
                    fs.ReadLine();
                    while (fs.EndOfStream != true)
                    {
                        var line = fs.ReadLine();
                        inputset.UnProcessedLines.Add(line);
                    }

                    inputset.UnProcessedLines.CompleteAdding();
                }
            }
        }
    }
}
