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
            var version = registryHKLMValue(WINDOWS_REG_KEY, "CSDVersion");
            var currentBuildNumber = registryHKLMValue(WINDOWS_REG_KEY, "CurrentBuildNumber");
            var displayVersion = registryHKLMValue(WINDOWS_REG_KEY, "DisplayVersion");

            if (string.IsNullOrEmpty(productName))
               return Environment.OSVersion.VersionString;

            // Windows 11 detection: Build number 22000 and above indicates Windows 11
            // But only apply this to client Windows, not Windows Server editions
            if (!string.IsNullOrEmpty(currentBuildNumber) && int.TryParse(currentBuildNumber, out var buildNumber) && buildNumber >= 22000 
                && !productName.ToLower().Contains("server"))
            {
               var windows11Name = "Windows 11";
               
               // Use DisplayVersion for Windows 11 version info if available
               if (!string.IsNullOrEmpty(displayVersion))
               {
                  windows11Name += $" {displayVersion}";
               }
               
               return windows11Name;
            }

            var info = new[]
            {
               productName,
               version,
               Environment.OSVersion.ServicePack
            };

            return info.Where(s => !string.IsNullOrEmpty(s)).ToString(" - ");
         }
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