using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;

namespace OSPSuite.InstallationValidator.Core.Domain
{
   public static class WithValidationStateExtensions
   {
      public static ValidationState CombineStates(this IReadOnlyCollection<IWithValidationState> withValidationStates)
      {
         if (withValidationStates.Any(x => x.State == ValidationState.Invalid))
            return ValidationState.Invalid;

         if (withValidationStates.Any(x => x.State == ValidationState.ValidWithWarnings))
            return ValidationState.ValidWithWarnings;

         return ValidationState.Valid;
      }

      public static bool IsStateValid(this IWithValidationState withValidationState)
      {
         return withValidationState.State == ValidationState.Valid;
      }
   }
}