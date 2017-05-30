using System.Drawing;
using OSPSuite.Core.Domain;

namespace InstallationValidator.Core.Domain
{
   public static class ValidationStateExtensions
   {
      public static Color ValidationColor(this ValidationState validationState)
      {
         switch (validationState)
         {
            case ValidationState.Invalid:
               return Color.Red;
            case ValidationState.ValidWithWarnings:
               return Color.Orange;
            case ValidationState.Valid:
               return Color.Green;
            default:
               return Color.Empty;
         }
      }
   }
}