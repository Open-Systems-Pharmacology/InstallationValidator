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

            if (string.IsNullOrEmpty(productName))
               return Environment.OSVersion.VersionString;

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