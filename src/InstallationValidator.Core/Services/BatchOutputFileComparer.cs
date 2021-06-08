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
      Task<OutputFileComparisonResult> Compare(string fileName, ComparisonSettings comparisonSettings, CancellationToken token);
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

      public Task<OutputFileComparisonResult> Compare(string fileName, ComparisonSettings comparisonSettings, CancellationToken token)
      {
         return Task.Run(() =>
         {
            var outputFileComparision = new OutputFileComparisonResult(fileName, comparisonSettings.FolderPath1, comparisonSettings.FolderPath2);
            var simulation1 = simulationExportFrom(fileName, comparisonSettings.FolderPath1);
            var simulation2 = simulationExportFrom(fileName, comparisonSettings.FolderPath2);

            outputFileComparision.TimeComparison = compareTime(simulation1, simulation2, comparisonSettings);
            if(!comparisonSettings.IgnoreRemovedCurves)
               outputFileComparision.AddOutputComparisons(missingOutputsFrom(simulation1, simulation2, comparisonSettings, comparisonSettings.FolderPath2));

            if (!comparisonSettings.IgnoreAddedCurves)
               outputFileComparision.AddOutputComparisons(missingOutputsFrom(simulation2, simulation1, comparisonSettings, comparisonSettings.FolderPath1));

            outputFileComparision.AbsTol = simulation1.Simulation.AbsTol;
            outputFileComparision.RelTol = simulation1.Simulation.RelTol;


            var outputComparisonResults = new List<OutputComparisonResult>();
            var outputValuesToCompare = simulation1.Simulation.OutputValues.Where(p => simulation2.HasOutput(p.Path)).Where(p => comparisonSettings.CanCompare(p.Path));
            foreach (var outputValue in outputValuesToCompare)
            {
               token.ThrowIfCancellationRequested();
               outputComparisonResults.Add(compareOutputs(simulation1, simulation2, outputValue, simulation2.OutputByPath(outputValue.Path), comparisonSettings));
            }

            outputFileComparision.AddOutputComparisons(selectOutputComparisonFrom(outputComparisonResults, comparisonSettings));
           
            return outputFileComparision;
         }, token);
      }

      private IEnumerable<OutputComparisonResult> selectOutputComparisonFrom(IReadOnlyList<OutputComparisonResult> outputComparisonResults, ComparisonSettings comparisonSettings)
      {
         if (!comparisonSettings.NumberOfCurves.HasValue)
            return outputComparisonResults;

         return outputComparisonResults.OrderByDescending(x => x.Deviation).Take(comparisonSettings.NumberOfCurves.Value);
      }

      private TimeComparisonResult compareTime(BatchSimulationComparison simulationComparison1, BatchSimulationComparison simulationComparison2, ComparisonSettings comparisonSettings)
      {
         return _comparisonStrategy.CompareTime(simulationComparison1, simulationComparison2, comparisonSettings);
      }

      private OutputComparisonResult compareOutputs(BatchSimulationComparison simulationComparison1, BatchSimulationComparison simulationComparison2, BatchOutputValues outputValue1, BatchOutputValues outputValue2, ComparisonSettings comparisonSettings)
      {
         return _comparisonStrategy.CompareOutputs(new BatchOutputComparison(simulationComparison1, outputValue1), new BatchOutputComparison(simulationComparison2, outputValue2), comparisonSettings);
      }

      private IEnumerable<OutputComparisonResult> missingOutputsFrom(
         BatchSimulationComparison simulationComparison1, 
         BatchSimulationComparison simulationComparison2, 
         ComparisonSettings comparisonSettings,
         string folderType)
      {
         return simulationComparison1.Simulation.OutputValues.Select(x => x.Path)
            .Where(comparisonSettings.CanCompare)
            .Where(p => !simulationComparison2.HasOutput(p))
            .Select(p => new MissingOutputComparisonResult(p, comparisonSettings, simulationComparison1.Name, simulationComparison2.Folder, folderType));
      }

      private BatchSimulationComparison simulationExportFrom(string fileName, string folderPath)
      {
         return new BatchSimulationComparison(_batchOutputLoader.LoadFrom(Path.Combine(folderPath, fileName)), folderPath);
      }
   }
}