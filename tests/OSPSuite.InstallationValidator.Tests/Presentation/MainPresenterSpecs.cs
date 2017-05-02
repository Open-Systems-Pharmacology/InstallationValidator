using System;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Core;
using OSPSuite.Core.Services;
using OSPSuite.InstallationValidator.Core;
using OSPSuite.InstallationValidator.Core.Assets;
using OSPSuite.InstallationValidator.Core.Domain;
using OSPSuite.InstallationValidator.Core.Events;
using OSPSuite.InstallationValidator.Core.Presentation;
using OSPSuite.InstallationValidator.Core.Presentation.DTO;
using OSPSuite.InstallationValidator.Core.Presentation.Views;
using OSPSuite.InstallationValidator.Core.Services;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.InstallationValidator.Presentation
{
   public abstract class concern_for_MainPresenter : ContextSpecification<MainPresenter>
   {
      protected IMainView _mainView;
      protected ILogPresenter _logPresenter;
      protected IDialogCreator _dialogCreator;
      protected IBatchStarterTask _batchStarterTask;
      protected IBatchComparisonTask _batchComparisonTask;
      protected IApplicationConfiguration _applicationConfiguration;
      protected IValidationReportingTask _validationReportingTask;
      protected FolderDTO _outputFolderDTO;

      protected override void Context()
      {
         _mainView = A.Fake<IMainView>();
         _logPresenter = A.Fake<ILogPresenter>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _batchStarterTask = A.Fake<IBatchStarterTask>();
         _batchComparisonTask = A.Fake<IBatchComparisonTask>();

         _applicationConfiguration = A.Fake<IApplicationConfiguration>();
         A.CallTo(() => _applicationConfiguration.IssueTrackerUrl).Returns(Constants.ISSUE_TRACKER_URL);
         _validationReportingTask = A.Fake<IValidationReportingTask>();

         A.CallTo(() => _mainView.BindTo(A<FolderDTO>._))
            .Invokes(x => _outputFolderDTO = x.GetArgument<FolderDTO>(0));

         sut = new MainPresenter(_mainView, _dialogCreator, _batchStarterTask, _batchComparisonTask, _applicationConfiguration, _validationReportingTask);
      }
   }

   public class When_canceling_but_validation_not_started : concern_for_MainPresenter
   {
      protected override void Because()
      {
         sut.Abort();
      }

      [Observation]
      public void the_confirmation_should_not_be_required()
      {
         A.CallTo(_dialogCreator).MustNotHaveHappened();
      }
   }

   public class When_the_main_presenter_is_notified_that_text_should_be_appended_to_the_log : concern_for_MainPresenter
   {
      private readonly string _newText = "Text";

      protected override void Because()
      {
         sut.Handle(new AppendTextToLogEvent(_newText));
      }

      [Observation]
      public void should_append_the_text_to_the_log()
      {
         A.CallTo(() => _mainView.AppendHTML(_newText)).MustHaveHappened();
      }
   }

   public class When_the_main_presenter_is_notified_that_a_line_should_be_appended_to_the_log_in_non_html_format : concern_for_MainPresenter
   {
      private readonly string _newText = "Text";

      protected override void Because()
      {
         sut.Handle(new AppendLineToLogEvent(_newText, isHtml: false));
      }

      [Observation]
      public void should_append_the_text_to_the_log()
      {
         A.CallTo(() => _mainView.AppendText($"{Environment.NewLine}{_newText}")).MustHaveHappened();
      }
   }

   public class When_the_main_presenter_is_notified_that_a_line_should_be_appended_to_the_log_in_html_format : concern_for_MainPresenter
   {
      private readonly string _newText = "Text";

      protected override void Because()
      {
         sut.Handle(new AppendLineToLogEvent(_newText, isHtml: true));
      }

      [Observation]
      public void should_append_the_text_to_the_log()
      {
         A.CallTo(() => _mainView.AppendHTML($"<br>{_newText}")).MustHaveHappened();
      }
   }

   public class When_the_batch_start_throws_operation_canceled_exception : concern_for_MainPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _batchStarterTask.StartBatch(A<string>._, A<CancellationToken>._)).Throws<OperationCanceledException>();
      }

      [Observation]
      public async Task the_view_should_be_notified_that_the_cancel_was_triggered()
      {
         await sut.StartInstallationValidation();
         A.CallTo(() => _mainView.AppendHTML(A<string>.That.Matches(x => x.Contains(Captions.TheValidationWasCanceled)))).MustHaveHappened();
      }
   }

   public class When_the_batch_start_throws_exception : concern_for_MainPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _batchStarterTask.StartBatch(A<string>._, A<CancellationToken>._)).Throws<InvalidOperationException>();
      }

      protected override void Because()
      {
         sut.StartInstallationValidation().Wait();
      }

      [Observation]
      public void the_view_should_be_updated_with_exception_information()
      {
         A.CallTo(() => _mainView.AppendHTML(Exceptions.ExceptionViewDescription(Constants.ISSUE_TRACKER_URL))).MustHaveHappened();
      }
   }

   public class When_the_main_presenter_is_starting_the_installation_validation : concern_for_MainPresenter
   {
      private InstallationValidationResult _result;

      protected override void Context()
      {
         base.Context();
         _outputFolderDTO.FolderPath = "XXX";
      }

      protected override void Because()
      {
         _result = sut.StartInstallationValidation().Result;
      }

      [Observation]
      public void should_start_the_batch_calculation()
      {
         A.CallTo(() => _batchStarterTask.StartBatch(_outputFolderDTO.FolderPath, A<CancellationToken>._)).MustHaveHappened();
      }

      [Observation]
      public void should_state_the_comparison_task()
      {
         A.CallTo(() => _batchComparisonTask.StartComparison(_outputFolderDTO.FolderPath, A<CancellationToken>._)).MustHaveHappened();
      }

      [Observation]
      public void should_generate_the_report()
      {
         A.CallTo(() => _validationReportingTask.CreateReport(_result, _outputFolderDTO.FolderPath, true)).MustHaveHappened();
      }
   }

   public class When_selecting_an_output_folder : concern_for_MainPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _dialogCreator.AskForFolder(A<string>._, OSPSuite.Core.Domain.Constants.DirectoryKey.PROJECT, A<string>._)).Returns("A_Directory");
      }

      protected override void Because()
      {
         sut.SelectOutputFolder();
      }

      [Observation]
      public void the_dialog_creator_should_be_used_to_get_the_output_path()
      {
         A.CallTo(() => _dialogCreator.AskForFolder(A<string>._, OSPSuite.Core.Domain.Constants.DirectoryKey.PROJECT, A<string>._)).MustHaveHappened();
      }
   }
}