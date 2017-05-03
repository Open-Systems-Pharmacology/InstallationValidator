using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InstallationValidator.Core.Domain;
using OSPSuite.Core.Batch;

namespace InstallationValidator.Core.Services
{
   public interface IBatchOutputFileComparer
   {
      Task<OutputFileComparisonResult> Compare(string fileName, string folderPath1, string folderPath2, CancellationToken token);
   }

   public class BatchOutputFileComparer : IBatchOutputFileComparer
   {
      private readonly IBatchOutputLoader _batchOutputLoader;
      private readonly IComparisonStrategy _comparisonStrategy;

      public BatchOutputFileComparer(IBatchOutputLoader batchOutputLoader, IComparisonStrategy comparisonStrategy)
      {
         _batchOutputLoader = batchOutputLoader;
         _comparisonStrategy = comparisonStrategy;
      }

      public Task<OutputFileComparisonResult> Compare(string fileName, string folderPath1, string folderPath2, CancellationToken token)
      {
         return Task.Run(() =>
         {
            var outputFileComparision = new OutputFileComparisonResult(fileName, folderPath1, folderPath2);
            var simulation1 = simulationExportFrom(fileName, folderPath1);
            var simulation2 = simulationExportFrom(fileName, folderPath2);

            outputFileComparision.TimeComparison = compareTime(simulation1, simulation2);
            outputFileComparision.AddOutputComparisons(missingOutputsFrom(simulation1, simulation2));
            outputFileComparision.AddOutputComparisons(missingOutputsFrom(simulation2, simulation1));

            foreach (var outputValue in simulation1.Simulation.OutputValues.Where(p => simulation2.HasOutput(p.Path)))
            {
               token.ThrowIfCancellationRequested();
               outputFileComparision.AddOutputComparison(compareOutputs(simulation1, simulation2, outputValue, simulation2.OutputByPath(outputValue.Path)));
            }

            return outputFileComparision;
         }, token);
      }

      private TimeComparisonResult compareTime(BatchSimulationComparison simulationComparison1, BatchSimulationComparison simulationComparison2)
      {
         return _comparisonStrategy.CompareTime(simulationComparison1, simulationComparison2);
      }

      private OutputComparisonResult compareOutputs(BatchSimulationComparison simulationComparison1, BatchSimulationComparison simulationComparison2, BatchOutputValues outputValue1, BatchOutputValues outputValue2)
      {
         return _comparisonStrategy.CompareOutputs(new BatchOutputComparison(simulationComparison1, outputValue1), new BatchOutputComparison(simulationComparison2, outputValue2));
      }

      private IEnumerable<OutputComparisonResult> missingOutputsFrom(BatchSimulationComparison simulationComparison1, BatchSimulationComparison simulationComparison2)
      {
         return simulationComparison1.Simulation.OutputValues.Select(x => x.Path)
            .Where(p => !simulationComparison2.HasOutput(p))
            .Select(p => new MissingOutputComparisonResult(p, simulationComparison1.Name, simulationComparison2.Name));
      }

      private BatchSimulationComparison simulationExportFrom(string fileName, string folderPath)
      {
         return new BatchSimulationComparison(_batchOutputLoader.LoadFrom(Path.Combine(folderPath, fileName)), folderPath);
      }
   }
}