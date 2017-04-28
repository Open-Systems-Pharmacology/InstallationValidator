using System;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.TeXReporting.Builder;

namespace OSPSuite.InstallationValidator.Core.Reporting
{
   public class OperatingSystemTexBuilder : OSPSuiteTeXBuilder<OperatingSystem>
   {
      private readonly ITeXBuilderRepository _builderRepository;

      public OperatingSystemTexBuilder(ITeXBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(OperatingSystem operatingSystem, OSPSuiteTracker buildTracker)
      {
         _builderRepository.Report(operatingSystem.VersionString, buildTracker);
      }
   }
}