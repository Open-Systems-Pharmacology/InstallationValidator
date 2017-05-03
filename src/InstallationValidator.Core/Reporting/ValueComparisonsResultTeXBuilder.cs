using System.Collections.Generic;
using FluentNHibernate.Utils;
using InstallationValidator.Core.Domain;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.TeXReporting.Builder;

namespace InstallationValidator.Core.Reporting
{
   public abstract class ValueComparisonsResultTeXBuilder : OSPSuiteTeXBuilder<IEnumerable<ValueComparisonResult>>
   {
      private readonly ITeXBuilderRepository _builderRepository;

      protected ValueComparisonsResultTeXBuilder(ITeXBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }
      public override void Build(IEnumerable<ValueComparisonResult> outputsToReport, OSPSuiteTracker buildTracker)
      {
         outputsToReport.Each(x => _builderRepository.Report(x, buildTracker));
      }
   }
}