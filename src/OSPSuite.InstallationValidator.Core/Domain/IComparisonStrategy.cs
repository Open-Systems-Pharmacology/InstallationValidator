namespace OSPSuite.InstallationValidator.Core.Domain
{
   public interface IComparisonStrategy
   {
      OutputComparisonResult CompareOutputs(BatchOutputComparison outputValues1, BatchOutputComparison outputValues2);
      TimeComparisonResult CompareTime(BatchSimulationComparison simulation1, BatchSimulationComparison simulation2);
   }
}