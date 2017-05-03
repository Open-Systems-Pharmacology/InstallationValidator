using InstallationValidator.Core.Domain;
using OSPSuite.Core.Domain;

namespace InstallationValidator.Helpers
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