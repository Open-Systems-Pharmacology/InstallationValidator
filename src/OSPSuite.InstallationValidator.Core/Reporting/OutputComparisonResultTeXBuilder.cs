using OSPSuite.Infrastructure.Reporting;
using OSPSuite.InstallationValidator.Core.Assets;
using OSPSuite.InstallationValidator.Core.Domain;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.TeXReporting.Items;

namespace OSPSuite.InstallationValidator.Core.Reporting
{
   public class OutputComparisonResultTeXBuilder : ValueComparisonResultTeXBuilder<OutputComparisonResult>
   {
      private readonly ITeXBuilderRepository _builderRepository;

      public OutputComparisonResultTeXBuilder(ITeXBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }
      public override void Build(OutputComparisonResult outputToReport, OSPSuiteTracker buildTracker)
      {
         _builderRepository.Report(new object[]
         {
            new LineBreak(),
            Assets.Reporting.OutputComparisonValidation,
            new LineBreak(), ValidationMessageFor(outputToReport),
            new LineBreak(), outputPathFor(outputToReport),
            new LineBreak(), DeviationFor(outputToReport)
         }, buildTracker);
      }

      private static string outputPathFor(OutputComparisonResult outputToReport)
      {
         return $"{Assets.Reporting.OutputPath}: {outputToReport.Path}";
      }
   }
}