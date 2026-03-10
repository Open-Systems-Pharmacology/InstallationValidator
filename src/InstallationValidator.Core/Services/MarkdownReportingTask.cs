using System;
using System.IO;
using System.Threading.Tasks;
using InstallationValidator.Core.Assets;
using InstallationValidator.Core.Domain;
using InstallationValidator.Core.Reporting.Markdown;
using OSPSuite.Utility;

namespace InstallationValidator.Core.Services
{
   public interface IMarkdownReportingTask
   {
      Task CreateReport(InstallationValidationResult installationValidationResult, string outputFolderPath, bool openReport = false);
      Task CreateReport(BatchComparisonResult comparisonResult, string firstFolderPath, string secondFolderPath, bool openReport = false);
   }

   public class MarkdownReportingTask : IMarkdownReportingTask
   {
      private readonly IMarkdownBuilderRepository _builderRepository;
      private readonly IValidationLogger _validationLogger;
      private readonly IInstallationValidatorConfiguration _applicationConfiguration;

      public MarkdownReportingTask(
         IMarkdownBuilderRepository builderRepository,
         IValidationLogger validationLogger,
         IInstallationValidatorConfiguration applicationConfiguration)
      {
         _builderRepository = builderRepository;
         _validationLogger = validationLogger;
         _applicationConfiguration = applicationConfiguration;
      }

      public async Task CreateReport(BatchComparisonResult comparisonResult, string firstFolderPath, string secondFolderPath, bool openReport = false)
      {
         var context = createReportContext(Assets.Reporting.FolderComparison);

         _builderRepository.Report(comparisonResult, context);

         var reportPath = reportOutputPath(secondFolderPath, DateTime.Now, Assets.Reporting.FolderComparison);
         await writeReportAsync(reportPath, context);

         openReportIfRequired(openReport, reportPath);
      }

      public async Task CreateReport(InstallationValidationResult installationValidationResult, string outputFolderPath, bool openReport = false)
      {
         if (installationValidationResult == null)
            throw new ArgumentNullException(nameof(installationValidationResult));
         if (installationValidationResult.RunSummary == null)
            throw new ArgumentNullException(nameof(installationValidationResult), "RunSummary cannot be null");

         var context = createReportContext(Assets.Reporting.InstallationValidation);

         _builderRepository.Report(installationValidationResult, context);

         var reportPath = reportOutputPath(outputFolderPath, installationValidationResult.RunSummary.StartTime, Assets.Reporting.InstallationValidation);
         await writeReportAsync(reportPath, context);

         openReportIfRequired(openReport, reportPath);
      }

      private MarkdownReportContext createReportContext(string reportSubtitle)
      {
         var context = new MarkdownReportContext();
         context.AppendHeading($"{_applicationConfiguration.OSPSuiteNameWithVersion} - {reportSubtitle}", 1);
         context.AppendHorizontalRule();
         return context;
      }

      private async Task writeReportAsync(string reportPath, MarkdownReportContext context)
      {
         var directory = Path.GetDirectoryName(reportPath);
         if (!string.IsNullOrEmpty(directory))
            Directory.CreateDirectory(directory);

         using (var writer = new StreamWriter(reportPath, false, System.Text.Encoding.UTF8))
         {
            await writer.WriteAsync(context.ToString());
         }
      }

      private void openReportIfRequired(bool openReport, string reportPath)
      {
         if (openReport)
            FileHelper.TryOpenFile(reportPath);

         _validationLogger.AppendLine(Logs.ReportCreatedUnder(reportPath));
      }

      private string reportOutputPath(string outputFilePath, DateTime dateTime, string reportName)
      {
         return Path.Combine(outputFilePath, $"{_applicationConfiguration.OSPSuiteNameWithVersion}-{reportName}_{dateTime:MM_dd_yy_H_mm_ss}.md");
      }
   }
}
