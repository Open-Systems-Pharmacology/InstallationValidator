using System;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Reporting;
using OSPSuite.Core.Services;
using OSPSuite.InstallationValidator.Core.Domain;
using OSPSuite.InstallationValidator.Core.Services;
using OSPSuite.Utility;

namespace OSPSuite.InstallationValidator.Services
{
   public abstract class concern_for_ValidationReportingTask : ContextSpecification<ValidationReportingTask>
   {
      protected IReportingTask _reportingTask;
      protected IReportTemplateRepository _reportTemplateRepository;
      private IValidationLogger _validationLogger;
      protected InstallationValidationResult _installationValidationResult;
      protected string _fileName = "TOTO";
      private Action<string> _tryOpenFile;
      protected string _fileToOpen;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _tryOpenFile = FileHelper.TryOpenFile;

         FileHelper.TryOpenFile = (s) => _fileToOpen = _fileName;
      }

      protected override void Context()
      {
         _reportingTask = A.Fake<IReportingTask>();
         _reportTemplateRepository = A.Fake<IReportTemplateRepository>();
         _validationLogger = A.Fake<IValidationLogger>();
         sut = new ValidationReportingTask(_reportTemplateRepository, _reportingTask, _validationLogger);
         _installationValidationResult = new InstallationValidationResult
         {
            RunSummary = new BatchRunSummary(),
            ComparisonResult = new BatchComparisonResult()
         };
      }

      public override void GlobalCleanup()
      {
         base.GlobalCleanup();
         FileHelper.TryOpenFile = _tryOpenFile;
      }
   }

   public class When_starting_the_report_generation_task_and_report_should_not_be_opened : concern_for_ValidationReportingTask
   {
      protected override void Because()
      {
         sut.CreateReport(_installationValidationResult, _fileName).Wait();
      }

      [Observation]
      public void the_report_task_is_used_to_generate_a_report()
      {
         A.CallTo(() => _reportingTask.CreateReportAsync(_installationValidationResult, A<ReportConfiguration>._)).MustHaveHappened();
      }

      [Observation]
      public void should_not_open_the_report()
      {
         _fileToOpen.ShouldBeNull();
      }
   }

   public class When_starting_the_report_generation_task_and_report_should_be_opened : concern_for_ValidationReportingTask
   {
      protected override void Because()
      {
         sut.CreateReport(_installationValidationResult, _fileName, openReport: true).Wait();
      }

      [Observation]
      public void the_report_task_is_used_to_generate_a_report()
      {
         A.CallTo(() => _reportingTask.CreateReportAsync(_installationValidationResult, A<ReportConfiguration>._)).MustHaveHappened();
      }

      [Observation]
      public void should_not_open_the_report()
      {
         _fileToOpen.ShouldBeEqualTo(_fileName);
      }
   }
}