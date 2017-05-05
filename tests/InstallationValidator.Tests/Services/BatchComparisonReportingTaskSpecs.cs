using FakeItEasy;
using InstallationValidator.Core.Assets;
using InstallationValidator.Core.Domain;
using InstallationValidator.Core.Services;
using OSPSuite.BDDHelper;
using OSPSuite.Core;
using OSPSuite.Core.Reporting;
using OSPSuite.Core.Services;

namespace InstallationValidator.Services
{
   public abstract class concern_for_BatchComparisonReportingTask : ContextSpecification<BatchComparisonReportingTask>
   {
      protected IApplicationConfiguration _applicationConfiguration;
      protected IValidationLogger _validationLogger;
      protected IReportingTask _reportingTask;
      protected IReportTemplateRepository _reportTemplateRepository;

      protected override void Context()
      {
         _applicationConfiguration = A.Fake<IApplicationConfiguration>();
         _validationLogger = A.Fake<IValidationLogger>();
         _reportingTask = A.Fake<IReportingTask>();
         _reportTemplateRepository = A.Fake<IReportTemplateRepository>();
         sut = new BatchComparisonReportingTask(_reportTemplateRepository, _reportingTask, _validationLogger, _applicationConfiguration);
      }
   }

   public class When_creating_the_report_for_folder_comparison : concern_for_BatchComparisonReportingTask
   {
      private BatchComparisonResult _comparisonResult;

      protected override void Context()
      {
         base.Context();
         _comparisonResult = new BatchComparisonResult();
      }

      protected override void Because()
      {
         sut.CreateReport(_comparisonResult, "first folder", "second folder", openReport: true).Wait();
      }

      [Observation]
      public void the_reporting_task_must_be_called_to_start_report_creation()
      {
         A.CallTo(() => _reportingTask.CreateReportAsync(_comparisonResult, A<ReportConfiguration>._)).MustHaveHappened();
      }

      [Observation]
      public void the_path_of_the_report_is_indicated_to_the_view()
      {
         A.CallTo(() => _validationLogger.AppendLine(Logs.ReportCreatedUnder(A<string>._))).MustHaveHappened();
      }
   }
}
