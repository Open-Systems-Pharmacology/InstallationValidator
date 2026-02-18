using InstallationValidator.Core.Domain;

namespace InstallationValidator.Core.Reporting.Markdown
{
   public class OperatingSystemInfoMarkdownBuilder : MarkdownBuilder<OperatingSystemInfo>
   {
      public override void Build(OperatingSystemInfo operatingSystem, MarkdownReportContext context)
      {
         context.AppendHeading(Assets.Reporting.OperatingSystem, 3);

         context.AppendListItem($"**{Assets.Reporting.ComputerName}**: {operatingSystem.ComputerName}");
         context.AppendListItem($"**OS**: {operatingSystem.FriendlyName}");
         context.AppendListItem($"**{Assets.Reporting.Architecture}**: {operatingSystem.Architecture}");
         context.AppendListItem($"**{Assets.Reporting.RunningOnVirtualMachine}**: {boolToYesNo(operatingSystem.IsRunningOnVirtualMachine)}");
         context.AppendListItem($"**{Assets.Reporting.RunningOnTerminalSession}**: {boolToYesNo(operatingSystem.IsRunningOnTerminalSession)}");
         context.AppendBlankLine();
      }

      private string boolToYesNo(bool value) => value ? "Yes" : "No";
   }
}
