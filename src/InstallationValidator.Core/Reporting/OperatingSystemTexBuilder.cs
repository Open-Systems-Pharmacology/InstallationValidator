using System;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.TeXReporting.Builder;

namespace InstallationValidator.Core.Reporting
{
   public class OperatingSystemTeXBuilder : OSPSuiteTeXBuilder<OperatingSystem>
   {
      private readonly ITeXBuilderRepository _builderRepository;

      public OperatingSystemTeXBuilder(ITeXBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(OperatingSystem operatingSystem, OSPSuiteTracker buildTracker)
      {
         _builderRepository.Report(operatingSystem.VersionString, buildTracker);
      }
   }
}