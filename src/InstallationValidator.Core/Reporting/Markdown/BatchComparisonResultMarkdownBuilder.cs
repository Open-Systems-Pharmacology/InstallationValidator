using System;
using System.Collections.Generic;
using System.Linq;
using InstallationValidator.Core.Domain;
using OSPSuite.Core.Domain;

namespace InstallationValidator.Core.Reporting.Markdown
{
   public class BatchComparisonResultMarkdownBuilder : MarkdownBuilder<BatchComparisonResult>
   {
      private readonly IMarkdownBuilderRepository _builderRepository;

      public BatchComparisonResultMarkdownBuilder(IMarkdownBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(BatchComparisonResult comparisonResult, MarkdownReportContext context)
      {
         context.AppendHeading(Assets.Reporting.BatchComparisonResults, 2);

         context.AppendHeading(Assets.Reporting.OverallComparisonResult, 3);
         _builderRepository.Report(new ValidationStateReport(comparisonResult), context);

         context.AppendHeading(Assets.Reporting.ComparisonFolder(comparisonResult.FolderPathCaption1), 3);
         context.AppendParagraph(comparisonResult.FolderPath1);

         context.AppendHeading(Assets.Reporting.ComparisonFolder(comparisonResult.FolderPathCaption2), 3);
         context.AppendParagraph(comparisonResult.FolderPath2);

         if (comparisonResult.ComparisonSettings.Exclusions.Any())
         {
            context.AppendHeading(Assets.Reporting.UsingExclusions, 3);
            foreach (var exclusion in comparisonResult.ComparisonSettings.Exclusions)
            {
               context.AppendListItem(exclusion);
            }
            context.AppendBlankLine();
         }

         reportSimulations(Assets.Reporting.InvalidSimulations, comparisonResult, ValidationState.Invalid, context);
         reportSimulations(Assets.Reporting.ValidWithWarningSimulations, comparisonResult, ValidationState.ValidWithWarnings, context);
         reportSimulations(Assets.Reporting.ValidSimulations, comparisonResult, ValidationState.Valid, context);
      }

      private void reportSimulations(Func<int, int, string> sectionName, BatchComparisonResult comparisonResult, ValidationState state, MarkdownReportContext context)
      {
         var simulationFiles = fileComparisonResultsForState(comparisonResult, state);

         if (!simulationFiles.Any())
            return;

         context.AppendHeading(sectionName(simulationFiles.Count, comparisonResult.FileComparisonResults.Count), 3);

         foreach (var simulationFile in simulationFiles)
         {
            _builderRepository.Report(simulationFile, context);
         }
      }

      private IReadOnlyList<FileComparisonResult> fileComparisonResultsForState(BatchComparisonResult comparisonResult, ValidationState state)
      {
         return comparisonResult.FileComparisonResults
            .Where(x => x.Is(state))
            .OrderBy(x => x.FileName)
            .ToList();
      }
   }
}
