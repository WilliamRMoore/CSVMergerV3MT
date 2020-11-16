using CSVMergerV3.Application.Domain;
using CSVMergerV3.Application.State;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace CSVMergerV3.Application.Services.LineConsumers
{
    public class LineProcessor
    {
    //    static Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
    //    private readonly IJobState _jobState;

    //    public LineProcessor(IJobState jobState)
    //    {
    //        _jobState = jobState;
    //    }
    //    public void ProcessLines(Thread[] threads)
    //    {
    //        var inputsets = _jobState.GetinputSets();
    //        foreach(var set in inputsets)
    //        {
    //            for (int i = 0; i < threads.Length; i++)
    //            {
    //                threads[i] = new Thread(Processor);
    //                threads[i].Start(set);
    //            }
    //            foreach(var t in threads)
    //            {
    //                t.Join();
    //            }
    //        }
    //    }

    //    private static void Processor(object set)
    //    {
    //        var dataset = (Dataset)set;
    //        try
    //        {
    //            while (true)
    //            {
    //                var line = dataset.UnProcessedLines.Take();
    //                var lineArr = LineSplitter(line);
    //                var resultArr = new string[dataset.Columns.Length];
    //                RowMapper(dataset.MapRules, lineArr, resultArr);
    //                var processedLine = string.Join(",", resultArr);
    //                dataset.ProcessedLines.Add(processedLine);
    //            }
    //        }
    //        catch (InvalidOperationException)
    //        {
    //            dataset.ProcessedLines.CompleteAdding();
    //        }
    //    }

    //    private static string[] LineSplitter(string line)
    //    {
    //        var lineArr = CSVParser.Split(line);
    //        return lineArr;
    //    }

    //    private static void RowMapper(List<MapRule> rules, string[] attributes, string[] resultArry)
    //    {
    //        foreach(var rule in rules)
    //        {
    //            resultArry[rule.TargetIndex] = attributes[rule.OriginIndex];
    //        }
    //    }

    }
}
