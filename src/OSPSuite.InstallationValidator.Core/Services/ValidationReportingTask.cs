using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OSPSuite.Core.Reporting;
using OSPSuite.Core.Services;
using OSPSuite.InstallationValidator.Core.Domain;

namespace OSPSuite.InstallationValidator.Core.Services
{
   public interface IValidationReportingTask
   {
      Task StartReport(InstallationValidationResult installationValidationResult, string outputFolderPath);
   }

   public class ValidationReportingTask : IValidationReportingTask
   {
      private readonly IReportTemplateRepository _reportTemplateRepository;
      private readonly IReportingTask _reportingTask;
      private DateTime _dateTime;

      public ValidationReportingTask(IReportTemplateRepository reportTemplateRepository, IReportingTask reportingTask)
      {
         _reportTemplateRepository = reportTemplateRepository;
         _reportingTask = reportingTask;
      }

      public Task StartReport(InstallationValidationResult installationValidationResult, string outputFolderPath)
      {
         _dateTime = installationValidationResult.RunSummary.StartTime;

         var reportConfiguration = new ReportConfiguration
         {
            Template = _reportTemplateRepository.All().FirstOrDefault(),
            Title = Assets.Reporting.ValidationReport,
            SubTitle = dateAndTime(),
            ReportFile = reportOutputPath(outputFolderPath)
         };

         return startCreationProcess(installationValidationResult, reportConfiguration);
      }

      private Task startCreationProcess(InstallationValidationResult batchComparisonResult, ReportConfiguration reportConfiguration)
      {
         return _reportingTask.CreateReportAsync(batchComparisonResult, reportConfiguration);
      }

      private string reportOutputPath(string outputFilePath)
      {
         return Path.Combine($"{outputFilePath}", $"{Assets.Reporting.ValidationReport}_{_dateTime:MM_dd_yy_H_mm_ss}.pdf");
      }

      private string dateAndTime()
      {
         return $"{_dateTime:MM dd yy @ H:mm:ss}";
      }
   }
}