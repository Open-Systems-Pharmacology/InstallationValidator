using System;
using System.Runtime.InteropServices;

namespace InstallationValidator.Core.Domain
{
   public class OperatingSystemInfo
   {
      public string ComputerName { get; set; } = Environment.MachineName;

      public string Architecture { get; set; } = RuntimeInformation.OSArchitecture.ToString();

      public string FriendlyName { get; set; } = RuntimeInformation.OSDescription;

      public bool IsRunningOnVirtualMachine { get; set; }

      public bool IsRunningOnTerminalSession { get; set; }
   }
}
