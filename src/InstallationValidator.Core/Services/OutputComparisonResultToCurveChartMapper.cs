using System.Collections.Generic;
using System.Drawing;
using InstallationValidator.Core.Domain;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility;

namespace InstallationValidator.Core.Services
{
   public interface IOutputComparisonResultToCurveChartMapper : IMapper<OutputComparisonResult, IEnumerable<CurveChart>>
   {
   }

   public class OutputComparisonResultToCurveChartMapper : IOutputComparisonResultToCurveChartMapper
   {
      private readonly IOutputResultToDataRepositoryMapper _outputResultToDataRepositoryMapper;
      private readonly IDimensionFactory _dimensionFactory;

      public OutputComparisonResultToCurveChartMapper(IOutputResultToDataRepositoryMapper outputResultToDataRepositoryMapper, IDimensionFactory dimensionFactory)
      {
         _outputResultToDataRepositoryMapper = outputResultToDataRepositoryMapper;
         _dimensionFactory = dimensionFactory;
      }

      public IEnumerable<CurveChart> MapFrom(OutputComparisonResult outputComparisonResult)
      {
         var dataRepository1 = _outputResultToDataRepositoryMapper.MapFrom(outputComparisonResult.Output1);
         var dataRepository2 = _outputResultToDataRepositoryMapper.MapFrom(outputComparisonResult.Output2);

         yield return createCurveChartWithScaling(outputComparisonResult, Scalings.Log, dataRepository1, dataRepository2);

         //only show linear plot if output is invalid
         if (!outputComparisonResult.IsValid())
            yield return createCurveChartWithScaling(outputComparisonResult, Scalings.Linear, dataRepository1, dataRepository2);
      }

      private CurveChart createCurveChartWithScaling(OutputComparisonResult outputComparisonResult, Scalings defaultYAxisScaling, DataRepository dataRepository1, DataRepository dataRepository2)
      {
         var linearCurveChart = new CurveChart
         {
            DefaultYAxisScaling = defaultYAxisScaling,
            ChartSettings = {LegendPosition = LegendPositions.RightInside},
            Title = outputComparisonResult.Path
         };
         addToCurveChart(linearCurveChart, dataRepository1, dataRepository2);
         return linearCurveChart;
      }

      private void addToCurveChart(CurveChart curveChart, DataRepository dataRepository1, DataRepository dataRepository2)
      {
         addCurve(curveChart, dataRepository1, Color.CornflowerBlue);
         addCurve(curveChart, dataRepository2, Color.OrangeRed);
      }

      private void addCurve(CurveChart curveChart, DataRepository dataRepository, Color color)
      {
         curveChart.AddCurvesFor(dataRepository, x => x.PathAsString, _dimensionFactory, (column, curve) =>
         {
            curve.Color = color;
            curve.VisibleInLegend = true;
            curve.Name = column.Name;
         });
      }
   }
}