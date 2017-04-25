using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Core;
using OSPSuite.Core.Domain;

namespace OSPSuite.InstallationValidator.Core
{
   public class ApplicationConfiguration : IApplicationConfiguration
   {
      public string ChartLayoutTemplateFolderPath { get; }
      public string TEXTemplateFolderPath { get; }
      public string PKParametersFilePath { get; set; }
      public string LicenseAgreementFilePath { get; }
      public string FullVersion { get; }
      public string Version { get; }
      public string MajorVersion { get; }
      public string BuildVersion { get; }
      public string ProductName { get; }
      public Origin Product { get; }
      public string ProductNameWithTrademark { get; }
      public ApplicationIcon Icon { get; } = ApplicationIcons.ConfigureAndRun;
      public IEnumerable<string> UserApplicationSettingsFilePaths { get; } = new List<string>();
      public string IssueTrackerUrl { get; }
   }
}