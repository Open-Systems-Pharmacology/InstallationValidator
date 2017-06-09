using System;
using System.Collections.Generic;
using System.Threading;
using FakeItEasy;
using InstallationValidator.Core;
using InstallationValidator.Core.Domain;
using InstallationValidator.Core.Services;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Utility;

namespace InstallationValidator.Services
{
   public abstract class concern_for_BatchStarterTask : ContextSpecification<BatchStarterTask>
   {
      protected IInstallationValidatorConfiguration _installationValidationConfiguration;
      protected IStartableProcessFactory _startableProcessFactory;
      protected ILogWatcherFactory _logWatcherFactory;
      protected StartableProcess _startableProcess;
      protected ILogWatcher _logWatcher;

      protected override void Context()
      {
         _installationValidationConfiguration = A.Fake<IInstallationValidatorConfiguration>();
         _startableProcessFactory = A.Fake<IStartableProcessFactory>();
         _logWatcherFactory = A.Fake<ILogWatcherFactory>();

         _logWatcher = A.Fake<ILogWatcher>();

         sut = new BatchStarterTask(_installationValidationConfiguration, _startableProcessFactory, _logWatcherFactory);
         _startableProcess = A.Fake<StartableProcess>();

         A.CallTo(() => _startableProcessFactory.CreateStartableProcess(A<string>._, A<string[]>._)).Returns(_startableProcess);
         A.CallTo(() => _logWatcherFactory.CreateLogWatcher(A<string>._, A<IEnumerable<string>>._)).Returns(_logWatcher);
      }
   }

   public class When_starting_a_batch_calculation : concern_for_BatchStarterTask
   {
      private Func<string, string> _getVersionInfo;
      private Func<string, string> _createDirectory;
      private readonly string _outputFolderPath = "outputfolder";
      private string _createdFolderPath;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _getVersionInfo = ValidationFileHelper.GetVersion;
         _createDirectory = DirectoryHelper.CreateDirectory;
         ValidationFileHelper.GetVersion = path => "1.0.0";

         DirectoryHelper.CreateDirectory = s => _createdFolderPath = s;
      }

      protected override void Because()
      {
         sut.StartBatch(_outputFolderPath, new CancellationToken()).Wait();
      }

      [Test]
      public void a_new_startable_process_is_created()
      {
         A.CallTo(() => _startableProcessFactory.CreateStartableProcess(A<string>._, A<string[]>._)).MustHaveHappened();
      }

      [Observation]
      public void a_file_watch_is_started()
      {
         A.CallTo(() => _logWatcher.Watch()).MustHaveHappened();
      }

      [Observation]
      public void the_startable_process_must_be_started()
      {
         A.CallTo(() => _startableProcess.Start()).MustHaveHappened();
         A.CallTo(() => _startableProcess.Wait(A<CancellationToken>._)).MustHaveHappened();
      }

      [Observation]
      public void should_create_the_target_folder_if_it_does_not_exist()
      {
         _createdFolderPath.ShouldBeEqualTo(_outputFolderPath);
      }

      public override void GlobalCleanup()
      {
         base.GlobalCleanup();
         ValidationFileHelper.GetVersion = _getVersionInfo;
         DirectoryHelper.CreateDirectory = _createDirectory;
      }
   }
}