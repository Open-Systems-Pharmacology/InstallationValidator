using System.Collections.Generic;
using System.Drawing;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.InstallationValidator.Core.Domain;
using OSPSuite.Utility;

namespace OSPSuite.InstallationValidator.Core.Services
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

         var linearCurveChart = new CurveChart
         {
            DefaultYAxisScaling = Scalings.Linear,
            ChartSettings = {LegendPosition = LegendPositions.RightInside},
            Title = outputComparisonResult.Path
         };
         addToCurveChart(linearCurveChart, dataRepository1, dataRepository2);

         var logCurveChart = new CurveChart
         {
            DefaultYAxisScaling = Scalings.Log,
            ChartSettings = {LegendPosition = LegendPositions.RightInside},
            Title = outputComparisonResult.Path
         };

         addToCurveChart(logCurveChart, dataRepository1, dataRepository2);
         return new []{ linearCurveChart, logCurveChart};
      }

      private void addToCurveChart(CurveChart curveChart, DataRepository dataRepository1, DataRepository dataRepository2)
      {
         curveChart.AddCurvesFor(dataRepository1, x => x.PathAsString, _dimensionFactory, (column, curve) =>
         {
            curve.Color = Color.CornflowerBlue;
            curve.VisibleInLegend = true;
            curve.Name = column.Name;
         });
         curveChart.AddCurvesFor(dataRepository2, x => x.PathAsString, _dimensionFactory, (column, curve) =>
         {
            curve.Color = Color.OrangeRed;
            curve.Name = column.Name;
            curve.VisibleInLegend = true;
         });
      }
   }
}