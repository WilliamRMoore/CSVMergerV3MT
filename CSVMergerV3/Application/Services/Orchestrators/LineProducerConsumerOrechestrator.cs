using CSVMergerV3.Application.Services.LineProducers;
using CSVMergerV3.Application.State;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CSVMergerV3.Application.Services.Orchestrators
{
    public class LineProducerConsumerOrechestrator : ILineProducerConsumerOrechestrator
    {
        private readonly IJobState _jobState;

        public LineProducerConsumerOrechestrator(IJobState jobState)
        {
            _jobState = jobState;
        }

        public void Run()
        {
            _jobState.ExecuteJob();
        }
    }
}
