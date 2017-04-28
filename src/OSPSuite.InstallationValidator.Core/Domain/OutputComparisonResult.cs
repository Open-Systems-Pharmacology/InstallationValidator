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

      public OutputComparisonResult(string path, ValidationState state, string message = null) : base(state, message)
      {
         Path = path;
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