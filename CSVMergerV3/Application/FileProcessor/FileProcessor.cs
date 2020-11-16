using CSVMergerV3.Application.Domain;
using CSVMergerV3.Application.Services.HelperServices;
using CSVMergerV3.Application.State;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CSVMergerV3.Application.FileProcessor
{
    public class FileProcessor : IFileProcessor
    {
        private InputDataSet _dataset;
        private IConfigurationState _configurationState;
        private readonly IFileStreamProvider _fileStreamProvider;
        private int OutputColumnCount;
        private string[] OuputColumns;
        private string OuputPath;

        public FileProcessor(IConfigurationState configurationState, IFileStreamProvider fileStreamProvider)
        {
            _configurationState = configurationState;
            _fileStreamProvider = fileStreamProvider;
        }

        public void processConfig(string[] outputColumns, int outputColumnCount, InputDataSet dataSet, string outputPath)
        {
            _dataset = dataSet;
            OutputColumnCount = outputColumnCount;
            OuputColumns = outputColumns;
            OuputPath = outputPath;

            ProccessDataset();
        }

        private void ProccessDataset()
        {
            Thread[] threads = new Thread[_configurationState.GetAppThreadCount()];
            threads[0] = new Thread(ReadLinesIntoCollection);
            threads[0].Start();

            for (int i = 1; i < threads.Length-1; i++)
            {
                threads[i] = new Thread(LineProcessor);
                threads[i].Start();
            }

            threads[threads.Length - 1] = new Thread(WriteLinesOutOfCollection);
            threads[threads.Length - 1].Start();

            foreach(var thread in threads)
            {
                thread.Join();
            }

            return;
        }

        private void ReadLinesIntoCollection()//read the lines into the blocking collection
        {
            using (var fs = _fileStreamProvider.GetReadStream(_dataset.FilePath))
            {
                fs.ReadLine();//skip the first line, as it's just the column names.

                while (fs.EndOfStream != true)
                {
                    var line = fs.ReadLine();
                    _dataset.UnProcessedLines.Add(line);
                }

                _dataset.UnProcessedLines.CompleteAdding();
            }
        }

        private void WriteLinesOutOfCollection()
        {
            using(var writeStream = _fileStreamProvider.GetWriteStream(OuputPath))
            {
                try
                {
                    while (true)
                    {
                        var line = _dataset.ProcessedLines.Take();
                        writeStream.WriteLine(line);
                    }
                }
                catch
                {
                    //maybe add some loggging here, maybe not, 
                }
            }
        }

        private void LineProcessor()
        {
            try
            {
                while (true)
                {
                    var line = _dataset.UnProcessedLines.Take();
                    var lineArr = LineSplitter(line);
                    var resultArr = new string[OutputColumnCount];//make the result array the exact column count of the desired new output file
                    resultArr = RowMapper(lineArr, resultArr);//see row mapper, try to do this by value instead.
                    var processedLine = string.Join(",", resultArr);
                    _dataset.ProcessedLines.Add(processedLine);

                }
            }
            catch (InvalidOperationException)
            {
                _dataset.ProcessedLines.CompleteAdding();
            }
        }

        private string[] LineSplitter(string line)
        {
            var lineArr = _configurationState.GetRegex().Split(line);
            return lineArr;
        }

        private string[] RowMapper(string[] attributes, string[] resultArray)
        {
            foreach (var rule in _dataset.MapRules)
            {
                resultArray[rule.TargetIndex] = attributes[rule.OriginIndex];
            }

            return resultArray;// try not using a return here later, see if it works. Might be a pass by value case.
        }
    }
}
