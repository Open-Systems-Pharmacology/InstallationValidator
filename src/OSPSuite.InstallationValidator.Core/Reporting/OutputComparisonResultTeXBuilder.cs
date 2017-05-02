using System.Collections.Generic;
using FluentNHibernate.Utils;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.InstallationValidator.Core.Domain;
using OSPSuite.InstallationValidator.Core.Extensions;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.TeXReporting.Items;
using OSPSuite.Utility;

namespace OSPSuite.InstallationValidator.Core.Reporting
{
   public class OutputComparisonResultTeXBuilder : ValueComparisonResultTeXBuilder<OutputComparisonResult>
   {
      private readonly ITeXBuilderRepository _builderRepository;
      private readonly IOutputComparisonResultToCurveChartMapper _outputComparisonResultToCurveChartMapper;

      public OutputComparisonResultTeXBuilder(ITeXBuilderRepository builderRepository, IOutputComparisonResultToCurveChartMapper outputComparisonResultToCurveChartMapper)
      {
         _builderRepository = builderRepository;
         _outputComparisonResultToCurveChartMapper = outputComparisonResultToCurveChartMapper;
      }
      public override void Build(OutputComparisonResult outputToReport, OSPSuiteTracker buildTracker)
      {
         var objectsToReport = new List<object>
         {
            new LineBreak(),
            Assets.Reporting.OutputComparisonValidation,
            new LineBreak(), ValidationMessageFor(outputToReport),
            new LineBreak(), outputPathFor(outputToReport),
            new LineBreak(), DeviationFor(outputToReport),

         };

         if (canCreateChartFor(outputToReport))
         {
            chartsFor(outputToReport).Each(chart =>
            {
               objectsToReport.AddRange(new object[] { new LineBreak(), chart});
            });
         }

         _builderRepository.Report(objectsToReport, buildTracker);
      }

      private static bool canCreateChartFor(OutputComparisonResult outputToReport)
      {
         return !(outputToReport.Output1.IsNullOutput() || outputToReport.Output2.IsNullOutput());
      }

      private IEnumerable<CurveChart> chartsFor(OutputComparisonResult outputComparisonResult)
      {
         return _outputComparisonResultToCurveChartMapper.MapFrom(outputComparisonResult);
      }

      private static string outputPathFor(OutputComparisonResult outputToReport)
      {
         return $"{Assets.Reporting.OutputPath}: {outputToReport.Path}";
      }
   }

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

         var linearCurveChart = new CurveChart {DefaultYAxisScaling = Scalings.Linear};
         addToCurveChart(linearCurveChart, dataRepository1, dataRepository2);

         var logCurveChart = new CurveChart { DefaultYAxisScaling = Scalings.Log };
         addToCurveChart(logCurveChart, dataRepository1, dataRepository2);
         return new []{ linearCurveChart, logCurveChart};
      }

      private void addToCurveChart(CurveChart curveChart, DataRepository dataRepository1, DataRepository dataRepository2)
      {
         curveChart.AddCurvesFor(dataRepository1, x => x.PathAsString, _dimensionFactory, (column, curve) => { curve.Name = "Output 1"; });
         curveChart.AddCurvesFor(dataRepository2, x => x.PathAsString, _dimensionFactory, (column, curve) => { curve.Name = "Output 2"; });
      }
   }

   public interface IOutputResultToDataRepositoryMapper : IMapper<OutputResult, DataRepository>
   {
   }

   public class OutputResultToDataRepositoryMapper : IOutputResultToDataRepositoryMapper
   {
      private readonly IDimensionFactory _dimensionFactory;

      public OutputResultToDataRepositoryMapper(IDimensionFactory dimensionFactory)
      {
         _dimensionFactory = dimensionFactory;
      }

      public DataRepository MapFrom(OutputResult outputResult)
      {
         var repository = new DataRepository();

         var timeGrid = new BaseGrid("Time", _dimensionFactory.GetDimension("Time"))
         {
            Values = outputResult.Times
         };


         var outputColumn = new DataColumn("Values", _dimensionFactory.NoDimension, timeGrid)
         {
            Values = outputResult.Values
         };


         repository.Add(outputColumn);

         return repository;
      }
   }
}