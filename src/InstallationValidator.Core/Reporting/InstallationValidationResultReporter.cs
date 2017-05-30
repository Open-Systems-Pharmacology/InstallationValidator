using System.Collections.Generic;
using InstallationValidator.Core.Domain;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.TeXReporting.Items;

namespace InstallationValidator.Core.Reporting
{
   public class InstallationValidationResultReporter : OSPSuiteTeXReporter<InstallationValidationResult>
   {
      public override IReadOnlyCollection<object> Report(InstallationValidationResult installationValidationResult, OSPSuiteTracker buildTracker)
      {
         var validationResultsChapter = new Chapter(Assets.Reporting.InstallationValidationResults);
         buildTracker.Track(validationResultsChapter);

         return new List<object>
         {
            validationResultsChapter,
            new Paragraph(Assets.Reporting.OverallValidationResult),
            new ValidationStateReport(installationValidationResult),
            installationValidationResult.RunSummary,
            installationValidationResult.ComparisonResult
         };
      }
   }
}