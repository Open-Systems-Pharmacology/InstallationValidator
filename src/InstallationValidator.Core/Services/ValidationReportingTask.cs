using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using InstallationValidator.Core.Assets;
using InstallationValidator.Core.Domain;
using OSPSuite.Core.Reporting;
using OSPSuite.Core.Services;
using OSPSuite.Utility;

namespace InstallationValidator.Core.Services
{
   public interface IValidationReportingTask
   {
      Task CreateReport(InstallationValidationResult installationValidationResult, string outputFolderPath, bool openReport = false);
      Task CreateReport(BatchComparisonResult comparisonResult, string firstFolderPath, string secondFolderPath, bool openReport);
   }

   public class ValidationReportingTask : IValidationReportingTask
   {
      private readonly IReportTemplateRepository _reportTemplateRepository;
      private readonly IReportingTask _reportingTask;
      private readonly IValidationLogger _validationLogger;
      private readonly IInstallationValidatorConfiguration _applicationConfiguration;

      public ValidationReportingTask(IReportTemplateRepository reportTemplateRepository, IReportingTask reportingTask, IValidationLogger validationLogger, IInstallationValidatorConfiguration applicationConfiguration)
      {
         _reportTemplateRepository = reportTemplateRepository;
         _reportingTask = reportingTask;
         _validationLogger = validationLogger;
         _applicationConfiguration = applicationConfiguration;
      }

      public async Task CreateReport(BatchComparisonResult comparisonResult, string firstFolderPath, string secondFolderPath, bool openReport = false)
      {
         var reportConfiguration = createReportConfiguration(Assets.Reporting.FolderComparison, secondFolderPath, DateTime.Now);

         await startCreationProcess(comparisonResult, reportConfiguration);

         if (openReport)
            FileHelper.TryOpenFile(reportConfiguration.ReportFile);

         _validationLogger.AppendLine(Logs.ReportCreatedUnder(reportConfiguration.ReportFile));
      }

      private ReportConfiguration createReportConfiguration(string reportSubtitle, string outputFilePath, DateTime reportDateAndTime)
      {
         return new ReportConfiguration
         {
            Template = _reportTemplateRepository.All().FirstOrDefault(),
            Title = _applicationConfiguration.OSPSuiteNameWithVersion,
            SubTitle = reportSubtitle,
            ReportFile = reportOutputPath(outputFilePath, reportDateAndTime)
         };
      }

      public async Task CreateReport(InstallationValidationResult installationValidationResult, string outputFolderPath, bool openReport = false)
      {
         var reportConfiguration = createReportConfiguration(Assets.Reporting.InstallationValidation, outputFolderPath, installationValidationResult.RunSummary.StartTime);

         await startCreationProcess(installationValidationResult, reportConfiguration);

         if (openReport)
            FileHelper.TryOpenFile(reportConfiguration.ReportFile);

         _validationLogger.AppendLine(Logs.ReportCreatedUnder(reportConfiguration.ReportFile));
      }

      private Task startCreationProcess(IWithValidationState batchComparisonResult, ReportConfiguration reportConfiguration)
      {
         return _reportingTask.CreateReportAsync(batchComparisonResult, reportConfiguration);
      }

      private string reportOutputPath(string outputFilePath, DateTime dateTime)
      {
         return Path.Combine($"{outputFilePath}", $"{_applicationConfiguration.OSPSuiteNameWithVersion}-{Assets.Reporting.InstallationValidation}_{dateTime:MM_dd_yy_H_mm_ss}.pdf");
      }
   }
}