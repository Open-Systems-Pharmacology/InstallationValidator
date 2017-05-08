using System;
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
            new Paragraph(Assets.Reporting.OverallComparisonResult),
            new ValidationStateReport(comparisonResult),
            new SubParagraph(Assets.Reporting.ComparisonFolder(comparisonResult.FolderPathCaption1)),
            comparisonResult.FolderPath1,
            new SubParagraph(Assets.Reporting.ComparisonFolder(comparisonResult.FolderPathCaption2)),
            comparisonResult.FolderPath2
         };

         objectsToReport.AddRange(reportSimulations(Assets.Reporting.InvalidSimulations, comparisonResult, ValidationState.Invalid));
         objectsToReport.AddRange(reportSimulations(Assets.Reporting.ValidWithWarningSimulations, comparisonResult, ValidationState.ValidWithWarnings));
         objectsToReport.AddRange(reportSimulations(Assets.Reporting.ValidSimulations, comparisonResult, ValidationState.Valid));

         _teXBuilderRepository.Report(objectsToReport, buildTracker);
      }

      private IEnumerable<object> reportSimulations(Func<int,int,string> sectionName, BatchComparisonResult comparisonResult, ValidationState state)
      {
         var simulationFiles = fileComparisonResultsForState(comparisonResult, state);
         var section = new SubSection(sectionName(simulationFiles.Count, comparisonResult.FileComparisonResults.Count));

         if (simulationFiles.Any())
            yield return section;

         foreach (var simulationFile in simulationFiles)
         {
            yield return simulationFile;
         }
      }

      private IReadOnlyList<FileComparisonResult> fileComparisonResultsForState(BatchComparisonResult comparisonResult, ValidationState state)
      {
         return comparisonResult.FileComparisonResults.Where(x => x.Is(state))
            .OrderBy(x => x.FileName)
            .ToList();
      }
   }
}