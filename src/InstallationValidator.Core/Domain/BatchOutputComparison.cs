using OSPSuite.Core.Batch;

namespace InstallationValidator.Core.Domain
{
   public class BatchSimulationComparison
   {
      public BatchSimulationExport Simulation { get; }
      public string Folder { get; }
      public string Name => Simulation.Name;
      public BatchValues Times => Simulation.Times;

      public BatchSimulationComparison(BatchSimulationExport simulation, string folder)
      {
         Simulation = simulation;
         Folder = folder;
      }

      public bool HasOutput(string outputPath)
      {
         return OutputByPath(outputPath) != null;
      }

      public BatchOutputValues OutputByPath(string outputPath)
      {
         return Simulation.OutputValues.Find(x => string.Equals(x.Path, outputPath));
      }
   }

   public class BatchOutputComparison
   {
      public BatchSimulationComparison Simulation { get; }
      public BatchOutputValues OutputValues { get; }
      public string SimulationName => Simulation.Name;
      public string Folder => Simulation.Folder;
      public string Path => OutputValues.Path;
      public BatchValues Times => Simulation.Times;
      public float[] Values => OutputValues.Values;

      public BatchOutputComparison(BatchSimulationComparison simulation, BatchOutputValues outputValues)
      {
         Simulation = simulation;
         OutputValues = outputValues;
      }
   }
}