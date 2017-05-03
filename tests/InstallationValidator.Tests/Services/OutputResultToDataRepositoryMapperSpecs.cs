using System.Linq;
using FakeItEasy;
using InstallationValidator.Core.Domain;
using InstallationValidator.Core.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;

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
         A.CallTo(() => _dimensionFactory.GetDimension("Time")).Returns(_timeDimension);
      }
   }

   public class When_mapping_a_output_result_to_data_repository : concern_for_OutputResultToDataRepositoryMapper
   {
      private readonly float[] _times = {0.0f, 1.1f};
      private readonly float[] _values = {2.2f, 3.3f};
      private OutputResult _ouputResult;
      private DataRepository _repository;
      private IDimension _theDimension;

      protected override void Context()
      {
         base.Context();
         _ouputResult = new OutputResult(_times, _values)
         {
            Caption = "The Caption",
            Dimension = "TheDimension"
         };

         _theDimension = A.Fake<IDimension>();
         A.CallTo(() => _dimensionFactory.GetDimension("TheDimension")).Returns(_theDimension);
      }

      protected override void Because()
      {
         _repository = sut.MapFrom(_ouputResult);
      }

      [Observation]
      public void the_curve_dimension_should_match_the_output_dimension()
      {
         _repository.AllButBaseGrid().First().Dimension.ShouldBeEqualTo(_theDimension);
      }

      [Observation]
      public void the_data_repository_should_have_the_caption_of_the_output()
      {
         _repository.AllButBaseGrid().First().Name.ShouldBeEqualTo(_ouputResult.Caption);
      }

      [Observation]
      public void the_data_reporitory_should_have_time_and_values_of_output()
      {
         _repository.BaseGrid.Values.ShouldOnlyContain(_times);
         _repository.AllButBaseGrid().First().Values.ShouldOnlyContain(_values);
      }
   }
}
