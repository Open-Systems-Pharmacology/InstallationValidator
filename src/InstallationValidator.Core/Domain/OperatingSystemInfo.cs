using System;
using System.Linq;
using System.Management;
using System.Windows.Forms;
using Microsoft.Win32;
using OSPSuite.Utility.Extensions;

namespace InstallationValidator.Core.Domain
{
   public class OperatingSystemInfo
   {
      private const string WINDOWS_REG_KEY = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion";

      public string ComputerName { get; } = Environment.MachineName;
      public string Architecture => Environment.Is64BitOperatingSystem ? "x64" : "x32";

      private string registryHKLMValue(string path, string key)
      {
         try
         {
            var rk = Registry.LocalMachine.OpenSubKey(path);
            if (rk == null)
               return string.Empty;

            return (string) rk.GetValue(key) ?? string.Empty;
         }
         catch
         {
            return "";
         }
      }

      public string FriendlyName
      {
         get
         {
            var productName = registryHKLMValue(WINDOWS_REG_KEY, "ProductName");
            var version = registryHKLMValue(WINDOWS_REG_KEY, "DisplayVersion");
            var currentBuildNumber = registryHKLMValue(WINDOWS_REG_KEY, "CurrentBuildNumber");

            if (string.IsNullOrEmpty(productName))
               return Environment.OSVersion.VersionString;

            //Windows 11 stores "Windows 10 XXX" as product name in the registry
            //To get the right product name, build version can be used
            productName = fixProductNameForWindows11(productName, currentBuildNumber);

            var info = new[]
            {
               productName,
               version,
               Environment.OSVersion.ServicePack
            };

            return info.Where(s => !string.IsNullOrEmpty(s)).ToString(" - ");
         }
      }

      private string fixProductNameForWindows11(string productName, string currentBuildNumber)
      {
         if (string.IsNullOrEmpty(currentBuildNumber))
            return productName;

         //no adjustment required for the Windows Server editions
         if(productName.ToLower().Contains("server"))
            return productName;

         //for non-server editions: Windows 11 can be detected by the build number >=22000
         // (s. e.g. https://en.wikipedia.org/wiki/Windows_11_version_history#Version_history)
         const int minimalWindows11BuildNumber = 22000;

         if (int.TryParse(currentBuildNumber, out var buildNumber) && buildNumber >= minimalWindows11BuildNumber)
            return productName.Replace("Windows 10", "Windows 11");
         else
            return productName;
      }

      //adapted from http://www.codeproject.com/Messages/1246676/Re-Absolute-excellent-code-but.aspx
      public bool IsRunningOnVirtualMachine
      {
         get
         {
            try
            {
               var wmi = new ManagementObjectSearcher("SELECT Model FROM Win32_ComputerSystem");

               foreach (var mo in wmi.Get().OfType<ManagementObject>())
               {
                  var computerModel = mo["Model"] as string;

                  if (!string.IsNullOrEmpty(computerModel))
                     return computerModel.ToLower().Contains("virtual");
               }

               return false;
            }
            catch
            {
               return false;
            }
         }
      }

      public bool IsRunningOnTerminalSession => SystemInformation.TerminalServerSession;
   }
}