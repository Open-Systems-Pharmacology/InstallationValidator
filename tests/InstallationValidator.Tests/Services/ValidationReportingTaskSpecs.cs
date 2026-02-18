using FakeItEasy;
using InstallationValidator.Core.Domain;
using InstallationValidator.Core.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility;

namespace InstallationValidator.Services
{
   public abstract class concern_for_ValidationReportingTask : ContextSpecification<ValidationReportingTask>
   {
      protected IMarkdownReportingTask _markdownReportingTask;
      protected IPdfReportingTask _pdfReportingTask;
      protected InstallationValidationResult _installationValidationResult;
      protected string _outputFolder = "TestOutput";

      protected override void Context()
      {
         _markdownReportingTask = A.Fake<IMarkdownReportingTask>();
         _pdfReportingTask = A.Fake<IPdfReportingTask>();

         sut = new ValidationReportingTask(_markdownReportingTask, _pdfReportingTask);

         _installationValidationResult = new InstallationValidationResult
         {
            RunSummary = new ValidationRunSummary(),
            ComparisonResult = new BatchComparisonResult()
         };
      }
   }

   public class When_creating_report_with_default_markdown_format : concern_for_ValidationReportingTask
   {
      protected override void Context()
      {
         base.Context();
         sut.DefaultFormat = ReportFormat.Markdown;
      }

      protected override void Because()
      {
         sut.CreateReport(_installationValidationResult, _outputFolder).Wait();
      }

      [Observation]
      public void should_use_markdown_reporting_task()
      {
         A.CallTo(() => _markdownReportingTask.CreateReport(_installationValidationResult, _outputFolder, false))
            .MustHaveHappened();
      }

      [Observation]
      public void should_not_use_pdf_reporting_task()
      {
         A.CallTo(() => _pdfReportingTask.CreateReport(_installationValidationResult, _outputFolder, A<bool>._))
            .MustNotHaveHappened();
      }
   }

   public class When_creating_report_with_pdf_format : concern_for_ValidationReportingTask
   {
      protected override void Context()
      {
         base.Context();
         sut.DefaultFormat = ReportFormat.Pdf;
      }

      protected override void Because()
      {
         sut.CreateReport(_installationValidationResult, _outputFolder).Wait();
      }

      [Observation]
      public void should_use_pdf_reporting_task()
      {
         A.CallTo(() => _pdfReportingTask.CreateReport(_installationValidationResult, _outputFolder, false))
            .MustHaveHappened();
      }

      [Observation]
      public void should_not_use_markdown_reporting_task()
      {
         A.CallTo(() => _markdownReportingTask.CreateReport(_installationValidationResult, _outputFolder, A<bool>._))
            .MustNotHaveHappened();
      }
   }

   public class When_creating_report_with_open_report_flag : concern_for_ValidationReportingTask
   {
      protected override void Context()
      {
         base.Context();
         sut.DefaultFormat = ReportFormat.Markdown;
      }

      protected override void Because()
      {
         sut.CreateReport(_installationValidationResult, _outputFolder, openReport: true).Wait();
      }

      [Observation]
      public void should_pass_open_report_flag_to_underlying_task()
      {
         A.CallTo(() => _markdownReportingTask.CreateReport(_installationValidationResult, _outputFolder, true))
            .MustHaveHappened();
      }
   }

   public class When_creating_batch_comparison_report : concern_for_ValidationReportingTask
   {
      private BatchComparisonResult _batchComparisonResult;

      protected override void Context()
      {
         base.Context();
         _batchComparisonResult = new BatchComparisonResult();
         sut.DefaultFormat = ReportFormat.Markdown;
      }

      protected override void Because()
      {
         sut.CreateReport(_batchComparisonResult, "folder1", "folder2", openReport: false).Wait();
      }

      [Observation]
      public void should_use_markdown_reporting_task_for_batch_comparison()
      {
         A.CallTo(() => _markdownReportingTask.CreateReport(_batchComparisonResult, "folder1", "folder2", false))
            .MustHaveHappened();
      }
   }
}
