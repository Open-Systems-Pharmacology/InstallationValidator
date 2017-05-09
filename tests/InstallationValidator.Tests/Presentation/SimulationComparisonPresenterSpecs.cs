using System;
using System.Threading;
using FakeItEasy;
using InstallationValidator.Core;
using InstallationValidator.Core.Domain;
using InstallationValidator.Core.Events;
using InstallationValidator.Core.Presentation;
using InstallationValidator.Core.Presentation.DTO;
using InstallationValidator.Core.Presentation.Views;
using InstallationValidator.Core.Services;
using OSPSuite.BDDHelper;
using OSPSuite.Core;
using OSPSuite.Core.Services;

namespace InstallationValidator.Presentation
{
   public abstract class concern_for_SimulationComparisonPresenter : ContextSpecification<SimulationComparisonPresenter>
   {
      protected ISimulationComparisonView _simulationComparisonView;
      protected IInstallationValidatorConfiguration _applicationConfiguration;
      protected IDialogCreator _dialogCreator;
      protected IBatchComparisonTask _batchComparisonTask;
      protected IValidationReportingTask _validationReportingTask;
      protected FolderDTO _firstFolder;
      protected FolderDTO _secondFolder;

      protected override void Context()
      {
         _simulationComparisonView = A.Fake<ISimulationComparisonView>();
         _applicationConfiguration = A.Fake<IInstallationValidatorConfiguration>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _batchComparisonTask = A.Fake<IBatchComparisonTask>();
         _validationReportingTask = A.Fake<IValidationReportingTask>();

         A.CallTo(() => _simulationComparisonView.BindTo(A<FolderComparisonDTO>._))
            .Invokes(x =>
            {
               var folderComparisonDTO = x.GetArgument<FolderComparisonDTO>(0);
               _firstFolder = folderComparisonDTO.firstFolder;
               _secondFolder = folderComparisonDTO.secondFolder;
            });

         sut = new SimulationComparisonPresenter(_simulationComparisonView, _applicationConfiguration, _dialogCreator, _batchComparisonTask, _validationReportingTask);
      }
   }

   public class When_canceling_but_comparison_not_started : concern_for_SimulationComparisonPresenter
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

   public class When_the_comparison_presenter_is_notified_that_text_should_be_appended_to_the_log : concern_for_SimulationComparisonPresenter
   {
      private readonly string _newText = "Text";

      protected override void Because()
      {
         sut.Handle(new AppendTextToLogEvent(_newText));
      }

      [Observation]
      public void should_append_the_text_to_the_log()
      {
         A.CallTo(() => _simulationComparisonView.AppendHTML(_newText)).MustHaveHappened();
      }
   }

   public class When_the_comparison_presenter_is_notified_that_a_line_should_be_appended_to_the_log_in_non_html_format : concern_for_SimulationComparisonPresenter
   {
      private readonly string _newText = "Text";

      protected override void Because()
      {
         sut.Handle(new AppendLineToLogEvent(_newText, isHtml: false));
      }

      [Observation]
      public void should_append_the_text_to_the_log()
      {
         A.CallTo(() => _simulationComparisonView.AppendText($"{Environment.NewLine}{_newText}")).MustHaveHappened();
      }
   }

   public class When_the_comparison_presenter_is_notified_that_a_line_should_be_appended_to_the_log_in_html_format : concern_for_SimulationComparisonPresenter
   {
      private readonly string _newText = "Text";

      protected override void Because()
      {
         sut.Handle(new AppendLineToLogEvent(_newText, isHtml: true));
      }

      [Observation]
      public void should_append_the_text_to_the_log()
      {
         A.CallTo(() => _simulationComparisonView.AppendHTML($"<br>{_newText}")).MustHaveHappened();
      }
   }

   public class When_the_comparison_presenter_is_starting_the_comparison : concern_for_SimulationComparisonPresenter
   {
      protected override void Context()
      {
         base.Context();
         _firstFolder.FolderPath = "XXX";
         _secondFolder.FolderPath = "YYY";
      }

      protected override void Because()
      {
         sut.StartComparison().Wait();
      }

      [Observation]
      public void should_start_the_comparison_task()
      {
         A.CallTo(() => _batchComparisonTask.StartComparison(A<ComparisonSettings>._, A<CancellationToken>._)).MustHaveHappened();
      }

      [Observation]
      public void should_generate_the_report()
      {
         A.CallTo(() => _validationReportingTask.CreateReport(A<BatchComparisonResult>._, _firstFolder.FolderPath, _secondFolder.FolderPath, true)).MustHaveHappened();
      }
   }

   public class When_comparer_selects_an_output_folder : concern_for_SimulationComparisonPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _dialogCreator.AskForFolder(A<string>._, OSPSuite.Core.Domain.Constants.DirectoryKey.PROJECT, A<string>._)).Returns("A_Directory");
      }

      [Observation]
      public void the_dialog_creator_should_be_used_to_get_the_second_path()
      {
         sut.SelectSecondFolder();
         A.CallTo(() => _dialogCreator.AskForFolder(A<string>._, OSPSuite.Core.Domain.Constants.DirectoryKey.PROJECT, A<string>._)).MustHaveHappened();
      }

      [Observation]
      public void the_dialog_creator_should_be_used_to_get_the_first_path()
      {
         sut.SelectFirstFolder();
         A.CallTo(() => _dialogCreator.AskForFolder(A<string>._, OSPSuite.Core.Domain.Constants.DirectoryKey.PROJECT, A<string>._)).MustHaveHappened();
      }
   }
}
