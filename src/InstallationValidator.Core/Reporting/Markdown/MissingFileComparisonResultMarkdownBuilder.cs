using InstallationValidator.Core.Domain;
using OSPSuite.Utility;

namespace InstallationValidator.Core.Reporting.Markdown
{
   public class MissingFileComparisonResultMarkdownBuilder : MarkdownBuilder<MissingFileComparisonResult>
   {
      private readonly IMarkdownBuilderRepository _builderRepository;

      public MissingFileComparisonResultMarkdownBuilder(IMarkdownBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(MissingFileComparisonResult result, MarkdownReportContext context)
      {
         context.AppendHeading($"{Assets.Reporting.Simulation}: {FileHelper.FileNameFromFileFullPath(result.FileName)}", 4);

         _builderRepository.Report(new ValidationStateReport(result, Assets.Reporting.ValidationResult), context);

         context.AppendHeading(Assets.Reporting.MissingFileValidation, 5);
         context.AppendParagraph(Assets.Reporting.MissingFileValidationMessage(
            result.FileName,
            result.FolderContainingFile,
            result.FolderWithoutFile));
      }
   }
}
