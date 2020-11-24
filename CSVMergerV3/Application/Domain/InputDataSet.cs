using CSVMergerV3.Application.Services.HelperServices;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace CSVMergerV3.Application.Domain
{
    public class InputDataSet : Dataset
    {
        private readonly IFileStreamProvider _fileStreamProvider;
        public BlockingCollection<string> UnProcessedLines = new BlockingCollection<string>(boundedCapacity: 100000);
        public BlockingCollection<string> ProcessedLines = new BlockingCollection<string>(boundedCapacity: 200000);
        public List<MapRule> MapRules = new List<MapRule>();
        private static Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
        public int OutputColumnCount = 0;

        public InputDataSet(IFileStreamProvider fileStreamProvider)
        {
            _fileStreamProvider = fileStreamProvider;
        }
        public void ReadLinesIntoCollection()
        {
            using (var fs = _fileStreamProvider.GetReadStream(FilePath))
            {
                fs.ReadLine();
                while (fs.EndOfStream != true)
                {
                    var line = fs.ReadLine();
                    UnProcessedLines.Add(line);
                }

                UnProcessedLines.CompleteAdding();
            }
        }

        public void ProcessLines(Thread[] threads)
        {

        }

        private void LineProcessor()
        {
            try
            {
                while (true)
                {
                    var line = UnProcessedLines.Take();
                    var lineArr = LineSplitter(line);
                    var resultArr = new string[Columns.Length];
                    RowMapper(MapRules, lineArr, resultArr);
                    var processedLine = string.Join(",", resultArr);
                    ProcessedLines.Add(processedLine);
                }
            }
            catch (InvalidOperationException)
            {
                ProcessedLines.CompleteAdding();
            }
        }

        private static string[] LineSplitter(string line)
        {
            var lineArr = CSVParser.Split(line);
            return lineArr;
        }

        private static void RowMapper(List<MapRule> rules, string[] attributes, string[] resultArry)
        {
            foreach (var rule in rules)
            {
                resultArry[rule.TargetIndex] = attributes[rule.OriginIndex];
            }
        }
    }
}
