using System;
using System.Runtime.InteropServices;

namespace InstallationValidator.Core.Domain
{
   public class OperatingSystemInfo
   {
      private const int WINDOWS_11_MIN_BUILD_NUMBER = 22000;

      public string ComputerName { get; set; } = Environment.MachineName;

      public string Architecture { get; set; } = RuntimeInformation.OSArchitecture.ToString();

      public string FriendlyName { get; set; } = RuntimeInformation.OSDescription;

      public bool IsRunningOnVirtualMachine { get; set; }

      public bool IsRunningOnTerminalSession { get; set; }

      private string fixProductNameForWindows11(string productName, string currentBuildNumber)
      {
         if (string.IsNullOrEmpty(currentBuildNumber))
            return productName;

         if (productName.Contains("Server"))
            return productName;

         if (!int.TryParse(currentBuildNumber, out var buildNumber))
            return productName;

         if (buildNumber >= WINDOWS_11_MIN_BUILD_NUMBER)
            return productName.Replace("Windows 10", "Windows 11");

         return productName;
      }
   }
}
