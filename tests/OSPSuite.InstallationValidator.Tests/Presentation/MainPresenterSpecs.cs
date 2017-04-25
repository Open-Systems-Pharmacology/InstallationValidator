using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Services;
using OSPSuite.InstallationValidator.Core.Presentation;
using OSPSuite.InstallationValidator.Core.Presentation.DTO;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using IMainView = OSPSuite.InstallationValidator.Core.Presentation.Views.IMainView;

namespace OSPSuite.InstallationValidator.Presentation
{
   public abstract class concern_for_MainPresenter : ContextSpecification<MainPresenter>
   {
      protected IMainView _mainView;
      protected ILogPresenter _logPresenter;
      protected IDialogCreator _dialogCreator;

      protected override void Context()
      {
         _mainView = A.Fake<IMainView>();
         _logPresenter = A.Fake<ILogPresenter>();
         _dialogCreator = A.Fake<IDialogCreator>();
         sut = new MainPresenter(_mainView, _logPresenter, _dialogCreator);
      }
   }

   public class When_instantiating_new_presenter : concern_for_MainPresenter
   {
      [Test]
      public void should_bind_to_a_folder_dto()
      {
         A.CallTo(() => _mainView.BindTo(A<FolderDTO>._)).MustHaveHappened();
      }

      [Observation]
      public void should_add_log_viewer_to_view()
      {
         A.CallTo(() => _mainView.AddLogView(A<ILogView>._)).MustHaveHappened();
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
