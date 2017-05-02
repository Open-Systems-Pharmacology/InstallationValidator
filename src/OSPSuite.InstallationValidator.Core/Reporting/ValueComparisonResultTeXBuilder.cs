using System;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.InstallationValidator.Core.Domain;

namespace OSPSuite.InstallationValidator.Core.Reporting
{
   public abstract class ValueComparisonResultTeXBuilder<T> : OSPSuiteTeXBuilder<T> where T:  ValueComparisonResult
   {
      protected static string DeviationFor(T outputToReport)
      {
         return $"{Assets.Reporting.Deviation}: {outputToReport.Deviation:0.####}{Environment.NewLine}";
      }

      protected static string ValidationMessageFor(T outputToReport)
      {
         return $"{outputToReport.Message}";
      }
   }
}