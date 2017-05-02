using System.Collections.Generic;
using OSPSuite.Core.Chart;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.InstallationValidator.Core.Domain;
using OSPSuite.InstallationValidator.Core.Extensions;
using OSPSuite.InstallationValidator.Core.Services;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.TeXReporting.Items;
using OSPSuite.Utility.Extensions;

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
            new LineBreak(), DeviationFor(outputToReport)
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
}