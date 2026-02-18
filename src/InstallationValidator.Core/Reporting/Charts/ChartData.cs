using System.Drawing;

namespace InstallationValidator.Core.Reporting.Charts
{
   public class ChartData
   {
      public string Title { get; set; }
      public string XAxisLabel { get; set; }
      public string YAxisLabel { get; set; }
      public bool UseLogScale { get; set; }
      public CurveData Curve1 { get; set; }
      public CurveData Curve2 { get; set; }
   }

   public class CurveData
   {
      public string Name { get; set; }
      public float[] XValues { get; set; }
      public float[] YValues { get; set; }
      public Color Color { get; set; }
   }
}
