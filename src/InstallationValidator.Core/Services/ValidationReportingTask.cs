using System.Threading.Tasks;
using InstallationValidator.Core.Domain;

namespace InstallationValidator.Core.Services
{
   public interface IValidationReportingTask
   {
      Task CreateReport(InstallationValidationResult installationValidationResult, string outputFolderPath, bool openReport = false);
      Task CreateReport(BatchComparisonResult comparisonResult, string firstFolderPath, string secondFolderPath, bool openReport);
   }

   public enum ReportFormat
   {
      Markdown,
      Pdf
   }

   public class ValidationReportingTask : IValidationReportingTask
   {
      private readonly IMarkdownReportingTask _markdownReportingTask;
      private readonly IPdfReportingTask _pdfReportingTask;

      public ReportFormat DefaultFormat { get; set; } = ReportFormat.Markdown;

      public ValidationReportingTask(IMarkdownReportingTask markdownReportingTask, IPdfReportingTask pdfReportingTask)
      {
         _markdownReportingTask = markdownReportingTask;
         _pdfReportingTask = pdfReportingTask;
      }

      public async Task CreateReport(BatchComparisonResult comparisonResult, string firstFolderPath, string secondFolderPath, bool openReport = false)
      {
         if (DefaultFormat == ReportFormat.Pdf)
         {
            await _pdfReportingTask.CreateReport(comparisonResult, firstFolderPath, secondFolderPath, openReport);
         }
         else
         {
            await _markdownReportingTask.CreateReport(comparisonResult, firstFolderPath, secondFolderPath, openReport);
         }
      }

      public async Task CreateReport(InstallationValidationResult installationValidationResult, string outputFolderPath, bool openReport = false)
      {
         if (DefaultFormat == ReportFormat.Pdf)
         {
            await _pdfReportingTask.CreateReport(installationValidationResult, outputFolderPath, openReport);
         }
         else
         {
            await _markdownReportingTask.CreateReport(installationValidationResult, outputFolderPath, openReport);
         }
      }
   }
}
