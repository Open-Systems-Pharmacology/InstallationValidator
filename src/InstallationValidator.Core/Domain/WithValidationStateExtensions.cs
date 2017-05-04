using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;

namespace InstallationValidator.Core.Domain
{
   public static class WithValidationStateExtensions
   {
      public static ValidationState CombineStates(this IReadOnlyCollection<IWithValidationState> withValidationStates)
      {
         if (withValidationStates.Any(x => x.IsInvalid()))
            return ValidationState.Invalid;

         if (withValidationStates.Any(x => x.IsValidWithWarnings()))
            return ValidationState.ValidWithWarnings;

         return ValidationState.Valid;
      }

      public static bool IsValid(this IWithValidationState withValidationState)
      {
         return withValidationState.Is(ValidationState.Valid);
      }

      public static bool IsInvalid(this IWithValidationState withValidationState)
      {
         return withValidationState.Is(ValidationState.Invalid);
      }

      public static bool IsValidWithWarnings(this IWithValidationState withValidationState)
      {
         return withValidationState.Is(ValidationState.ValidWithWarnings);
      }

      public static bool Is(this IWithValidationState withValidationState, ValidationState state)
      {
         return withValidationState.State == state;
      }
   }
}