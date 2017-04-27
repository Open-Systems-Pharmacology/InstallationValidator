using System;
using System.IO;
using Microsoft.Win32;
using OSPSuite.Assets;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Infrastructure.Configuration;
using OSPSuite.Utility;
using RegistryPaths = OSPSuite.Core.Domain.Constants.RegistryPaths;

namespace OSPSuite.InstallationValidator.Core
{
   public interface IInstallationValidatorConfiguration : IApplicationConfiguration
   {
      string PKSimInstallFolderPath { get; }
      string BatchInputsFolderPath { get; }
      string BatchOutputsFolderPath { get; }
      string PKSimBatchToolPath { get; }
   }

   public class InstallationValidatorConfiguration : OSPSuiteConfiguration, IInstallationValidatorConfiguration
   {
      protected override string ApplicationFolderPathWithRevision(string revision)
      {
         return Path.Combine(Constants.APPLICATION_FOLDER_PATH, revision);
      }

      protected override string[] LatestVersionWithOtherMajor { get; }
      public override string ChartLayoutTemplateFolderPath { get; }
      public override string TEXTemplateFolderPath { get; }
      public override string ProductName { get; }
      public override Origin Product { get; }
      public override string ProductNameWithTrademark { get; }
      public override ApplicationIcon Icon { get; } = ApplicationIcons.Comparison;
      public override string UserSettingsFileName { get; }
      public override string IssueTrackerUrl { get; } = Constants.ISSUE_TRACKER_URL;
      private string applicationFolderPathWithMajorVersion => ApplicationFolderPathWithRevision(MajorVersion);
      private string applicationSettingsFolderPath => applicationSettingsFolderPathFor(applicationFolderPathWithMajorVersion);

      private string applicationSettingsFolderPathFor(string applicationFolderPath)
      {
         return Path.Combine(EnvironmentHelper.ApplicationDataFolder(), applicationFolderPath);
      }

      // TODO - move to OSPSuite.Core
      public string PKSimInstallFolderPath
      {
         get
         {
            try
            {
               return (string) Registry.GetValue($@"HKEY_LOCAL_MACHINE\SOFTWARE\{RegistryPaths.PKSIM_REG_PATH}{MajorVersion}", RegistryPaths.INSTALL_DIR, null);
            }
            catch (Exception)
            {
               return string.Empty;
            }
         }
      }

      public string BatchInputsFolderPath => Path.Combine(applicationSettingsFolderPath, Constants.Tools.BATCH_INPUTS);
      public string BatchOutputsFolderPath => Path.Combine(applicationSettingsFolderPath, Constants.Tools.BATCH_OUTPUTS);
      public string PKSimBatchToolPath => Path.Combine(PKSimInstallFolderPath, Constants.Tools.PKSIM_BATCH_TOOL);
   }
}