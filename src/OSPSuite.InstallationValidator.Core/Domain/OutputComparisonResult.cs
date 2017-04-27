using OSPSuite.Core.Domain;

namespace OSPSuite.InstallationValidator.Core.Domain
{
   public class OutputComparisonResult : IWithValidationState
   {
      public ValidationState State { get; }
   }
}