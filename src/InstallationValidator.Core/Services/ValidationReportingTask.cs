using System;
using System.Threading.Tasks;
using InstallationValidator.Core.Domain;

namespace InstallationValidator.Core.Services
{
   [Flags]
   public enum ReportFormat
   {
      None = 0,
      Markdown = 1,
      Pdf = 2,
      All = Markdown | Pdf
   }

   public class ReportOptions
   {
      public ReportFormat Format { get; }
      public bool OpenReport { get; }

      public bool ExportToMarkdown => Format.HasFlag(ReportFormat.Markdown);
      public bool ExportToPdf => Format.HasFlag(ReportFormat.Pdf);

      public ReportOptions(ReportFormat format, bool openReport)
      {
         Format = format;
         OpenReport = openReport;
      }
   }

   public interface IValidationReportingTask
   {
      Task CreateReport(InstallationValidationResult installationValidationResult, string outputFolderPath, ReportOptions options);
      Task CreateReport(BatchComparisonResult comparisonResult, string firstFolderPath, string secondFolderPath, ReportOptions options);
   }

   public class ValidationReportingTask : IValidationReportingTask
   {
      private readonly IMarkdownReportingTask _markdownReportingTask;
      private readonly IPdfReportingTask _pdfReportingTask;

      public ValidationReportingTask(IMarkdownReportingTask markdownReportingTask, IPdfReportingTask pdfReportingTask)
      {
         _markdownReportingTask = markdownReportingTask;
         _pdfReportingTask = pdfReportingTask;
      }

      public async Task CreateReport(BatchComparisonResult comparisonResult, string firstFolderPath, string secondFolderPath, ReportOptions options)
      {
         if (options.ExportToMarkdown)
         {
            await _markdownReportingTask.CreateReport(comparisonResult, firstFolderPath, secondFolderPath, options.OpenReport);
         }

         if (options.ExportToPdf)
         {
            await _pdfReportingTask.CreateReport(comparisonResult, firstFolderPath, secondFolderPath, options.OpenReport);
         }
      }

      public async Task CreateReport(InstallationValidationResult installationValidationResult, string outputFolderPath, ReportOptions options)
      {
         if (options.ExportToMarkdown)
         {
            await _markdownReportingTask.CreateReport(installationValidationResult, outputFolderPath, options.OpenReport);
         }

         if (options.ExportToPdf)
         {
            await _pdfReportingTask.CreateReport(installationValidationResult, outputFolderPath, options.OpenReport);
         }
      }
   }
}
