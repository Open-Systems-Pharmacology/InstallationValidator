using InstallationValidator.Core.Domain;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.TeXReporting.Items;

namespace InstallationValidator.Core.Reporting
{
   public class OperatingSystemInfoTeXBuilder : OSPSuiteTeXBuilder<OperatingSystemInfo>
   {
      private readonly ITeXBuilderRepository _builderRepository;

      public OperatingSystemInfoTeXBuilder(ITeXBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(OperatingSystemInfo operatingSystem, OSPSuiteTracker buildTracker)
      {
         var objectsToReport = new object[]
         {
            new Paragraph(Assets.Reporting.ComputerName),
            operatingSystem.ComputerName,
            new Paragraph(Assets.Reporting.OperatingSystem),
            operatingSystem.FriendlyName,
            new Paragraph(Assets.Reporting.Architecture),
            operatingSystem.Architecture,
            new Paragraph(Assets.Reporting.RunningOnVirtualMachine),
            boolFrom(operatingSystem.IsRunningOnVirtualMachine),
            new Paragraph(Assets.Reporting.RunningOnTerminalSession),
            boolFrom(operatingSystem.IsRunningOnTerminalSession),
            new Paragraph(Assets.Reporting.RunningOnTerminalSession),
            boolFrom(operatingSystem.IsRunningOnTerminalSession)
         };

         _builderRepository.Report(objectsToReport, buildTracker);
      }  

      private string boolFrom(bool booleanValue)
      {
         return booleanValue ? "Yes" : "No";
      }
   }
}