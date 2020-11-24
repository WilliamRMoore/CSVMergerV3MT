using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CSVMergerV3.Application.Services.HelperServices
{
    public class PercentageCalculator : IPercentageCalculator
    {
        private Object Batton = new Object();
        private decimal TotalLines;
        private readonly ILogger _logger;
        private decimal ProcessedLines = 0;
        private CancellationTokenSource CancellationToken;

        public PercentageCalculator(ILogger<PercentageCalculator> logger)
        {
            //_totalLines = totalLines;
            _logger = logger;
        }

        public void IncrementProcessedLines()
        {
            lock (Batton)
            {
                ProcessedLines ++;
            }
        }

        public void DisplayPercent(long totalLines)
        {
            TotalLines = (decimal)totalLines;
            CancellationToken = new CancellationTokenSource();
            ThreadPool.QueueUserWorkItem(new WaitCallback(Percent), CancellationToken.Token);
        }

        public void Stop()
        {
            CancellationToken.Cancel();
            CancellationToken.Dispose();
        }

        private void Percent(object obj)
        {
            CancellationToken token = (CancellationToken)obj;
            while (true)
            {
                //Console.SetCursorPosition(0, Console.CursorTop);
                
                var percentage = GetPercentage();
                _logger.LogInformation("{perect}% done", percentage);
                if (token.IsCancellationRequested)
                {
                    break;
                }
                Thread.Sleep(333);
            }
        }

        private decimal GetPercentage()
        {
            lock (Batton)
            {
                return Math.Floor((ProcessedLines / TotalLines) * 100);
            }
        }
    }
}
