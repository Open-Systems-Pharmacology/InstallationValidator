using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Batch;
using OSPSuite.Core.Domain;
using OSPSuite.InstallationValidator.Core.Assets;
using OSPSuite.InstallationValidator.Core.Domain;
using Constants = OSPSuite.InstallationValidator.Core.Constants;

namespace OSPSuite.InstallationValidator.Domain
{
   public abstract class concern_for_PointwiseComparisonStrategy : ContextSpecification<PointwiseComparisonStrategy>
   {
      protected BatchSimulationExport _simulation1;
      protected BatchSimulationExport _simulation2;
      protected BatchSimulationComparison _simulationComparison1;
      protected BatchSimulationComparison _simulationComparison2;
      protected BatchOutputComparison _outputComparison1;
      protected BatchOutputComparison _outputComparison2;
      protected BatchOutputValues _outputValues1;
      protected BatchOutputValues _outputValues2;
      protected readonly double _threshold = 1e-4;

      protected override void Context()
      {
         sut = new PointwiseComparisonStrategy();

         _simulation1 = new BatchSimulationExport {Name = "S1"};
         _simulation2 = new BatchSimulationExport {Name = "S1"};

         _simulationComparison1 = new BatchSimulationComparison(_simulation1, "F1");
         _simulationComparison2 = new BatchSimulationComparison(_simulation2, "F2");

         _outputValues1 = new BatchOutputValues {Path = "P1", Threshold = _threshold};
         _outputValues2 = new BatchOutputValues {Path = "P1", Threshold = _threshold};
         _outputComparison1 = new BatchOutputComparison(_simulationComparison1, _outputValues1);
         _outputComparison2 = new BatchOutputComparison(_simulationComparison2, _outputValues2);
      }
   }

   public class When_comparing_time_arrays_based_on_two_simulations_with_time_of_different_length : concern_for_PointwiseComparisonStrategy
   {
      private TimeComparisonResult _result;

      protected override void Context()
      {
         base.Context();
         _simulation1.Time = new[] {1f, 2f, 3f};
         _simulation2.Time = new[] {1f, 2f};
      }

      protected override void Because()
      {
         _result = sut.CompareTime(_simulationComparison1, _simulationComparison2);
      }

      [Observation]
      public void should_return_a_time_comparison_results_indication_that_the_array_have_different_length()
      {
         _result.State.ShouldBeEqualTo(ValidationState.Invalid);
         _result.Message.ShouldBeEqualTo(Validation.TimeArraysHaveDifferentLength(_simulation1.Name, _simulation1.Time.Length, _simulation2.Time.Length));
      }
   }

   public class When_comparing_time_arrays_based_on_two_simulations_with_time_undefined : concern_for_PointwiseComparisonStrategy
   {
      private TimeComparisonResult _result;

      protected override void Context()
      {
         base.Context();
         _simulation1.Time = new[] {1f, 2f, 3f};
      }

      protected override void Because()
      {
         _result = sut.CompareTime(_simulationComparison1, _simulationComparison2);
      }

      [Observation]
      public void should_return_a_time_comparison_results_indicating_that_the_array_do_not_exist()
      {
         _result.State.ShouldBeEqualTo(ValidationState.Invalid);
         _result.Message.ShouldBeEqualTo(Validation.TimeArrayDoesNotExist(_simulation2.Name, _simulationComparison2.Folder));
      }
   }

   public class When_comparing_time_arrays_based_on_two_simulations_having_comparable_values : concern_for_PointwiseComparisonStrategy
   {
      private TimeComparisonResult _result;

      protected override void Context()
      {
         base.Context();
         _simulation1.Time = new[] {1f, 2f, float.NaN, 1.1234f, 0};
         _simulation2.Time = new[] {1f, 2f, float.NaN, 1.1235f, 0};
      }

      protected override void Because()
      {
         _result = sut.CompareTime(_simulationComparison1, _simulationComparison2);
      }

      [Observation]
      public void should_return_a_time_comparison_results_indicating_a_valid_state()
      {
         _result.State.ShouldBeEqualTo(ValidationState.Valid);
      }
   }

   public class When_comparing_time_arrays_based_on_two_simulations_having_values_leading_to_a_deviation_greater_than_allowed_tolerance : concern_for_PointwiseComparisonStrategy
   {
      private TimeComparisonResult _result;

      protected override void Context()
      {
         base.Context();
         _simulation1.Time = new[] {1f, 2f, float.NaN, 1.12f, 0};
         _simulation2.Time = new[] {1f, 2f, float.NaN, 1.13f, 0};
      }

      protected override void Because()
      {
         _result = sut.CompareTime(_simulationComparison1, _simulationComparison2);
      }

      [Observation]
      public void should_return_a_time_comparison_results_indicating_that_tolerance_was_exceeded()
      {
         _result.State.ShouldBeEqualTo(ValidationState.Invalid);
         _result.Deviation.ShouldBeGreaterThan(Constants.MAX_DEVIATION_TIME);
      }
   }

   public class When_comparing_output_values_based_on_two_simulations_with_different_values_length : concern_for_PointwiseComparisonStrategy
   {
      private OutputComparisonResult _result;

      protected override void Context()
      {
         base.Context();
         _outputValues1.Values = new[] {1f, 2f, 3f};
         _outputValues2.Values = new[] {1f, 2f, 3f, 4f};
      }

      protected override void Because()
      {
         _result = sut.CompareOutputs(_outputComparison1, _outputComparison2);
      }

      [Observation]
      public void should_return_an_output_comparison_results_indication_that_the_array_have_different_length()
      {
         _result.State.ShouldBeEqualTo(ValidationState.Invalid);
         _result.Message.ShouldBeEqualTo(Validation.ValueArraysHaveDifferentLength(_simulation1.Name, _outputValues1.Path, _outputValues1.Values.Length, _outputValues2.Values.Length));
      }
   }

   public class When_comparing_output_values_based_on_two_simulations_with_one_undefined_values : concern_for_PointwiseComparisonStrategy
   {
      private OutputComparisonResult _result;

      protected override void Context()
      {
         base.Context();
         _outputValues1.Values = new[] {1f, 2f, 3f};
      }

      protected override void Because()
      {
         _result = sut.CompareOutputs(_outputComparison1, _outputComparison2);
      }

      [Observation]
      public void should_return_an_output_comparison_results_indicating_that_the_array_do_not_exist()
      {
         _result.State.ShouldBeEqualTo(ValidationState.Invalid);
         _result.Message.ShouldBeEqualTo(Validation.ValueArrayDoesNotExist(_simulation1.Name, _simulationComparison2.Folder, _outputValues1.Path));
      }
   }

   public class When_comparing_output_values_based_on_two_simulations_having_comparable_values : concern_for_PointwiseComparisonStrategy
   {
      private OutputComparisonResult _result;

      protected override void Context()
      {
         base.Context();
         _outputValues1.Values = new[] {1f, 2f, 0, 4f, (float) _threshold / 10, float.NaN};
         _outputValues2.Values = new[] {1f, 2f, 0, 4f, (float) _threshold / 20, float.NaN};
      }

      protected override void Because()
      {
         _result = sut.CompareOutputs(_outputComparison1, _outputComparison2);
      }

      [Observation]
      public void should_return_an_output_comparison_results_indicating_a_valid_state()
      {
         _result.State.ShouldBeEqualTo(ValidationState.Valid);
      }

      [Observation]
      public void should_have_set_the_output_values_to_default_output_values()
      {
         _result.Output1.ShouldBeAnInstanceOf<NullOutputResult>();
         _result.Output2.ShouldBeAnInstanceOf<NullOutputResult>();
      }
   }

   public class When_comparing_output_values_based_on_two_simulations_leading_to_a_deviation_greater_than_allowed_tolerance : concern_for_PointwiseComparisonStrategy
   {
      private OutputComparisonResult _result;

      protected override void Context()
      {
         base.Context();
         _simulation1.Time = new[] { 1f, 2f, 3f };
         _simulation2.Time = new[] { 1f, 2f, 4f };

         _outputValues1.Values = new[] {1f, 2f, 0, 4f, 0.01f};
         _outputValues2.Values = new[] {1f, 2f, 0, 4f, 0.05f};
      }

      protected override void Because()
      {
         _result = sut.CompareOutputs(_outputComparison1, _outputComparison2);
      }

      [Observation]
      public void should_return_an_output_comparison_results_indicating_that_tolerance_was_exceeded()
      {
         _result.State.ShouldBeEqualTo(ValidationState.Invalid);
         _result.Deviation.ShouldBeGreaterThan(Constants.MAX_DEVIATION_OUTPUT);
      }

      [Observation]
      public void should_have_set_the_output_values_that_can_be_used_for_further_analyses()
      {
         _result.Output1.Times.ShouldBeEqualTo(_simulation1.Time);
         _result.Output2.Times.ShouldBeEqualTo(_simulation2.Time);
         _result.Output1.Values.ShouldBeEqualTo(_outputValues1.Values);
         _result.Output2.Values.ShouldBeEqualTo(_outputValues2.Values);
      }
   }
}