using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.InstallationValidator.Core.Domain;
using OSPSuite.InstallationValidator.Core.Services;

namespace OSPSuite.InstallationValidator.Services
{
   public abstract class concern_for_BatchOutputFileComparer : ContextSpecification<IBatchOutputFileComparer>
   {
      protected string _fileName;
      protected string _folderPath1;
      protected string _folderPath2;

      protected override void Context()
      {
         _fileName = "F";
         _folderPath1 = "Path1";
         _folderPath2 = "Path2";

         sut = new BatchOutputFileComparer();
      }
   }

   public class When_starting_the_comparison_of_a_batch_output_file_defined_in_a_source_and_target_folder : concern_for_BatchOutputFileComparer
   {
      private OutputFileComparisonResult _result;

      protected override void Because()
      {
         _result = sut.Compare(_fileName, _folderPath1, _folderPath2);
      }

      [Observation]
      public void should_return_an_output_file_comparison_result()
      {
         _result.ShouldNotBeNull();
      }
   }
}