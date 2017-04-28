using System.Collections.Generic;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.InstallationValidator.Core.Domain;
using OSPSuite.TeXReporting.Items;

namespace OSPSuite.InstallationValidator.Core
{
   public class BatchComparisonResultReporter : OSPSuiteTeXReporter<BatchComparisonResult>
   {
      public override IReadOnlyCollection<object> Report(BatchComparisonResult comparisonResult, OSPSuiteTracker buildTracker)
      {
         var chapter = new Chapter("Log");
         var report = new List<object> { chapter };
         buildTracker.AddReference(comparisonResult, chapter);
         report.AddRange(this.ReportDescription(comparisonResult, buildTracker));
         return report;
      }
   }
}
