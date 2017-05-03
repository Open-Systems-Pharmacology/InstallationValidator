using System.Linq;
using System.Threading;
using FakeItEasy;
using InstallationValidator.Core;
using InstallationValidator.Core.Domain;
using InstallationValidator.Core.Services;
using InstallationValidator.Helpers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;

namespace InstallationValidator.Services
{
   public abstract class concern_for_BatchComparisonTask : ContextSpecification<IBatchComparisonTask>
   {
      protected IInstallationValidatorConfiguration _configuration;
      protected string _calculatedOutputPath1;
      protected string _calculatedOutputPath2;
      private CancellationTokenSource _cancellationTokenSource;
      protected CancellationToken _token;
      protected IFolderInfoFactory _folderInfoFatory;
      private IBatchOutputFileComparer _batchOutpufFileComparer;
      private IValidationLogger _validationLogger;

      protected override void Context()
      {
         _configuration = A.Fake<IInstallationValidatorConfiguration>();
         _folderInfoFatory = A.Fake<IFolderInfoFactory>();
         _batchOutpufFileComparer = A.Fake<IBatchOutputFileComparer>();
         _validationLogger= A.Fake<IValidationLogger>(); 
         sut = new BatchComparisonTask(_configuration, _folderInfoFatory, _batchOutpufFileComparer,_validationLogger);

         A.CallTo(() => _configuration.BatchOutputsFolderPath).Returns("batchOutputPath");
         _calculatedOutputPath1 = "calculatedOutputPath1";
         _calculatedOutputPath2 = "calculatedOutputPath2";
         _cancellationTokenSource = new CancellationTokenSource();
         _token = _cancellationTokenSource.Token;
      }
   }

   public class When_starting_the_comparison_of_outputs_defined_in_one_folder_only : concern_for_BatchComparisonTask
   {
      private BatchComparisonResult _result;

      protected override void Because()
      {
         _result = sut.StartComparison(_calculatedOutputPath1, _token).Result;
      }

      [Observation]
      public void should_compare_the_outputs_of_the_default_output_folder_and_the_given_folder()
      {
         _result.FolderPath1.ShouldBeEqualTo(_configuration.BatchOutputsFolderPath);
         _result.FolderPath2.ShouldBeEqualTo(_calculatedOutputPath1);
      }
   }

   public class When_starting_the_comparison_of_two_folders : concern_for_BatchComparisonTask
   {
      private BatchComparisonResult _result;
      private FolderInfo _folderInfo1;
      private FolderInfo _folderInfo2;

      protected override void Context()
      {
         base.Context();
         _folderInfo1 = new FolderInfoForSpecs(_calculatedOutputPath1, new[] {"F1", "F2", "MISSING1"});
         _folderInfo2 = new FolderInfoForSpecs(_calculatedOutputPath2, new[] {"F1", "MISSING2", "F2"});
         A.CallTo(() => _folderInfoFatory.CreateFor(_calculatedOutputPath1, A<string>._)).Returns(_folderInfo1);
         A.CallTo(() => _folderInfoFatory.CreateFor(_calculatedOutputPath2, A<string>._)).Returns(_folderInfo2);
      }

      protected override void Because()
      {
         _result = sut.StartComparison(_calculatedOutputPath1, _calculatedOutputPath2, _token).Result;
      }

      [Observation]  
      public void should_compare_the_outputs_of_the_given_folders()
      {
         _result.FolderPath1.ShouldBeEqualTo(_calculatedOutputPath1);
         _result.FolderPath2.ShouldBeEqualTo(_calculatedOutputPath2);
      }

      [Observation]
      public void should_return_a_file_comparison_result_for_each_file_defined_in_the_input_folder_or_output_folder()
      {
         _result.FileComparisonResults.Count.ShouldBeEqualTo(4);
         var missingFiles = _result.FileComparisonResults.OfType<MissingFileComparisonResult>().ToList();
         missingFiles.Count.ShouldBeEqualTo(2);
         var missingFile1 = missingFiles.Find(x => x.FileName == "MISSING1");
         missingFile1.ShouldNotBeNull();
         missingFile1.FolderContainingFile.ShouldBeEqualTo(_calculatedOutputPath1);
         missingFile1.FolderWithoutFile.ShouldBeEqualTo(_calculatedOutputPath2);

         var missingFile2 = missingFiles.Find(x => x.FileName == "MISSING2");
         missingFile2.ShouldNotBeNull();
         missingFile2.FolderContainingFile.ShouldBeEqualTo(_calculatedOutputPath2);
         missingFile2.FolderWithoutFile.ShouldBeEqualTo(_calculatedOutputPath1);
      }
   }
}