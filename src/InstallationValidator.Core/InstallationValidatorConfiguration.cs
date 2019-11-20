using System;
using System.IO;
using Microsoft.Win32;
using OSPSuite.Assets;
using OSPSuite.Core;
using OSPSuite.Core.Domain;

namespace InstallationValidator.Core
{
   public interface IInstallationValidatorConfiguration : IApplicationConfiguration
   {
      string BatchInputsFolderPath { get; }
      string BatchOutputsFolderPath { get; }
      string PKSimCLIPath { get; }
      string PKSimBinaryExecutablePath { get; }
      string MoBiBinaryExecutablePath { get; }
      string DefaultOutputPath { get; }
   }

   public class InstallationValidatorConfiguration : OSPSuiteConfiguration, IInstallationValidatorConfiguration
   {
      public override string ProductName => Constants.PRODUCT_NAME_WITH_TRADEMARK;
      //not used in this context
      public override int InternalVersion { get; } = 1;
      public override Origin Product { get; } = Origins.Other;
      public override string ProductNameWithTrademark => Constants.PRODUCT_NAME_WITH_TRADEMARK;
      public override string IconName { get; } = ApplicationIcons.Comparison.IconName;
      public override string UserSettingsFileName { get; } = "UserSettings.xml";
      public override string ApplicationSettingsFileName { get; } = "ApplicationSettings.xml";
      public override string IssueTrackerUrl { get; } = Constants.ISSUE_TRACKER_URL;
      protected override string[] LatestVersionWithOtherMajor { get; } = new String [0];
      public override string WatermarkOptionLocation { get; } = "Options -> Settings -> Application";
      public override string ApplicationFolderPathName { get; } = Constants.APPLICATION_FOLDER_PATH;

      public string PKSimBinaryExecutablePath => getRegistryValueForRegistryPathAndKey(OSPSuite.Core.Domain.Constants.RegistryPaths.PKSIM_REG_PATH, OSPSuite.Core.Domain.Constants.RegistryPaths.INSTALL_PATH);

      public string MoBiBinaryExecutablePath => getRegistryValueForRegistryPathAndKey(OSPSuite.Core.Domain.Constants.RegistryPaths.MOBI_REG_PATH, OSPSuite.Core.Domain.Constants.RegistryPaths.INSTALL_PATH);

      public string PKSimInstallFolderPath => getRegistryValueForRegistryPathAndKey(OSPSuite.Core.Domain.Constants.RegistryPaths.PKSIM_REG_PATH, OSPSuite.Core.Domain.Constants.RegistryPaths.INSTALL_DIR);

      public string BatchInputsFolderPath => AllUsersFile(Constants.Tools.BATCH_INPUTS);

      public string BatchOutputsFolderPath => AllUsersFile(Constants.Tools.BATCH_OUTPUTS);

      public string PKSimCLIPath => Path.Combine(PKSimInstallFolderPath, Constants.Tools.PKSIM_CLI);

      private string getRegistryValueForRegistryPathAndKey(string openSystemsPharmacology, string installDir)
      {
         try
         {
            return (string) Registry.GetValue($@"HKEY_LOCAL_MACHINE\SOFTWARE\{openSystemsPharmacology}{Version}", installDir, null);
         }
         catch (Exception)
         {
            return string.Empty;
         }
      }

      public string DefaultOutputPath => Path.Combine(CurrentUserFolderPath, Constants.Tools.CALCULATION_OUTPUTS);
   }
}