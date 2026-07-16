using InstallationValidator.Core.Domain;

namespace InstallationValidator.Core.Reporting.Markdown
{
   public class InstallationValidationResultMarkdownBuilder : MarkdownBuilder<InstallationValidationResult>
   {
      private readonly IMarkdownBuilderRepository _builderRepository;

      public InstallationValidationResultMarkdownBuilder(IMarkdownBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(InstallationValidationResult installationValidationResult, MarkdownReportContext context)
      {
         context.AppendHeading(Assets.Reporting.InstallationValidationResults, 1);

         context.AppendHeading(Assets.Reporting.OverallValidationResult, 2);
         _builderRepository.Report(new ValidationStateReport(installationValidationResult), context);

         _builderRepository.Report(installationValidationResult.RunSummary, context);
         _builderRepository.Report(installationValidationResult.ComparisonResult, context);
      }
   }
}
