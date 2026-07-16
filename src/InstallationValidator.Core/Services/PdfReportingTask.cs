using System;
using System.IO;
using System.Threading.Tasks;
using InstallationValidator.Core.Assets;
using InstallationValidator.Core.Domain;
using InstallationValidator.Core.Reporting.Charts;
using InstallationValidator.Core.Reporting.Pdf;
using OSPSuite.Utility;
using QuestPDF.Fluent;

namespace InstallationValidator.Core.Services
{
   public interface IPdfReportingTask
   {
      Task CreateReport(InstallationValidationResult installationValidationResult, string outputFolderPath, bool openReport = false);
      Task CreateReport(BatchComparisonResult comparisonResult, string firstFolderPath, string secondFolderPath, bool openReport = false);
   }

   public class PdfReportingTask : IPdfReportingTask
   {
      private readonly IValidationLogger _validationLogger;
      private readonly IInstallationValidatorConfiguration _applicationConfiguration;
      private readonly ISvgChartGenerator _svgChartGenerator;

      public PdfReportingTask(
         IValidationLogger validationLogger,
         IInstallationValidatorConfiguration applicationConfiguration,
         ISvgChartGenerator svgChartGenerator)
      {
         _validationLogger = validationLogger;
         _applicationConfiguration = applicationConfiguration;
         _svgChartGenerator = svgChartGenerator;
      }

      public Task CreateReport(BatchComparisonResult comparisonResult, string firstFolderPath, string secondFolderPath, bool openReport = false)
      {
         if (comparisonResult == null)
            throw new ArgumentNullException(nameof(comparisonResult));

         return Task.Run(() =>
         {
            var document = new PdfReportDocument(
               comparisonResult,
               _applicationConfiguration.OSPSuiteNameWithVersion,
               Assets.Reporting.FolderComparison,
               _svgChartGenerator);

            var reportPath = reportOutputPath(secondFolderPath, DateTime.Now, Assets.Reporting.FolderComparison);
            ensureDirectoryExists(reportPath);
            document.GeneratePdf(reportPath);

            openReportIfRequired(openReport, reportPath);
         });
      }

      public Task CreateReport(InstallationValidationResult installationValidationResult, string outputFolderPath, bool openReport = false)
      {
         if (installationValidationResult == null)
            throw new ArgumentNullException(nameof(installationValidationResult));
         if (installationValidationResult.RunSummary == null)
            throw new ArgumentNullException(nameof(installationValidationResult), "RunSummary cannot be null");

         return Task.Run(() =>
         {
            var document = new PdfReportDocument(
               installationValidationResult,
               _applicationConfiguration.OSPSuiteNameWithVersion,
               Assets.Reporting.InstallationValidation,
               _svgChartGenerator);

            var reportPath = reportOutputPath(outputFolderPath, installationValidationResult.RunSummary.StartTime, Assets.Reporting.InstallationValidation);
            ensureDirectoryExists(reportPath);
            document.GeneratePdf(reportPath);

            openReportIfRequired(openReport, reportPath);
         });
      }

      private void openReportIfRequired(bool openReport, string reportPath)
      {
         if (openReport)
            FileHelper.TryOpenFile(reportPath);

         _validationLogger.AppendLine(Logs.ReportCreatedUnder(reportPath));
      }

      private string reportOutputPath(string outputFilePath, DateTime dateTime, string reportName)
      {
         return Path.Combine(outputFilePath, $"{_applicationConfiguration.OSPSuiteNameWithVersion}-{reportName}_{dateTime:MM_dd_yy_H_mm_ss}.pdf");
      }

      private void ensureDirectoryExists(string filePath)
      {
         var directory = Path.GetDirectoryName(filePath);
         if (!string.IsNullOrEmpty(directory))
            Directory.CreateDirectory(directory);
      }
   }
}
