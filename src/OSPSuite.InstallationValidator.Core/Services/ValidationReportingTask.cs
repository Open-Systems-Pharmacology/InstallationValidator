using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OSPSuite.Core.Reporting;
using OSPSuite.Core.Services;
using OSPSuite.InstallationValidator.Core.Assets;
using OSPSuite.InstallationValidator.Core.Domain;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.InstallationValidator.Core.Services
{
   public interface IValidationReportingTask
   {
      Task CreateReport(InstallationValidationResult installationValidationResult, string outputFolderPath, bool openReport = false);
   }

   public class ValidationReportingTask : IValidationReportingTask
   {
      private readonly IReportTemplateRepository _reportTemplateRepository;
      private readonly IReportingTask _reportingTask;
      private readonly IValidationLogger _validationLogger;

      public ValidationReportingTask(IReportTemplateRepository reportTemplateRepository, IReportingTask reportingTask, IValidationLogger validationLogger)
      {
         _reportTemplateRepository = reportTemplateRepository;
         _reportingTask = reportingTask;
         _validationLogger = validationLogger;
      }

      public async Task CreateReport(InstallationValidationResult installationValidationResult, string outputFolderPath, bool openReport = false)
      {
         var dateTime = installationValidationResult.RunSummary.StartTime;

         var reportConfiguration = new ReportConfiguration
         {
            Template = _reportTemplateRepository.All().FirstOrDefault(),
            Title = Assets.Reporting.ValidationReport,
            SubTitle = dateTime.ToIsoFormat(withTime: true, withSeconds: true),
            ReportFile = reportOutputPath(outputFolderPath, dateTime)
         };

         await startCreationProcess(installationValidationResult, reportConfiguration);

         if (openReport)
            FileHelper.TryOpenFile(reportConfiguration.ReportFile);

         _validationLogger.AppendLine(Captions.ReportCreatedUnder(reportConfiguration.ReportFile));
      }

      private Task startCreationProcess(InstallationValidationResult batchComparisonResult, ReportConfiguration reportConfiguration)
      {
         return _reportingTask.CreateReportAsync(batchComparisonResult, reportConfiguration);
      }

      private string reportOutputPath(string outputFilePath, DateTime dateTime)
      {
         return Path.Combine($"{outputFilePath}", $"{Assets.Reporting.ValidationReport}_{dateTime:MM_dd_yy_H_mm_ss}.pdf");
      }
   }
}