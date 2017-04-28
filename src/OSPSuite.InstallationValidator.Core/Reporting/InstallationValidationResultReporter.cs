using System.Collections.Generic;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.InstallationValidator.Core.Domain;
using OSPSuite.TeXReporting.Items;

namespace OSPSuite.InstallationValidator.Core.Reporting
{
   public class InstallationValidationResultReporter : OSPSuiteTeXReporter<InstallationValidationResult>
   {
      public override IReadOnlyCollection<object> Report(InstallationValidationResult installationValidationResult, OSPSuiteTracker buildTracker)
      {
         var validationResultsChapter = new Chapter(Assets.Reporting.InstallationValidationResults);
         buildTracker.Track(validationResultsChapter);
         return new List<object>
         {
            validationResultsChapter, installationValidationResult.RunSummary, installationValidationResult.ComparisonResult
         };
      }

   }
}