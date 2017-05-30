using InstallationValidator.Core.Domain;
using OSPSuite.Utility.Extensions;

namespace InstallationValidator.Core.Extensions
{
   public static class OutputResultExtensions
   {
      public static bool IsNullOutput(this OutputResult theResult)
      {
         return theResult.IsAnImplementationOf<NullOutputResult>();
      }
   }
}
