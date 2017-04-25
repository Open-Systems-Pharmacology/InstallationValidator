using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.InstallationValidator.Core;
using OSPSuite.InstallationValidator.Core.Domain;
using OSPSuite.InstallationValidator.Core.Services;

namespace OSPSuite.InstallationValidator.Services
{
   public abstract class concern_for_BatchStarterTask : ContextSpecification<BatchStarterTask>
   {
      protected IInstallationValidationConfiguration _installationValidationConfiguration;
      protected IStartableProcessFactory _startableProcessFactory;
      protected StartableProcess _startableProcess;

      protected override void Context()
      {
         _installationValidationConfiguration = A.Fake<IInstallationValidationConfiguration>();
         _startableProcessFactory = A.Fake<IStartableProcessFactory>();
         sut = new BatchStarterTask(_installationValidationConfiguration, _startableProcessFactory);
         _startableProcess = A.Fake<StartableProcess>();

         A.CallTo(() => _startableProcessFactory.CreateStartableProcess(A<string>._, A<string>._)).Returns(_startableProcess);
      }
   }

   public class When_stopping_a_validation : concern_for_BatchStarterTask
   {
      protected override void Because()
      {
         sut.StopValidation(_startableProcess);
      }

      [Observation]
      public void the_startable_process_is_stopped()
      {
         A.CallTo(() => _startableProcess.Stop()).MustHaveHappened();
      }
   }

   public class When_starting_a_validation : concern_for_BatchStarterTask
   {
      protected override void Because()
      {
         sut.StartValidation("./outputfolder");
      }

      [Observation]
      public void a_new_startable_process_is_created()
      {
         A.CallTo(() => _startableProcessFactory.CreateStartableProcess(A<string>._, A<string>._)).MustHaveHappened();
      }

      [Observation]
      public void the_startable_process_must_be_started()
      {
         A.CallTo(() => _startableProcess.Start()).MustHaveHappened();
      }
   }
}
