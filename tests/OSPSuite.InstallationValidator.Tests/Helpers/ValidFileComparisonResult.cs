using OSPSuite.Core.Domain;
using OSPSuite.InstallationValidator.Core.Domain;

namespace OSPSuite.InstallationValidator.Helpers
{
   public class StateFileComparisonResult : FileComparisonResult
   {
      public StateFileComparisonResult(ValidationState state) : base(string.Empty, string.Empty, string.Empty)
      {
         State = state;
      }

      public override ValidationState State { get; }
   }
}