using System;
using InstallationValidator.Core.Domain;
using OSPSuite.Core.Domain;
using OSPSuite.Infrastructure.Reporting;

namespace InstallationValidator.Core.Reporting
{
   public abstract class ValueComparisonResultTeXBuilder<T> : OSPSuiteTeXBuilder<T> where T : ValueComparisonResult
   {
      private readonly DoubleFormatter _doubleFormatter;

      protected ValueComparisonResultTeXBuilder()
      {
         _doubleFormatter = new DoubleFormatter();
      }

      protected string DeviationFor(T outputToReport)
      {
         return $"{Assets.Reporting.Deviation}: {_doubleFormatter.Format(outputToReport.Deviation)}{Environment.NewLine}";
      }

      protected static string ValidationMessageFor(T outputToReport)
      {
         return $"{outputToReport.Message}";
      }
   }
}