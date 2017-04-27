using System;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Core;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.InstallationValidator.Core;
using OSPSuite.InstallationValidator.Core.Assets;
using OSPSuite.InstallationValidator.Core.Events;
using OSPSuite.InstallationValidator.Core.Presentation;
using OSPSuite.InstallationValidator.Core.Services;
using OSPSuite.Presentation.Presenters;
using IMainView = OSPSuite.InstallationValidator.Core.Presentation.Views.IMainView;

namespace OSPSuite.InstallationValidator.Presentation
{
   public abstract class concern_for_MainPresenter : ContextSpecification<MainPresenter>
   {
      protected IMainView _mainView;
      protected ILogPresenter _logPresenter;
      protected IDialogCreator _dialogCreator;
      protected IBatchStarterTask _batchStarterTask;
      private IBatchComparisonTask _batchComparisonTask;
      private IApplicationConfiguration _applicationConfiguration;

      protected override void Context()
      {
         _mainView = A.Fake<IMainView>();
         _logPresenter = A.Fake<ILogPresenter>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _batchStarterTask = A.Fake<IBatchStarterTask>();
         _batchComparisonTask = A.Fake<IBatchComparisonTask>();

         _applicationConfiguration = A.Fake<IApplicationConfiguration>();
         A.CallTo(() => _applicationConfiguration.IssueTrackerUrl).Returns(Constants.ISSUE_TRACKER_URL);
         sut = new MainPresenter(_mainView, _dialogCreator, _batchStarterTask, _batchComparisonTask, _applicationConfiguration);
      }
   }

   public class When_cancaling_a_validation_is_declined : concern_for_MainPresenter
   {
      private CancellationToken _token;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _batchStarterTask.StartBatch(A<string>._, A<CancellationToken>._)).Invokes((string aString, CancellationToken token) => _token = token);
         A.CallTo(_dialogCreator).WithReturnType<ViewResult>().Returns(ViewResult.No);
      }

      [Observation]
      public async Task confirmation_is_requested_and_task_is_canceled()
      {
         await sut.StartInstallationValidation();
         sut.Abort();
         A.CallTo(_dialogCreator).WithReturnType<ViewResult>().MustHaveHappened();
         _token.IsCancellationRequested.ShouldBeFalse();
      }
   }

   public class When_canceling_a_validation : concern_for_MainPresenter
   {
      private CancellationToken _token;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _batchStarterTask.StartBatch(A<string>._, A<CancellationToken>._)).Invokes((string aString, CancellationToken token) => _token = token);
         A.CallTo(_dialogCreator).WithReturnType<ViewResult>().Returns(ViewResult.Yes);
      }

      [Observation]
      public async Task confirmation_is_requested_and_task_is_canceled()
      {
         await sut.StartInstallationValidation();
         sut.Abort();
         A.CallTo(_dialogCreator).WithReturnType<ViewResult>().MustHaveHappened();
         _token.IsCancellationRequested.ShouldBeTrue();
      }
   }

   public class When_responding_to_published_events : concern_for_MainPresenter
   {
      private string _newText = "Text";

      [Observation]
      public void view_is_notified_of_reset_events()
      {
         sut.Handle(new LogResetEvent(_newText));
         A.CallTo(() => _mainView.ResetText(_newText)).MustHaveHappened();
      }

      [Observation]
      public void view_is_notified_of_append_events()
      {
         sut.Handle(new LogAppendedEvent(_newText));
         A.CallTo(() => _mainView.AppendText(_newText)).MustHaveHappened();
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
         A.CallTo(() => _mainView.AppendText(Captions.TheValidationWasCanceled)).MustHaveHappened();
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
         A.CallTo(() => _mainView.AppendText(Captions.Exceptions.ExceptionSupportMessage(Constants.ISSUE_TRACKER_URL))).MustHaveHappened();
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
