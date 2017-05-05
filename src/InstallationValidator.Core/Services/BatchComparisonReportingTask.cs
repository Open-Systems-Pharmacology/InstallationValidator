using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using InstallationValidator.Core.Assets;
using InstallationValidator.Core.Domain;
using OSPSuite.Core;
using OSPSuite.Core.Reporting;
using OSPSuite.Core.Services;
using OSPSuite.Utility;

namespace InstallationValidator.Core.Services
{
   public interface IBatchComparisonReportingTask
   {
      Task CreateReport(BatchComparisonResult comparisonResult, string firstFolderPath, string secondFolderPath, bool openReport);
   }

   public class BatchComparisonReportingTask : IBatchComparisonReportingTask
   {
      private readonly IReportTemplateRepository _reportTemplateRepository;
      private readonly IReportingTask _reportingTask;
      private readonly IValidationLogger _validationLogger;
      private readonly IApplicationConfiguration _applicationConfiguration;

      public BatchComparisonReportingTask(IReportTemplateRepository reportTemplateRepository, IReportingTask reportingTask, IValidationLogger validationLogger, IApplicationConfiguration applicationConfiguration)
      {
         _reportTemplateRepository = reportTemplateRepository;
         _reportingTask = reportingTask;
         _validationLogger = validationLogger;
         _applicationConfiguration = applicationConfiguration;
      }

      public async Task CreateReport(BatchComparisonResult comparisonResult, string firstFolderPath, string secondFolderPath, bool openReport = false)
      {
         var reportConfiguration = new ReportConfiguration
         {
            Template = _reportTemplateRepository.All().FirstOrDefault(),
            Title = _applicationConfiguration.OSPSuiteNameWithVersion,
            SubTitle = Assets.Reporting.FolderComparison,
            ReportFile = reportOutputPath(firstFolderPath, DateTime.Now)
         };

         await createReport(comparisonResult, reportConfiguration);

         if (openReport)
            FileHelper.TryOpenFile(reportConfiguration.ReportFile);

         _validationLogger.AppendLine(Logs.ReportCreatedUnder(reportConfiguration.ReportFile));
      }

      private Task createReport(BatchComparisonResult comparisonResult, ReportConfiguration reportConfiguration)
      {
         return _reportingTask.CreateReportAsync(comparisonResult, reportConfiguration);
      }

      private string reportOutputPath(string outputFilePath, DateTime dateTime)
      {
         return Path.Combine($"{outputFilePath}", $"{_applicationConfiguration.OSPSuiteNameWithVersion}-{Assets.Reporting.InstallationValidation}_{dateTime:MM_dd_yy_H_mm_ss}.pdf");
      }
   }
}