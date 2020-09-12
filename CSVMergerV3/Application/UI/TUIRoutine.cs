using CSVMergerV3.Application.Services.Validation;
using CSVMergerV3.Application.State;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSVMergerV3.UI
{
    public class TUIRoutine : ITUIRoutine
    {
        private readonly ILogger<TUIRoutine> _log;
        private readonly IConfiguration _config;
        private readonly IDataSetNameValidator _dataSetNameValidator;
        private readonly IConfigurationState _configurationState;

        public TUIRoutine(ILogger<TUIRoutine> log, IConfiguration config, IDataSetNameValidator dataSetNameValidator, IConfigurationState configurationState)
        {
            _log = log;
            _config = config;
            _dataSetNameValidator = dataSetNameValidator;
            _configurationState = configurationState;
        }

        public void Run()
        {
            Welcome();
            GetNewDatasetName();
            SetColumnNamesFromUser();
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
    }
}
