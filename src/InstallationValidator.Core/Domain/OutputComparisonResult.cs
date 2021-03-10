using InstallationValidator.Core.Assets;
using InstallationValidator.Core.Extensions;
using OSPSuite.Core.Domain;

namespace InstallationValidator.Core.Domain
{
   public abstract class ValueComparisonResult : IWithValidationState
   {
      public ValidationState State { get; }
      public string Message { get; }
      public double Deviation { get; set; }

      protected ValueComparisonResult(ValidationState state, string message = null)
      {
         State = state;
         Message = message ?? string.Empty;
      }
   }

   public class TimeComparisonResult : ValueComparisonResult
   {
      public TimeComparisonResult(ValidationState state, string message = null) : base(state, message)
      {
      }
   }

   public class OutputComparisonResult : ValueComparisonResult
   {
      public ComparisonSettings ComparisonSettings { get; }

      public string Path { get; }

      /// <summary>
      ///    Values of output 1. Can be null typically for a valid state
      /// </summary>
      public OutputResult Output1 { get; set; } = new NullOutputResult();

      /// <summary>
      ///    Values of output 2. Can be null typically for a valid state
      /// </summary>
      public OutputResult Output2 { get; set; } = new NullOutputResult();

      public OutputComparisonResult(string path, ComparisonSettings comparisonSettings, ValidationState state, string message = null) : base(state, message)
      {
         ComparisonSettings = comparisonSettings;
         Path = path;
      }

      public bool HasData => !Output1.IsNullOutput() && !Output2.IsNullOutput();
   }

   public class OutputResult
   {
      public string Caption { get; set; }
      public float[] Times { get; }
      public float[] Values { get; }
      public string Dimension { get; set; }

      public OutputResult(float[] times, float[] values)
      {
         Times = times;
         Values = values;
      }
   }

   public class NullOutputResult : OutputResult
   {
      public NullOutputResult() : base(new float[0], new float[0])
      {
      }
   }

   public class MissingOutputComparisonResult : OutputComparisonResult
   {
      public MissingOutputComparisonResult(string path, ComparisonSettings comparisonSettings, string simulationName, string folderPath, string folderType)
         : base(path, comparisonSettings, ValidationState.Invalid, Validation.OutputIsMissingFromSimulation(path, simulationName, folderPath, folderType))
      {
      }
   }
}