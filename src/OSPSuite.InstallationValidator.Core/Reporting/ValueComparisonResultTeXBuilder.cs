using System;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.InstallationValidator.Core.Assets;
using OSPSuite.InstallationValidator.Core.Domain;

namespace OSPSuite.InstallationValidator.Core.Reporting
{
   public abstract class ValueComparisonResultTeXBuilder<T> : OSPSuiteTeXBuilder<T> where T:  ValueComparisonResult
   {
      protected static string DeviationFor(T outputToReport)
      {
         return $"{Captions.Deviation}: {outputToReport.Deviation:0.####}{Environment.NewLine}";
      }
   }
}