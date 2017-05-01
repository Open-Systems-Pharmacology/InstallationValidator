using OSPSuite.Infrastructure.Reporting;
using OSPSuite.InstallationValidator.Core.Assets;
using OSPSuite.InstallationValidator.Core.Domain;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.TeXReporting.Items;

namespace OSPSuite.InstallationValidator.Core.Reporting
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
            Captions.TimeComparisonValidation, 
            new LineBreak(), DeviationFor(timeComparisonResult)
         }, buildTracker);
      }
   }
}