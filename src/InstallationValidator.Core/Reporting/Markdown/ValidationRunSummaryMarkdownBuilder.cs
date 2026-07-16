using System;
using InstallationValidator.Core.Domain;

namespace InstallationValidator.Core.Reporting.Markdown
{
   public class ValidationRunSummaryMarkdownBuilder : MarkdownBuilder<ValidationRunSummary>
   {
      private readonly IMarkdownBuilderRepository _builderRepository;

      public ValidationRunSummaryMarkdownBuilder(IMarkdownBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(ValidationRunSummary validationRunSummary, MarkdownReportContext context)
      {
         context.AppendHeading(Assets.Reporting.ValidationSummary, 2);

         context.AppendHeading(Assets.Reporting.BatchRunDuration, 3);
         context.AppendParagraph(durationFor(validationRunSummary));

         context.AppendHeading(Assets.Reporting.InputConfigurationFolder, 3);
         context.AppendParagraph(validationRunSummary.InputFolder);

         context.AppendHeading(Assets.Reporting.BatchOutputFolder, 3);
         context.AppendParagraph(validationRunSummary.OutputFolder);

         context.AppendHeading(Assets.Reporting.ApplicationVersions, 3);
         context.AppendParagraph(applicationVersions(validationRunSummary));

         context.AppendHeading(Assets.Reporting.LanguageSettings, 3);
         context.AppendParagraph(languageSettings(validationRunSummary));

         _builderRepository.Report(validationRunSummary.OperatingSystem, context);
      }

      private string languageSettings(ValidationRunSummary validationRunSummary) =>
         $"{validationRunSummary.CultureInfo.EnglishName} ({validationRunSummary.CultureInfo.Name})";

      private string durationFor(ValidationRunSummary validationRunSummary)
      {
         var timeSpent = validationRunSummary.EndTime - validationRunSummary.StartTime;
         return Assets.Reporting.InstallationValidationPerformedIn(
            validationRunSummary.StartTime.ToString("yyyy-MM-dd HH:mm:ss"),
            validationRunSummary.EndTime.ToString("yyyy-MM-dd HH:mm:ss"),
            formatTimeSpan(timeSpent));
      }

      private string formatTimeSpan(TimeSpan timeSpan)
      {
         if (timeSpan.TotalHours >= 1)
            return $"{(int)timeSpan.TotalHours}h {timeSpan.Minutes}min {timeSpan.Seconds}s";
         if (timeSpan.TotalMinutes >= 1)
            return $"{timeSpan.Minutes}min {timeSpan.Seconds}s";
         return $"{timeSpan.Seconds}s";
      }

      private string applicationVersions(ValidationRunSummary installationValidationSummary)
      {
         return $"PK-Sim Version {installationValidationSummary.PKSimVersion}  {Environment.NewLine}MoBi Version {installationValidationSummary.MoBiVersion}";
      }
   }
}
