using System;
using System.Collections.Generic;
using Microsoft.Win32;
using OSPSuite.Assets;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Infrastructure.Configuration;

namespace OSPSuite.InstallationValidator.Core
{
   public interface IInstallationValidationConfiguration : IApplicationConfiguration
   {
      string PKSimPath { get; }
   }

   public class ApplicationConfiguration : OSPSuiteConfiguration, IInstallationValidationConfiguration
   {
      protected override string ApplicationFolderPathWithRevision(string revision)
      {
         throw new NotImplementedException();
      }

      protected override string[] LatestVersionWithOtherMajor { get; }
      public override string ChartLayoutTemplateFolderPath { get; }
      public override string TEXTemplateFolderPath { get; }
      public override string ProductName { get; }
      public override Origin Product { get; }
      public override string ProductNameWithTrademark { get; }
      public override ApplicationIcon Icon { get; } = ApplicationIcons.Comparison;
      public override string UserSettingsFileName { get; }
      public override string IssueTrackerUrl => "https://github.com/Open-Systems-Pharmacology/OSPSuite.InstallationValidator/issues";

      // TODO - move to OSPSuite.Core
      public string PKSimPath
      {
         get
         {
            try
            {
               return (string)Registry.GetValue($@"HKEY_LOCAL_MACHINE\SOFTWARE\{Assets.Constants.RegistryPaths.PKSimRegPath}{MajorVersion}", "InstallPath", null);
            }
            catch (Exception)
            {
               return string.Empty;
            }
         }
      }
   }
}