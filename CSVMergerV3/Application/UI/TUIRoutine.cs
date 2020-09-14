using CSVMergerV3.Application.Domain;
using CSVMergerV3.Application.Services.HelperServices;
using CSVMergerV3.Application.Services.Validation;
using CSVMergerV3.Application.State;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public TUIRoutine(ILogger<TUIRoutine> log, IConfiguration config, IDataSetNameValidator dataSetNameValidator, IConfigurationState configurationState,
            IDirectoryValidator directoryValidator, IFileValidator fileValidator, IRuleChecker ruleChecker)
        {
            _log = log;
            _config = config;
            _dataSetNameValidator = dataSetNameValidator;
            _configurationState = configurationState;
            _directoryValidator = directoryValidator;
            _fileValidator = fileValidator;
            _ruleChecker = ruleChecker;
        }

        public void Run()
        {
            Welcome();
            GetNewDatasetName();
            SetColumnNamesFromUser();
            AskForTargetDirectory();
            AskForMergeFiles();
            MapDatasetRules();
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
                    _configurationState.setOutputSetName(newDataSetName);
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

            _configurationState.setOutputSetColumns(columnArr);
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
                    _configurationState.setOutputPath(outputPath);
                    break;
                }

            } while (true);
           
        }

        private void AskForMergeFiles()
        {

            _log.LogInformation("Please input path for the CSV files you wish to merge. One at a time.");

            do
            {

                var inputFile = Console.ReadLine();

                var fileExists = _fileValidator.ValidateFilePath(inputFile);

                if (!fileExists)
                {
                    _log.LogInformation("Directory does not exist.");
                    _log.LogInformation("Try another path\n");
                    return;
                }

                //make the new Dataset object and add it to the inputsets list on the configurationstate object.
                var ds = new Dataset();
                ds.FilePath = inputFile;
                _configurationState.AddInputset(ds);

                break;

            } while (true);

            do
            {
                _log.LogInformation("Would you like to add another file? [Y,N]");

                var yesNo = Console.ReadLine().ToUpper();

                if (yesNo != "Y" && yesNo != "N")
                {
                    _log.LogInformation("Please input a Y or N");
                }
                else if (yesNo == "Y")
                {
                    AskForMergeFiles();
                }
                else
                {
                    break;
                }
            } while (true);
        }

        private void MapDatasetRules()
        {
            Console.Clear();

            var inputSets = _configurationState.GetinputSets();
            var outputColumns = _configurationState.GetOutputColumns();
            var outputsetName = _configurationState.GetOutputsetName();

            for (int i = 0; i < inputSets.Count; i++)//loop through inputsets
            {
                var inputset = inputSets.ElementAt(i);

                for (int j = 0; j < inputset.Columns.Length; j++)//loop through inputset columns
                {
                    var inputsetColumn = inputset.Columns[j];

                    for (int k = 0; k < outputColumns.Length; k++)//loop through output columns
                    {
                        var outputColumn = outputColumns[k];

                        if (_ruleChecker.DoesRuleExist(inputset, k))//if a rule for the output (k) index exists, skip.
                        {
                            continue;
                        }
                        else
                        {
                            _log.LogInformation("\n");
                            _log.LogInformation($"Map [{inputset.DatasetName}] column \"{inputset.Columns[j]}\" to --> [{outputsetName}] column \"{outputColumns[k]}\"");
                            _log.LogInformation("\n");
                            _log.LogInformation("Yes, No, skip, or back [Y,N,S,B]");
                        }

                        var YNSB = Console.ReadLine().ToUpper();

                        do
                        {
                            if (YNSB.Equals("Y"))//if yes, make the maprule
                            {
                                var rule = new MapRule
                                {
                                    OriginIndex = j,
                                    TargetIndex = k
                                };

                                inputset.MapRules.Add(rule);

                                j++;
                                
                                break;
                            }

                            else if (YNSB.Equals("N"))
                            {
                                break;
                            }

                            else if (YNSB.Equals("S"))
                            {
                                j++;
                                k--;
                                break;
                            }

                            else if (YNSB.Equals("B"))
                            {
                                k--;
                                break;
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
    }
}
