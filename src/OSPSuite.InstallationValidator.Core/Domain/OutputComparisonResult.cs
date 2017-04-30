using OSPSuite.Core.Domain;
using OSPSuite.InstallationValidator.Core.Assets;

namespace OSPSuite.InstallationValidator.Core.Domain
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
      public string Path { get; }

      /// <summary>
      ///    Values of output 1. Can be null typically for a valid state
      /// </summary>
      public OutputResult Output1 { get; set; }

      /// <summary>
      ///    Values of output 2. Can be null typically for a valid state
      /// </summary>
      public OutputResult Output2 { get; set; }

      public OutputComparisonResult(string path, ValidationState state, string message = null) : base(state, message)
      {
         Path = path;
         Output1 = new NullOutputResult();
         Output2 = new NullOutputResult();
      }
   }

   public class OutputResult
   {
      public float[] Times { get; }
      public float[] Values { get; }

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
      public MissingOutputComparisonResult(string path, string simulationName, string folderPath)
         : base(path, ValidationState.Invalid, Validation.OutputIsMissingFromSimulation(path, simulationName, folderPath))
      {
      }
   }
}