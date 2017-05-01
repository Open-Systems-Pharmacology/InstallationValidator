using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.InstallationValidator.Core.Assets;
using OSPSuite.InstallationValidator.Core.Domain;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.TeXReporting.Items;

namespace OSPSuite.InstallationValidator.Core.Reporting
{
   public class BatchComparisonResultBuilder : OSPSuiteTeXBuilder<BatchComparisonResult>
   {
      private readonly ITeXBuilderRepository _teXBuilderRepository;

      public BatchComparisonResultBuilder(ITeXBuilderRepository teXBuilderRepository)
      {
         _teXBuilderRepository = teXBuilderRepository;
      }

      public override void Build(BatchComparisonResult comparisonResult, OSPSuiteTracker buildTracker)
      {
         var objectsToReport = new List<object>
         {
            new Section(Assets.Reporting.BatchComparisonResults),
            new Paragraph(Assets.Reporting.ComparisonFolders), firstComparisonFolder(comparisonResult), new LineBreak(), secondComparisonFolder(comparisonResult),
            new Paragraph(Assets.Reporting.DefaultTolerances), tolerancesFor(comparisonResult),
            new Paragraph(Assets.Reporting.OverallComparisonResult), validationResultFor(comparisonResult)
         };

         var fileComparisonResults = comparisonResult.FileComparisonResults.Where(x => x.State != ValidationState.Valid).ToList();
         if(fileComparisonResults.Any())
            objectsToReport.Add(new Paragraph(Assets.Reporting.FailedValidations));
         objectsToReport.AddRange(fileComparisonResults);

         _teXBuilderRepository.Report(objectsToReport, buildTracker);
      }

      private ValidationState validationResultFor(BatchComparisonResult comparisonResult)
      {
         return comparisonResult.State;
      }

      private object tolerancesFor(BatchComparisonResult comparisonResult)
      {
         return "todo";
      }

      private object secondComparisonFolder(BatchComparisonResult comparisonResult)
      {
         return comparisonResult.FolderPath2;
      }

      private object firstComparisonFolder(BatchComparisonResult comparisonResult)
      {
         return comparisonResult.FolderPath1;
      }
   }
}
