using System;
using InstallationValidator.Core.Domain;
using OSPSuite.Infrastructure.Reporting;

namespace InstallationValidator.Core.Reporting
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