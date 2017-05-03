using System.IO;
using System.Linq;
using System.Threading;
using FakeItEasy;
using InstallationValidator.Core.Domain;
using InstallationValidator.Core.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Batch;
using OSPSuite.Core.Domain;

namespace InstallationValidator.Services
{
   public abstract class concern_for_BatchOutputFileComparer : ContextSpecification<IBatchOutputFileComparer>
   {
      protected string _fileName;
      protected string _folderPath1;
      protected string _folderPath2;
      private IBatchOutputLoader _batchOutputLoader;
      protected IComparisonStrategy _comparisonStrategy;
      protected CancellationToken _token;
      private CancellationTokenSource _cancellationTokenSource;
      protected BatchSimulationExport _simulationExport1;
      protected BatchSimulationExport _simulationExport2;

      protected override void Context()
      {
         _fileName = "F";
         _folderPath1 = "Path1";
         _folderPath2 = "Path2";

         _batchOutputLoader = A.Fake<IBatchOutputLoader>();
         _comparisonStrategy = A.Fake<IComparisonStrategy>();
         _cancellationTokenSource = new CancellationTokenSource();
         _token = _cancellationTokenSource.Token;
         sut = new BatchOutputFileComparer(_batchOutputLoader, _comparisonStrategy);

         _simulationExport1 = new BatchSimulationExport();
         _simulationExport2 = new BatchSimulationExport();

         A.CallTo(() => _batchOutputLoader.LoadFrom(Path.Combine(_folderPath1, _fileName))).Returns(_simulationExport1);
         A.CallTo(() => _batchOutputLoader.LoadFrom(Path.Combine(_folderPath2, _fileName))).Returns(_simulationExport2);
      }
   }

   public class When_starting_the_comparison_of_a_batch_output_file_defined_in_a_source_and_target_folder : concern_for_BatchOutputFileComparer
   {
      private OutputFileComparisonResult _result;
      private TimeComparisonResult _timeComparison;
      private OutputComparisonResult _outputComparison;

      protected override void Context()
      {
         base.Context();
         _timeComparison = new TimeComparisonResult(ValidationState.Valid);
         _outputComparison = new OutputComparisonResult("P1", ValidationState.Valid);
         A.CallTo(() => _comparisonStrategy.CompareTime(A<BatchSimulationComparison>._, A<BatchSimulationComparison>._)).Returns(_timeComparison);
         A.CallTo(() => _comparisonStrategy.CompareOutputs(A<BatchOutputComparison>._, A<BatchOutputComparison>._)).Returns(_outputComparison);

         _simulationExport1.OutputValues.Add(new BatchOutputValues {Path = "P1"});
         _simulationExport1.OutputValues.Add(new BatchOutputValues {Path = "P2"});

         _simulationExport2.OutputValues.Add(new BatchOutputValues {Path = "P1"});
         _simulationExport2.OutputValues.Add(new BatchOutputValues {Path = "P3"});
      }

      protected override void Because()
      {
         _result = sut.Compare(_fileName, _folderPath1, _folderPath2, _token).Result;
      }

      [Observation]
      public void should_return_an_output_file_comparison_result()
      {
         _result.ShouldNotBeNull();
      }

      [Observation]
      public void should_contain_one_missing_output_comparison_for_each_output_missing()
      {
         var missingOutputs = _result.OutputComparisonResults.OfType<MissingOutputComparisonResult>().ToList();
         missingOutputs.Count.ShouldBeEqualTo(2);
         missingOutputs[0].Path.ShouldBeEqualTo("P2");
         missingOutputs[1].Path.ShouldBeEqualTo("P3");
      }

      [Observation]
      public void should_contain_a_time_comparison_for_the_simulation_time()
      {
         _result.TimeComparison.ShouldBeEqualTo(_timeComparison);
      }

      [Observation]
      public void should_contain_an_output_comparison_for_each_output_defined_in_both_simulations()
      {
         _result.OutputComparisonResults.ShouldContain(_outputComparison);
      }
   }
}