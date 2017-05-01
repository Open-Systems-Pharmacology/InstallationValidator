using System;

namespace OSPSuite.InstallationValidator.Core.Domain
{
   public class InstallationValidationResult
   {
      public BatchRunSummary RunSummary { set; get; }
      public BatchComparisonResult ComparisonResult { set; get; }
   }


   public class BatchRunSummary
   {
      public DateTime StartTime { set; get; }
      public DateTime EndTime { set; get; }
      public string ComputerName { set; get; }
      public string PKSimVersion { set; get; }
      public string MoBiVersion { set; get; }
      public string BatchOutputFolder { set; get; }
      public string ConfigurationInputFolder { set; get; }
      public OperatingSystem OperatingSystem { get; set; }
   }
}