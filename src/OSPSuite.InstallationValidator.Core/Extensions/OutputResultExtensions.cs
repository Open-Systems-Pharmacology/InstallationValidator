using OSPSuite.InstallationValidator.Core.Domain;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.InstallationValidator.Core.Extensions
{
   public static class OutputResultExtensions
   {
      public static bool IsNullOutput(this OutputResult theResult)
      {
         return theResult.IsAnImplementationOf<NullOutputResult>();
      }
   }
}
