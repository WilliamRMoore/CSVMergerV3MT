namespace CSVMergerV3.Application.Services.HelperServices
{
    public interface IPercentageCalculator
    {
        void DisplayPercent(long totalLines);
        void IncrementProcessedLines();
        void Stop();
    }
}