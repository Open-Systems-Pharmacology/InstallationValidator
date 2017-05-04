using System;
using OSPSuite.Core.Domain;

namespace InstallationValidator.Core.Domain
{
   public class InstallationValidationResult : IWithValidationState
   {
      public BatchRunSummary RunSummary { set; get; }
      public BatchComparisonResult ComparisonResult { set; get; }

      public ValidationState State => ComparisonResult?.State ?? ValidationState.Invalid;
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