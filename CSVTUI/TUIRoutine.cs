using Microsoft.Extensions.Configuration;
using Serilog;
using System;

namespace CSVTUI
{
    public class TUIRoutine
    {
        public TUIRoutine(ILogger<TUIRoutine> log, IConfiguration config)
        public void Run()
        {
            Console.WriteLine("Hello World!");
        }
    }
}
