using System.Collections.Generic;
using System.Linq;
using InstallationValidator.Core.Domain;
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

            new Paragraph(Assets.Reporting.OverallComparisonResult),
            new ValidationStateReport(comparisonResult),
 
            new SubParagraph(Assets.Reporting.ComparisonFolder(comparisonResult.FolderPathCaption1)),
            comparisonResult.FolderPath1,

            new SubParagraph(Assets.Reporting.ComparisonFolder(comparisonResult.FolderPathCaption2)),
            comparisonResult.FolderPath2
         };

         var fileComparisonResults = comparisonResult.FileComparisonResults
            .OrderByDescending(x => x.State)
            .ThenBy(x => x.FileName);

         objectsToReport.AddRange(fileComparisonResults);

         _teXBuilderRepository.Report(objectsToReport, buildTracker);
      }
   }
}