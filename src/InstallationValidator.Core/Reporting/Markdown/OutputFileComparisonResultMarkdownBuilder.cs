using System.Linq;
using InstallationValidator.Core.Domain;
using OSPSuite.Core.Domain;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace InstallationValidator.Core.Reporting.Markdown
{
   public class OutputFileComparisonResultMarkdownBuilder : MarkdownBuilder<OutputFileComparisonResult>
   {
      private readonly IMarkdownBuilderRepository _builderRepository;
      private readonly DoubleFormatter _doubleFormatter = new DoubleFormatter();

      public OutputFileComparisonResultMarkdownBuilder(IMarkdownBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(OutputFileComparisonResult fileComparisonResult, MarkdownReportContext context)
      {
         context.AppendHeading($"{Assets.Reporting.Simulation}: {FileHelper.FileNameFromFileFullPath(fileComparisonResult.FileName)}", 4);

         _builderRepository.Report(new ValidationStateReport(fileComparisonResult, Assets.Reporting.ValidationResult), context);

         if (!fileComparisonResult.IsValid())
         {
            context.AppendParagraph(Assets.Reporting.AbsoluteToleranceIs(_doubleFormatter.Format(fileComparisonResult.AbsTol)));
            context.AppendParagraph(Assets.Reporting.RelativeToleranceIs(_doubleFormatter.Format(fileComparisonResult.RelTol)));
         }

         if (!fileComparisonResult.TimeComparison.IsValid())
         {
            _builderRepository.Report(fileComparisonResult.TimeComparison, context);
         }

         var allInvalidOutputs = fileComparisonResult.OutputComparisonResults.Where(x => !x.IsValid());
         allInvalidOutputs.Each(x => _builderRepository.Report(x, context));

         var allValidOutputsWithData = fileComparisonResult.OutputComparisonResults.Where(x => x.IsValid()).Where(x => x.HasData);
         allValidOutputsWithData.Each(x => _builderRepository.Report(x, context));
      }
   }
}