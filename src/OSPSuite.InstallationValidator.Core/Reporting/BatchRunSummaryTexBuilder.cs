using System;
using System.Collections.Generic;
using System.Globalization;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.InstallationValidator.Core.Assets;
using OSPSuite.InstallationValidator.Core.Domain;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.TeXReporting.Items;

namespace OSPSuite.InstallationValidator.Core.Reporting
{
   public class BatchRunSummaryTexBuilder : OSPSuiteTeXBuilder<BatchRunSummary>
   {
      private readonly ITeXBuilderRepository _builderRepository;

      public BatchRunSummaryTexBuilder(ITeXBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(BatchRunSummary batchRunSummary, OSPSuiteTracker buildTracker)
      {
         var report = new List<object>();

         report.AddRange(new List<object> { new Section(Assets.Reporting.RunSummary),
            new SubSection(Assets.Reporting.BatchRunDuration), durationFor(batchRunSummary),
            new SubSection(Assets.Reporting.InputConfigurationFolder), inputFolderFor(batchRunSummary),
            new SubSection(Assets.Reporting.BatchOutputFolder), outputCalculatedFolderLocation(batchRunSummary),
            new SubSection(Assets.Reporting.ComputerName), computerName(batchRunSummary),
            new SubSection(Assets.Reporting.OperatingSystem), operatingSystem(batchRunSummary),
            new SubSection(Assets.Reporting.ApplicationVersions), applicationVersions(batchRunSummary)});

         _builderRepository.Report(report, buildTracker);
      }

      private object inputFolderFor(BatchRunSummary batchRunSummary)
      {
         return batchRunSummary.ConfigurationInputFolder;
      }

      private object durationFor(BatchRunSummary batchRunSummary)
      {
         return $"{convertDateTimeToString(batchRunSummary.StartTime)} to {convertDateTimeToString(batchRunSummary.EndTime)}";
      }

      private static string convertDateTimeToString(DateTime startTime)
      {
         return startTime.ToLocalTime().ToString(CultureInfo.InvariantCulture);
      }

      private object applicationVersions(BatchRunSummary installationValidationSummary)
      {
         return $"PKSim Version {installationValidationSummary.PKSimVersion}{Environment.NewLine}MoBi Version {installationValidationSummary.MoBiVersion}";
      }

      private object outputCalculatedFolderLocation(BatchRunSummary installationValidationSummary)
      {
         return installationValidationSummary.BatchOutputFolder;
      }

      private object operatingSystem(BatchRunSummary installationValidationSummary)
      {
         return installationValidationSummary.OperatingSystem;
      }

      private object computerName(BatchRunSummary installationValidationSummary)
      {
         return installationValidationSummary.ComputerName;
      }
   }
}