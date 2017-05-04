using InstallationValidator.Core.Domain;
using OSPSuite.Core.Domain;

namespace InstallationValidator.Core.Reporting
{
   public class ValidationStateReport : IWithValidationState
   {
      public ValidationState State { get; }
      public string Caption { get; }

      public ValidationStateReport(IWithValidationState withValidationState, string caption = "") : this(withValidationState.State, caption)
      {
      }

      public ValidationStateReport(ValidationState validationState, string caption = "")
      {
         State = validationState;
         Caption = caption;
      }
   }
}