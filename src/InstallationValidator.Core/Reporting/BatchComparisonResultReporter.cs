using System.Collections.Generic;
using InstallationValidator.Core.Domain;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.TeXReporting.Items;

namespace InstallationValidator.Core.Reporting
{
   public class BatchComparisonResultReporter : OSPSuiteTeXReporter<BatchComparisonResult>
   {
      public override IReadOnlyCollection<object> Report(BatchComparisonResult batchComparisonResult, OSPSuiteTracker buildTracker)
      {
         var folderComparisonResultsChapter = new Chapter(Assets.Reporting.FolderComparisonResults);
         buildTracker.Track(folderComparisonResultsChapter);

         return new List<object>
         {
            folderComparisonResultsChapter,
            new ValidationStateReport(batchComparisonResult),
            batchComparisonResult
         };
      }
   }
}