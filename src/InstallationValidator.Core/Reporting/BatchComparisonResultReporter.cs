using System.Collections.Generic;
using System.Linq;
using InstallationValidator.Core.Domain;
using OSPSuite.Core.Domain;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.TeXReporting.Items;

namespace InstallationValidator.Core.Reporting
{
   public class BatchComparisonResultTeXBuilder : OSPSuiteTeXBuilder<BatchComparisonResult>
   {
      private readonly ITeXBuilderRepository _teXBuilderRepository;

      public BatchComparisonResultTeXBuilder(ITeXBuilderRepository teXBuilderRepository)
      {
         _teXBuilderRepository = teXBuilderRepository;
      }

      public override void Build(BatchComparisonResult comparisonResult, OSPSuiteTracker buildTracker)
      {
         var objectsToReport = new List<object>
         {
            new Section(Assets.Reporting.BatchComparisonResults),
            new Paragraph(Assets.Reporting.ComparisonFolders),
            firstComparisonFolder(comparisonResult),
            new LineBreak(),
            secondComparisonFolder(comparisonResult),
            new Paragraph(Assets.Reporting.OverallComparisonResult),
            validationResultFor(comparisonResult)
         };

         var fileComparisonResults = comparisonResult.FileComparisonResults.OrderByDescending(x => x.State);
         objectsToReport.AddRange(fileComparisonResults);

         _teXBuilderRepository.Report(objectsToReport, buildTracker);
      }

      private ValidationState validationResultFor(BatchComparisonResult comparisonResult)
      {
         return comparisonResult.State;
      }

      private string secondComparisonFolder(BatchComparisonResult comparisonResult)
      {
         return comparisonResult.FolderPath2;
      }

      private string firstComparisonFolder(BatchComparisonResult comparisonResult)
      {
         return comparisonResult.FolderPath1;
      }
   }
}