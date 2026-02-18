using System.Drawing;
using InstallationValidator.Core.Domain;
using InstallationValidator.Core.Reporting.Charts;
using OSPSuite.Core.Domain;

namespace InstallationValidator.Core.Reporting.Markdown
{
   public class OutputComparisonResultMarkdownBuilder : MarkdownBuilder<OutputComparisonResult>
   {
      private readonly ISvgChartGenerator _svgChartGenerator;
      private readonly DoubleFormatter _doubleFormatter = new DoubleFormatter();

      public OutputComparisonResultMarkdownBuilder(ISvgChartGenerator svgChartGenerator)
      {
         _svgChartGenerator = svgChartGenerator;
      }

      public override void Build(OutputComparisonResult output, MarkdownReportContext context)
      {
         context.AppendHeading($"{Assets.Reporting.OutputPath}: {output.Path}", 5);
         context.AppendParagraph(output.Message);
         context.AppendBold(Assets.Reporting.Deviation, _doubleFormatter.Format(output.Deviation));

         if (canCreateChartFor(output))
         {
            var logChartData = createChartData(output, useLogScale: true);
            context.AppendSvg(_svgChartGenerator.GenerateLineChart(logChartData));

            if (!output.IsValid())
            {
               var linearChartData = createChartData(output, useLogScale: false);
               context.AppendSvg(_svgChartGenerator.GenerateLineChart(linearChartData));
            }
         }
      }

      private static bool canCreateChartFor(OutputComparisonResult output) => output.HasData;

      private ChartData createChartData(OutputComparisonResult output, bool useLogScale)
      {
         return new ChartData
         {
            Title = output.Path,
            XAxisLabel = $"Time [{output.TimeDisplayUnit}]",
            YAxisLabel = $"[{output.ValuesDisplayUnit}]",
            UseLogScale = useLogScale,
            Curve1 = new CurveData
            {
               Name = output.Output1.Caption,
               XValues = output.Output1.Times,
               YValues = output.Output1.Values,
               Color = Color.CornflowerBlue
            },
            Curve2 = new CurveData
            {
               Name = output.Output2.Caption,
               XValues = output.Output2.Times,
               YValues = output.Output2.Values,
               Color = Color.OrangeRed
            }
         };
      }
   }
}
