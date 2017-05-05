using OSPSuite.Core.Domain;

namespace InstallationValidator.Core.Domain
{
   public class InstallationValidationResult : IWithValidationState
   {
      public ValidationRunSummary RunSummary { get; set; }
      public BatchComparisonResult ComparisonResult { get; set; }
      public ValidationState State => ComparisonResult?.State ?? ValidationState.Invalid;
   }
}