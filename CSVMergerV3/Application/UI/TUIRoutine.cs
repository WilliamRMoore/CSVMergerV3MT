using CSVMergerV3.Application.Domain;
using CSVMergerV3.Application.Factories;
using CSVMergerV3.Application.Services.HelperServices;
using CSVMergerV3.Application.Services.Orchestrators;
using CSVMergerV3.Application.Services.Validation;
using CSVMergerV3.Application.State;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Transactions;

namespace CSVMergerV3.UI
{
    public class TUIRoutine : ITUIRoutine
    {
        private readonly ILogger<TUIRoutine> _log;
        private readonly IConfiguration _config;
        private readonly IDataSetNameValidator _dataSetNameValidator;
        private readonly IConfigurationState _configurationState;
        private readonly IDirectoryValidator _directoryValidator;
        private readonly IFileValidator _fileValidator;
        private readonly IRuleChecker _ruleChecker;
        private readonly IFileLineCounter _fileLineCounter;
        private readonly IJobState _jobState;
        private readonly ILineProducerConsumerOrechestrator _lineProducerConsumerOrechestrator;
        private readonly IOutputDatasetFactory _outputDatasetFactory;
        private readonly IInputDatasetFactory _inputDatasetFactory;

        public TUIRoutine(ILogger<TUIRoutine> log, IConfiguration config, IDataSetNameValidator dataSetNameValidator, IConfigurationState configurationState,
            IDirectoryValidator directoryValidator, IFileValidator fileValidator, IRuleChecker ruleChecker, IFileLineCounter fileLineCounter, IJobState jobState,
            ILineProducerConsumerOrechestrator lineProducerConsumerOrechestrator, IOutputDatasetFactory outputDatasetFactory, IInputDatasetFactory inputDatasetFactory)
        {
            _log = log;
            _config = config;
            _dataSetNameValidator = dataSetNameValidator;
            _configurationState = configurationState;
            _directoryValidator = directoryValidator;
            _fileValidator = fileValidator;
            _ruleChecker = ruleChecker;
            _fileLineCounter = fileLineCounter;
            _jobState = jobState;
            _lineProducerConsumerOrechestrator = lineProducerConsumerOrechestrator;
            _outputDatasetFactory = outputDatasetFactory;
            _inputDatasetFactory = inputDatasetFactory;
        }

        public void Run()
        {
            Welcome();
            GetNewDatasetName();
            SetColumnNamesFromUser();
            Console.Clear();
            AskForTargetDirectory();
            Console.Clear();
            AskForMergeFiles();
            Console.Clear();
            MapDatasetRules();
            Console.Clear();
            AskForThreadCount();
            Console.Clear();
            AskUserForConfirmation();
            Console.Clear();
            SetUp();
            StartJob();
        }

        private void Welcome()
        {
            _log.LogInformation("Welcome to the .csv generator tool, this tool is used to merge disparate data sets \n" +
                "into a .arff file for use in the Weka data analysis program.\n" +
                "\n Please specify the name of the dataset you would like to create \n" +
                "***WARNING DO NOT INCLUDE FILE EXTENSION IN NAME***\n" +
                "***WARNING DO NOT INCLUDE THE FOLLOWING CHARACTERS IN THE FILE NAME***\n" +
                "* . \" / \\ [ ] : ; | ,\n");
        }

        private void PrintDataSetNameCursor()
        {
            _log.LogInformation("Please enter new dataset name.");
        }

        private void GetNewDatasetName()
        {
            do
            {
                PrintDataSetNameCursor();
                string newDataSetName = Console.ReadLine();//validate this input
                var valid = _dataSetNameValidator.ValidateNewDatasetName(newDataSetName);

                if (valid)
                {
                    _jobState.SetOutputSetName(newDataSetName);
                    break;
                }
                else
                {
                    _log.LogInformation("\nPlease make sure your dataset name does not contain illegal characters\n");
                }

            } while (true);
        }

        private void SetColumnNamesFromUser()
        {
            _log.LogInformation("\nPlease List the Column Names seperated by commas.");

            var columnNames = Console.ReadLine();
            var columnArr = columnNames.Split(",");

            _jobState.SetOutputSetColumns(columnArr);
        }

        private void AskForTargetDirectory()
        {
            do
            {
                _log.LogInformation("Please input the directory where you would like new dataset to be output.");

                var outputPath = Console.ReadLine();

                var fileExists = _directoryValidator.DirectoryExists(outputPath);

                if (!fileExists)
                {
                    _log.LogInformation("Directory does not exist.");
                    _log.LogInformation("Try another path\n");
                }
                else
                {
                    _jobState.SetOutputPath(outputPath);
                    break;
                }

            } while (true);

        }

        private void AskForMergeFiles()
        {
            _log.LogInformation("Please input path for the CSV files you wish to merge. One at a time.");

            do
            {
                _log.LogInformation("Please input Filepath.");

                var inputFile = Console.ReadLine();

                var fileExists = _fileValidator.ValidateFilePath(inputFile);

                if (!fileExists)
                {
                    _log.LogInformation("Directory does not exist.");
                    _log.LogInformation("Try another path\n");
                    return;
                }

                //make the new Dataset object and add it to the inputsets list on the configurationstate object.
                var ds = _inputDatasetFactory.MakeInputDataset();
                ds.FilePath = inputFile;
                _jobState.AddInputset(ds);

                _log.LogInformation("Would you like to add another file? [Y,N]");

                var yesNo = Console.ReadLine().ToUpper();

                if (yesNo != "Y" && yesNo != "N")
                {
                    _log.LogInformation("Please input a Y or N");
                }
                else if (yesNo != "Y")
                {
                    break;
                }

            } while (true);
        }

        private void MapDatasetRules()//this method is terrible. Doesn't matter, all of this is getting replaced, anyway.
        {
           // Console.Clear();

            var inputSets = _jobState.GetinputSets();
            var outputColumns = _jobState.GetOutputColumns();
            var outputsetName = _jobState.GetOutputsetName();

            for (int i = 0; i < inputSets.Count; i++)//loop through inputsets
            {
                var inputset = inputSets.ElementAt(i);

                for (int j = 0; j < inputset.Columns.Length; j++)//loop through inputset columns
                {

                    for (int k = 0; k < outputColumns.Length; k++)//loop through output columns
                    {
                        var YNSB = "";

                        do
                        {
                            if (_ruleChecker.DoesRuleExist(inputset, k))//if a rule for the output (k) index exists, skip.
                            {
                                break;
                            }

                            _log.LogInformation("Map [{DatasetName}] column \"{inputsetColumns}\" to --> [{outputsetName}] column \"{outputColumns}\"\n",inputset.DatasetName, inputset.Columns[j], outputsetName, outputColumns[k]);

                            _log.LogInformation("Yes, No, skip, or back [Y,N,S,B]");

                            YNSB = Console.ReadLine().ToUpper();

                            if (YNSB.Equals("Y"))//if yes, make the maprule
                            {
                                var rule = new MapRule
                                {
                                    OriginIndex = j,
                                    TargetIndex = k
                                };

                                inputset.MapRules.Add(rule);

                                j++;
                            }

                            else if (YNSB.Equals("N"))//break do while loop, K gets incrimented
                            {

                                break;

                            }
                            else if (YNSB.Equals("S"))//Incriment J, unless J is at the last index, in whicn case, break the loop.
                            {

                                if(j == inputset.Columns.Length - 1)
                                {
                                    break;
                                }
                                else
                                {
                                    j++;
                                }

                            }
                            else if (YNSB.Equals("B"))//this doesn't work properly yet
                            {

                                if (k > 0)
                                {

                                    k--;//Need to remove a rule is the previous input generated one. Come back to this. Maybe use a queue and then convert to list once all rules are created.

                                }
                                else
                                {
                                    _log.LogInformation("Can not go back any further.");
                                }

                            }
                            else
                            {
                                _log.LogInformation("Invalid input.");
                            }

                        } while (true);

                        if (YNSB.Equals("y"))
                        {
                            break;
                        }
                    }
                }
            }
        }

        private void AskForThreadCount()
        {
            _log.LogInformation("This application will use a minimum of 3 threads, would you like to dedicate more? (Note: Order can not be gaurenteed beyond 3 threads) [Y/N]?");

            while (true)
            {
                var yn = Console.ReadLine().ToUpper();

                if(!yn.Equals("Y") && !yn.Equals("N"))
                {
                    _log.LogInformation("Invalid Input");
                    continue;
                }
                else if (yn.Equals("Y"))
                {
                    UserThreadCount();
                    break;
                }

                break;
            }
        }

        private void UserThreadCount()
        {

            while (true)
            {
               
                var threadInt = 3;

                _log.LogInformation("Please Input the Number of Threads. Must be Greater than {minimumThreadCount} and equal to or less than {systemThreadCount}", 3, _configurationState.GetCPUCount());
                var input = Console.ReadLine();
                bool success = int.TryParse(input, out threadInt);

                if (!success)
                {
                    _log.LogInformation("Invalid Input");
                    continue;
                }
                else if(threadInt < 3 || threadInt > _configurationState.GetCPUCount())
                {
                    _log.LogInformation("Please select a number no less than {minimumThreadCount} and no Greater than {systemThreadCount}", 3, _configurationState.GetCPUCount());
                    continue;
                }

                _configurationState.SetAppThreadCount(threadInt);
                break;
            }
        }
        private void AskUserForConfirmation()
        {
            //Console.Clear();

            var outputColumns = _jobState.GetOutputColumns();
            var inputSets = _jobState.GetinputSets();
            _log.LogInformation("A file with name {filename} will be created in Directoy {targetDirectory}.\n", _jobState.GetOutputsetName(), _jobState.GetOutputDirectory());
            
            foreach(var set in inputSets)
            {
                _log.LogInformation("Input set {inputsetName} will map to {targetDirectory} by rules of...\n", set.DatasetName, _jobState.GetOutputsetName());

                foreach(var rule in set.MapRules)
                {
                    _log.LogInformation("Input set Name [{inputSetName}] Column {columnName} --> Output set Name [{outputsetName}] Column {outColumnName}", set.DatasetName, set.Columns[rule.OriginIndex], _jobState.GetOutputsetName(), outputColumns[rule.TargetIndex]);
                }
            }

            _log.LogInformation("Confirm? [Y/N]");

            var confirm = Console.ReadLine().ToUpper();
        }

        private void SetUp()
        {
            _log.LogInformation("Preparing Job please wait...");
            _jobState.SetTotalLines();
            _log.LogInformation("A job to create a new file with {LineCount} lines has been created.", _jobState.GetTotalLines());
            _log.LogInformation("Press any key to execute job.");
            Console.ReadLine();
        }
        private void StartJob()
        {
            _log.LogInformation("Job is now starting");
            _lineProducerConsumerOrechestrator.Run();
            Thread.Sleep(350);
            _log.LogInformation("Jobe Complete!");
            _log.LogInformation("Press enter to exit.");
            Console.ReadLine();
        }
    }
}
