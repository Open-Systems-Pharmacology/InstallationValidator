using System.Collections.Generic;
using InstallationValidator.Core.Domain;
using InstallationValidator.Core.Services;
using OSPSuite.Core.Chart;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.TeXReporting.Items;
using OSPSuite.Utility.Extensions;

namespace InstallationValidator.Core.Reporting
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
            new SubParagraph($"{Assets.Reporting.OutputPath}: {outputToReport.Path}"),
            ValidationMessageFor(outputToReport),
            DeviationFor(outputToReport)
         };

         if (canCreateChartFor(outputToReport))
         {
            chartsFor(outputToReport).Each(chart => { objectsToReport.AddRange(new object[] {new LineBreak(), chart}); });
         }

         _builderRepository.Report(objectsToReport, buildTracker);
      }

      private static bool canCreateChartFor(OutputComparisonResult outputToReport) => outputToReport.HasData;

      private IEnumerable<CurveChart> chartsFor(OutputComparisonResult outputComparisonResult)
      {
         return _outputComparisonResultToCurveChartMapper.MapFrom(outputComparisonResult);
      }
   }
}