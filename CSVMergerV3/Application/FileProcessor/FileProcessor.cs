using CSVMergerV3.Application.Domain;
using CSVMergerV3.Application.Services.HelperServices;
using CSVMergerV3.Application.State;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CSVMergerV3.Application.FileProcessor
{
    /**
     *  This class is the meat and potatos of the program. It's function is to take a dataset, read the lines of that dataset into a blocking collection,
     *  Take the lines out of that blocking collection, process them (e.g. split and map them), then put them into a new blocking collection (processed lines),
     *  the finally write them to the target file.
     */
    public class FileProcessor : IFileProcessor
    {
        private InputDataSet _dataset;
        private readonly IConfigurationState _configurationState;
        private readonly IFileStreamProvider _fileStreamProvider;
        private readonly IPercentageCalculator _percentageCalculator;
        private int OutputColumnCount;
        private string OuputPath;

        public FileProcessor(IConfigurationState configurationState, IFileStreamProvider fileStreamProvider, IPercentageCalculator percentageCalculator)
        {
            _configurationState = configurationState;
            _fileStreamProvider = fileStreamProvider;
            _percentageCalculator = percentageCalculator;
        }

        public void processConfig(int outputColumnCount, InputDataSet dataSet, string outputPath)
        {
            _dataset = dataSet;
            OutputColumnCount = outputColumnCount;
            OuputPath = outputPath;

            ProccessDataset();
        }

        private void ProccessDataset()
        {
            /**
             * Main orechestrating method for this class. Responsible for spinning up all of the threads. Get's the application threadc count
             * from the configuration state object, makes a thread array, and assigns the worker threads a process. 
             */

            Thread[] threads = new Thread[_configurationState.GetAppThreadCount()];

            threads[0] = new Thread(ReadLinesIntoCollection);//first thread reads from data into collection.
            threads[0].Start();

            for (int i = 1; i < threads.Length-1; i++)
            {
                //This loops sets up the main processing threads. They will be responsible for processing the lines that are
                //read into memory by the reading thread (thread[0]);

                threads[i] = new Thread(LineProcessor);
                threads[i].Start();
            }

            threads[threads.Length - 1] = new Thread(WriteLinesOutOfCollection);//Last thread will be responsible for writing the proccessed lines to the target file.
            threads[threads.Length - 1].Start();

            foreach(var thread in threads)
            {
                //This loops joins all of the threads, which means the program will wait here until all threads 
                //have finished and returned to the main thread.

                thread.Join();
            }

            return;
        }

        private void ReadLinesIntoCollection()//read the lines into the blocking collection
        {
            using (var fs = _fileStreamProvider.GetReadStream(_dataset.FilePath))
            {
                fs.ReadLine();//skip the first line, as it's just the column names.

                while (fs.EndOfStream != true)//while we are not at the end of the file...
                {
                    var line = fs.ReadLine();//read a line into the variable line
                    _dataset.UnProcessedLines.Add(line);//add that line to our dataset's unprocessedlines collection.
                }

                //Once all of the lines in the file have been read, we need to set the CompleteAddin flag on the UnProcessedLines collection.

                _dataset.UnProcessedLines.CompleteAdding();
            }
        }

        private void WriteLinesOutOfCollection()//this method will write all processed lines out of the collection to the target file.
        {
            using(var writeStream = _fileStreamProvider.GetWriteStream(OuputPath))
            {
                try
                {
                    while (true)
                    {
                        var line = _dataset.ProcessedLines.Take();//take a line out of the proccessed collection
                        writeStream.WriteLine(line);//write it to the file.
                        _percentageCalculator.IncrementProcessedLines();//call the precentageCalculators increment function. This is so the calculator can tell how many lines of been writen, as compared to how many there were for the job in total.
                    }
                }
                catch
                //You can catch the exception, but really we just want to return. This seems a bit janky, but the blocking collection is actaully designed to throw an exception once all data has been taken out of it and the CompleteAdding flag has been set. 
                //The reason for this is because the threads reading from a blocking collection can't know the difference between a collection that is currently empty, but may receive more data in the future, and a collection that is empty and will be receiving no more data.    
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
                    var line = _dataset.UnProcessedLines.Take();//take an unproccessed line and assign it to variable line
                    var lineArr = LineSplitter(line);//split that line into it's delimited attributes.
                    var resultArr = new string[OutputColumnCount];//make the result array the exact column count of the desired new output file, as this will ultimately be written asa complete row to the file.
                    resultArr = RowMapper(lineArr, resultArr);//see row mapper, try to do this by value instead.
                    var processedLine = string.Join(",", resultArr);//Join the newly mapped line together with a comma so it's now a single string.
                    _dataset.ProcessedLines.Add(processedLine);//add it to the data set's processed line collection where it will wait to be written to the output file.

                }
            }
            catch (InvalidOperationException)
            {
                _dataset.ProcessedLines.CompleteAdding();
            }
        }

        private string[] LineSplitter(string line)
        {
            var lineArr = _configurationState.GetRegex().Split(line);//splits the line according to the configurationstate regex. 
            return lineArr;//returns the string array.
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
