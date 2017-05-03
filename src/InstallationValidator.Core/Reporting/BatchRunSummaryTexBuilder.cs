using System;
using System.Collections.Generic;
using InstallationValidator.Core.Domain;
using OSPSuite.Core.Extensions;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.TeXReporting.Items;
using OSPSuite.Utility.Extensions;

namespace InstallationValidator.Core.Reporting
{
   public class BatchRunSummaryTeXBuilder : OSPSuiteTeXBuilder<BatchRunSummary>
   {
      private readonly ITeXBuilderRepository _builderRepository;

      public BatchRunSummaryTeXBuilder(ITeXBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(BatchRunSummary batchRunSummary, OSPSuiteTracker buildTracker)
      {
         var report = new List<object>();

         report.AddRange(new List<object>
         {
            new Section(Assets.Reporting.RunSummary),
            new Paragraph(Assets.Reporting.BatchRunDuration),
            durationFor(batchRunSummary),
            new Paragraph(Assets.Reporting.InputConfigurationFolder),
            inputFolderFor(batchRunSummary),
            new Paragraph(Assets.Reporting.BatchOutputFolder),
            outputCalculatedFolderLocation(batchRunSummary),
            new Paragraph(Assets.Reporting.ComputerName),
            computerName(batchRunSummary),
            new Paragraph(Assets.Reporting.OperatingSystem),
            operatingSystem(batchRunSummary),
            new Paragraph(Assets.Reporting.ApplicationVersions),
            applicationVersions(batchRunSummary)
         });

         _builderRepository.Report(report, buildTracker);
      }

      private string inputFolderFor(BatchRunSummary batchRunSummary)
      {
         return batchRunSummary.ConfigurationInputFolder;
      }

      private string durationFor(BatchRunSummary batchRunSummary)
      {
         var timeSpent = batchRunSummary.EndTime - batchRunSummary.StartTime;
         return Assets.Reporting.InstallationValidationPerformedIn(batchRunSummary.StartTime.ToIsoFormat(), batchRunSummary.EndTime.ToIsoFormat(), timeSpent.ToDisplay());
      }

      private string applicationVersions(BatchRunSummary installationValidationSummary)
      {
         return $"PK-Sim Version {installationValidationSummary.PKSimVersion}{Environment.NewLine}MoBi Version {installationValidationSummary.MoBiVersion}";
      }

      private string outputCalculatedFolderLocation(BatchRunSummary installationValidationSummary)
      {
         return installationValidationSummary.BatchOutputFolder;
      }

      private OperatingSystem operatingSystem(BatchRunSummary installationValidationSummary)
      {
         return installationValidationSummary.OperatingSystem;
      }

      private string computerName(BatchRunSummary installationValidationSummary)
      {
         return installationValidationSummary.ComputerName;
      }
   }
}