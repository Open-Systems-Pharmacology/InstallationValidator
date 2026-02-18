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
         return Task.Run(() =>
         {
            var document = new PdfReportDocument(
               comparisonResult,
               _applicationConfiguration.OSPSuiteNameWithVersion,
               Assets.Reporting.FolderComparison,
               _svgChartGenerator);

            var reportPath = reportOutputPath(secondFolderPath, DateTime.Now);
            document.GeneratePdf(reportPath);

            openReportIfRequired(openReport, reportPath);
         });
      }

      public Task CreateReport(InstallationValidationResult installationValidationResult, string outputFolderPath, bool openReport = false)
      {
         return Task.Run(() =>
         {
            var document = new PdfReportDocument(
               installationValidationResult,
               _applicationConfiguration.OSPSuiteNameWithVersion,
               Assets.Reporting.InstallationValidation,
               _svgChartGenerator);

            var reportPath = reportOutputPath(outputFolderPath, installationValidationResult.RunSummary.StartTime);
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

      private string reportOutputPath(string outputFilePath, DateTime dateTime)
      {
         return Path.Combine(outputFilePath, $"{_applicationConfiguration.OSPSuiteNameWithVersion}-{Assets.Reporting.InstallationValidation}_{dateTime:MM_dd_yy_H_mm_ss}.pdf");
      }
   }
}
