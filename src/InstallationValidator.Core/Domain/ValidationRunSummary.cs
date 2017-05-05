using System;

namespace InstallationValidator.Core.Domain
{
   public class ValidationRunSummary
   {
      public DateTime StartTime { get; set; }
      public DateTime EndTime { get; set; }
      public string PKSimVersion { get; set; }
      public string MoBiVersion { get; set; }
      public string OutputFolder { get; set; }
      public string InputFolder { get; set; }
      public OperatingSystemInfo OperatingSystem { get;  } = new OperatingSystemInfo();
   }
}