using System.Linq;
using FakeItEasy;
using InstallationValidator.Core.Domain;
using InstallationValidator.Core.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Collections;

namespace InstallationValidator.Services
{
   public abstract class concern_for_OutputResultToDataRepositoryMapper : ContextSpecification<OutputResultToDataRepositoryMapper>
   {
      protected IDimensionFactory _dimensionFactory;
      private IDimension _timeDimension;

      protected override void Context()
      {
         _dimensionFactory = A.Fake<IDimensionFactory>();
         sut = new OutputResultToDataRepositoryMapper(_dimensionFactory);
         _timeDimension = A.Fake<IDimension>();
         A.CallTo(() => _dimensionFactory.Dimension("Time")).Returns(_timeDimension);
      }
   }

   public class When_mapping_a_output_result_to_data_repository : concern_for_OutputResultToDataRepositoryMapper
   {
      private readonly float[] _times = {0.0f, 1.1f};
      private readonly float[] _values = {2.2f, 3.3f};
      private OutputResult _outputResult;
      private DataRepository _repository;
      private IDimension _theDimension;
      private OutputComparisonResult _outputComparisonResult;

      protected override void Context()
      {
         base.Context();
         _outputResult = new OutputResult(_times, _values)
         {
            Caption = "The Caption",
         };

         _theDimension = A.Fake<IDimension>();
         A.CallTo(() => _dimensionFactory.Dimension("TheDimension")).Returns(_theDimension);
         _outputComparisonResult = new OutputComparisonResult(new OutputComparisonResultParams("PATH") {ValuesDimension = "TheDimension"}, new ComparisonSettings(), ValidationState.Valid);
      }

      protected override void Because()
      {
         _repository = sut.MapFrom(_outputComparisonResult, _outputResult);
      }

      [Observation]
      public void the_curve_dimension_should_match_the_output_dimension()
      {
         _repository.AllButBaseGrid().First().Dimension.ShouldBeEqualTo(_theDimension);
      }

      [Observation]
      public void the_data_repository_should_have_the_caption_of_the_output()
      {
         _repository.AllButBaseGrid().First().Name.ShouldBeEqualTo(_outputResult.Caption);
      }

      [Observation]
      public void the_data_repository_should_have_time_and_values_of_output()
      {
         _repository.BaseGrid.Values.ShouldOnlyContain(_times);
         _repository.AllButBaseGrid().First().Values.ShouldOnlyContain(_values);
      }
   }

   public class When_mapping_a_output_result_to_data_repository_with_display_units : concern_for_OutputResultToDataRepositoryMapper
   {
      private readonly float[] _times = { 0.0f, 1.1f };
      private readonly float[] _values = { 2.2f, 3.3f };
      private OutputResult _outputResult;
      private DataRepository _repository;
      private IDimension _timeDimension;
      private OutputComparisonResult _outputComparisonResult;
      private Dimension _valueDimension;

      protected override void Context()
      {
         base.Context();
         _outputResult = new OutputResult(_times, _values)
         {
            Caption = "The Caption",
         };

         _timeDimension = new Dimension(new BaseDimensionRepresentation(), Constants.Dimension.TIME, "s");
         _timeDimension.AddUnit("min", 60, 0);

         A.CallTo(() => _dimensionFactory.Dimension(_timeDimension.Name)).Returns(_timeDimension);

         _valueDimension = new Dimension(new BaseDimensionRepresentation(), "ValueDim", "g");
         _valueDimension.AddUnit("mg", 10, 0);
         A.CallTo(() => _dimensionFactory.Dimension(_valueDimension.Name)).Returns(_valueDimension);

         _outputComparisonResult = new OutputComparisonResult(new OutputComparisonResultParams("PATH") { ValuesDimension = _valueDimension.Name, TimeDisplayUnit = "min", ValuesDisplayUnit = "mg"}, new ComparisonSettings(), ValidationState.Valid);
      }

      protected override void Because()
      {
         _repository = sut.MapFrom(_outputComparisonResult, _outputResult);
      }

    [Observation]
      public void the_data_repository_should_have_time_and_values_of_output_with_the_expected_display_unit()
      {
         _repository.BaseGrid.Values.ShouldOnlyContain(_times);
         _repository.BaseGrid.DisplayUnitName().ShouldBeEqualTo("min");

         var dataColumn = _repository.AllButBaseGrid().First();
         dataColumn.Values.ShouldOnlyContain(_values);
         dataColumn.DisplayUnitName().ShouldBeEqualTo("mg");
      }
   }

}
