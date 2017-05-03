using InstallationValidator.Core.Domain;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.TeXReporting.Items;

namespace InstallationValidator.Core.Reporting
{
   public class TimeComparisonResultTeXBuilder : ValueComparisonResultTeXBuilder<TimeComparisonResult>
   {
      private readonly ITeXBuilderRepository _builderRepository;

      public TimeComparisonResultTeXBuilder(ITeXBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(TimeComparisonResult timeComparisonResult, OSPSuiteTracker buildTracker)
      {
         _builderRepository.Report(new object[]
         {
            new LineBreak(),
            Assets.Reporting.TimeComparisonValidation,
            new LineBreak(), ValidationMessageFor(timeComparisonResult),
            new LineBreak(), DeviationFor(timeComparisonResult)
         }, buildTracker);
      }
   }
}