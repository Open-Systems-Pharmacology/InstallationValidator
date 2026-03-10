using System.Drawing;
using InstallationValidator.Core.Domain;

namespace InstallationValidator.Core.Reporting.Charts
{
   public static class ChartDataFactory
   {
      public static ChartData CreateFor(OutputComparisonResult output, bool useLogScale)
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
