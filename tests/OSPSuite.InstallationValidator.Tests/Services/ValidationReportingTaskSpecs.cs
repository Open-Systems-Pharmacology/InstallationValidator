using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Reporting;
using OSPSuite.Core.Services;
using OSPSuite.InstallationValidator.Core.Domain;
using OSPSuite.InstallationValidator.Core.Services;

namespace OSPSuite.InstallationValidator.Services
{
   public abstract class concern_for_ValidationReportingTask : ContextSpecification<ValidationReportingTask>
   {
      protected IReportingTask _reportingTask;
      protected IReportTemplateRepository _reportTemplateRepository;
      private IValidationLogger _validationLogger;

      protected override void Context()
      {
         _reportingTask = A.Fake<IReportingTask>();
         _reportTemplateRepository = A.Fake<IReportTemplateRepository>();
         _validationLogger= A.Fake<IValidationLogger>(); 
         sut = new ValidationReportingTask(_reportTemplateRepository, _reportingTask, _validationLogger);
      }
   }

   public class When_starting_the_report_generation_task : concern_for_ValidationReportingTask
   {
      private InstallationValidationResult _installationValidationResult;
      protected override void Context()
      {
         base.Context();
         _installationValidationResult = new InstallationValidationResult
         {
            RunSummary = new BatchRunSummary(),
            ComparisonResult = new BatchComparisonResult()
         };
      }

      protected override void Because()
      {
         sut.StartReport(_installationValidationResult, "").Wait();
      }

      [Observation]
      public void the_report_task_is_used_to_generate_a_report()
      {
         A.CallTo(() => _reportingTask.CreateReportAsync(_installationValidationResult, A<ReportConfiguration>._)).MustHaveHappened();
      }
   }
}
