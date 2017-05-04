using System;
using InstallationValidator.Core.Domain;
using OSPSuite.Core.Extensions;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.TeXReporting.Items;
using OSPSuite.Utility.Extensions;

namespace InstallationValidator.Core.Reporting
{
   public class ValidationRunSummaryTeXBuilder : OSPSuiteTeXBuilder<ValidationRunSummary>
   {
      private readonly ITeXBuilderRepository _builderRepository;

      public ValidationRunSummaryTeXBuilder(ITeXBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(ValidationRunSummary validationRunSummary, OSPSuiteTracker buildTracker)
      {
         var report = new object[]
         {
            new Section(Assets.Reporting.ValidationSummary),
            new Paragraph(Assets.Reporting.BatchRunDuration),
            durationFor(validationRunSummary),
            new Paragraph(Assets.Reporting.InputConfigurationFolder),
            validationRunSummary.InputFolder,
            new Paragraph(Assets.Reporting.BatchOutputFolder),
            validationRunSummary.OutputFolder,
            new Paragraph(Assets.Reporting.ApplicationVersions),
            applicationVersions(validationRunSummary),
            validationRunSummary.OperatingSystem
         };

         _builderRepository.Report(report, buildTracker);
      }

      private string durationFor(ValidationRunSummary validationRunSummary)
      {
         var timeSpent = validationRunSummary.EndTime - validationRunSummary.StartTime;
         return Assets.Reporting.InstallationValidationPerformedIn(validationRunSummary.StartTime.ToIsoFormat(), validationRunSummary.EndTime.ToIsoFormat(), timeSpent.ToDisplay());
      }

      private string applicationVersions(ValidationRunSummary installationValidationSummary)
      {
         return $"PK-Sim Version {installationValidationSummary.PKSimVersion}{Environment.NewLine}MoBi Version {installationValidationSummary.MoBiVersion}";
      }

      
   }
}