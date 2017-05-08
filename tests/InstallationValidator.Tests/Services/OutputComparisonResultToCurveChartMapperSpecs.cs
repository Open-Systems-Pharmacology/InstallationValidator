using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using InstallationValidator.Core.Domain;
using InstallationValidator.Core.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;

namespace InstallationValidator.Services
{
   public abstract class concern_for_OutputComparisonResultToCurveChartMapper : ContextSpecification<OutputComparisonResultToCurveChartMapper>
   {
      protected IDimensionFactory _dimensionFactory;
      protected IOutputResultToDataRepositoryMapper _outputResultToDataRepositoryMapper;

      protected override void Context()
      {
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _outputResultToDataRepositoryMapper = A.Fake<IOutputResultToDataRepositoryMapper>();
         sut = new OutputComparisonResultToCurveChartMapper(_outputResultToDataRepositoryMapper, _dimensionFactory);

         A.CallTo(() => _outputResultToDataRepositoryMapper.MapFrom(A<OutputResult>._)).ReturnsLazily(() => new DataRepository());
      }

      public class When_the_mapper_maps_from_an_invalid_output_comparison : concern_for_OutputComparisonResultToCurveChartMapper
      {
         private OutputComparisonResult _outputComparisonResult;
         private List<CurveChart> _charts;

         protected override void Context()
         {
            base.Context();
            var comparsionSettings = new ComparisonSettings();
            _outputComparisonResult = new OutputComparisonResult("path", comparsionSettings, ValidationState.Invalid, string.Empty);
         }

         protected override void Because()
         {
            _charts = sut.MapFrom(_outputComparisonResult).ToList();
         }

         [Observation]
         public void the_repositories_should_be_configured_correctly()
         {
            _charts.Count(x => x.DefaultYAxisScaling == Scalings.Linear).ShouldBeEqualTo(1);
            _charts.Count(x => x.DefaultYAxisScaling == Scalings.Log).ShouldBeEqualTo(1);

            _charts.Count(x => x.Title == _outputComparisonResult.Path).ShouldBeEqualTo(2);
            _charts.Count(x => x.ChartSettings.LegendPosition == LegendPositions.RightInside).ShouldBeEqualTo(2);
         }

         [Observation]
         public void a_log_and_a_lin_charts_should_be_returned()
         {
            _charts.Count.ShouldBeEqualTo(2);
         }
      }

      public class When_the_mapper_maps_from_a_valid_output_comparison : concern_for_OutputComparisonResultToCurveChartMapper
      {
         private OutputComparisonResult _outputComparisonResult;
         private List<CurveChart> _repositories;

         protected override void Context()
         {
            base.Context();
            var comparsionSettings = new ComparisonSettings();
            _outputComparisonResult = new OutputComparisonResult("path", comparsionSettings, ValidationState.Valid, string.Empty);
         }

         protected override void Because()
         {
            _repositories = sut.MapFrom(_outputComparisonResult).ToList();
         }

         [Observation]
         public void the_repositories_should_be_configured_with_only_a_log_plot()
         {
            _repositories.Count(x => x.DefaultYAxisScaling == Scalings.Log).ShouldBeEqualTo(1);
            _repositories.Count(x => x.Title == _outputComparisonResult.Path).ShouldBeEqualTo(1);
            _repositories.Count(x => x.ChartSettings.LegendPosition == LegendPositions.RightInside).ShouldBeEqualTo(1);
         }

         [Observation]
         public void only_a_log_chart_should_be_returned()
         {
            _repositories.Count.ShouldBeEqualTo(1);
         }
      }
   }
}