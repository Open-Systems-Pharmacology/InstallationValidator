using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;

namespace InstallationValidator.Core.Reporting.Charts
{
   public interface ISvgChartGenerator
   {
      string GenerateLineChart(ChartData chartData);
   }

   public class SvgChartGenerator : ISvgChartGenerator
   {
      private const int Width = 700;
      private const int Height = 400;
      private const int MarginLeft = 80;
      private const int MarginRight = 140;
      private const int MarginTop = 40;
      private const int MarginBottom = 60;

      private int PlotWidth => Width - MarginLeft - MarginRight;
      private int PlotHeight => Height - MarginTop - MarginBottom;
      private int PlotRight => MarginLeft + PlotWidth;
      private int PlotBottom => MarginTop + PlotHeight;

      public string GenerateLineChart(ChartData chartData)
      {
         var sb = new StringBuilder();

         var (minX, maxX, minY, maxY) = calculateBounds(chartData);

         if (chartData.UseLogScale)
         {
            minY = minY > 0 ? (float)Math.Log10(minY) : -1;
            maxY = maxY > 0 ? (float)Math.Log10(maxY) : 1;
         }

         addPadding(ref minX, ref maxX, ref minY, ref maxY);

         sb.AppendLine($"<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 {Width} {Height}\" style=\"max-width: 100%; height: auto; font-family: Arial, sans-serif;\">");

         appendBackground(sb);
         appendGridLines(sb, minX, maxX, minY, maxY, chartData.UseLogScale);
         appendAxes(sb, minX, maxX, minY, maxY, chartData);
         appendCurve(sb, chartData.Curve1, minX, maxX, minY, maxY, chartData.UseLogScale);
         appendCurve(sb, chartData.Curve2, minX, maxX, minY, maxY, chartData.UseLogScale);
         appendLegend(sb, chartData);
         appendTitle(sb, chartData.Title, chartData.UseLogScale);

         sb.AppendLine("</svg>");
         return sb.ToString();
      }

      private (float minX, float maxX, float minY, float maxY) calculateBounds(ChartData chartData)
      {
         var allX = chartData.Curve1.XValues.Concat(chartData.Curve2.XValues).Where(v => !float.IsNaN(v) && !float.IsInfinity(v)).ToArray();
         var allY = chartData.Curve1.YValues.Concat(chartData.Curve2.YValues).Where(v => !float.IsNaN(v) && !float.IsInfinity(v) && v > 0).ToArray();

         if (allX.Length == 0 || allY.Length == 0)
            return (0, 1, 0.1f, 1);

         return (allX.Min(), allX.Max(), allY.Min(), allY.Max());
      }

      private void addPadding(ref float minX, ref float maxX, ref float minY, ref float maxY)
      {
         var xPadding = (maxX - minX) * 0.05f;
         var yPadding = (maxY - minY) * 0.1f;

         if (xPadding == 0) xPadding = 0.1f;
         if (yPadding == 0) yPadding = 0.1f;

         minX -= xPadding;
         maxX += xPadding;
         minY -= yPadding;
         maxY += yPadding;
      }

      private void appendBackground(StringBuilder sb)
      {
         sb.AppendLine($"  <rect x=\"{MarginLeft}\" y=\"{MarginTop}\" width=\"{PlotWidth}\" height=\"{PlotHeight}\" fill=\"#fafafa\" stroke=\"#ccc\" stroke-width=\"1\"/>");
      }

      private void appendGridLines(StringBuilder sb, float minX, float maxX, float minY, float maxY, bool useLogScale)
      {
         var numGridLines = 5;

         for (int i = 0; i <= numGridLines; i++)
         {
            var y = MarginTop + (PlotHeight * i / numGridLines);
            sb.AppendLine($"  <line x1=\"{MarginLeft}\" y1=\"{y}\" x2=\"{PlotRight}\" y2=\"{y}\" stroke=\"#e0e0e0\" stroke-width=\"1\"/>");
         }

         for (int i = 0; i <= numGridLines; i++)
         {
            var x = MarginLeft + (PlotWidth * i / numGridLines);
            sb.AppendLine($"  <line x1=\"{x}\" y1=\"{MarginTop}\" x2=\"{x}\" y2=\"{PlotBottom}\" stroke=\"#e0e0e0\" stroke-width=\"1\"/>");
         }
      }

      private void appendAxes(StringBuilder sb, float minX, float maxX, float minY, float maxY, ChartData chartData)
      {
         sb.AppendLine($"  <line x1=\"{MarginLeft}\" y1=\"{PlotBottom}\" x2=\"{PlotRight}\" y2=\"{PlotBottom}\" stroke=\"#333\" stroke-width=\"2\"/>");
         sb.AppendLine($"  <line x1=\"{MarginLeft}\" y1=\"{MarginTop}\" x2=\"{MarginLeft}\" y2=\"{PlotBottom}\" stroke=\"#333\" stroke-width=\"2\"/>");

         var numTicks = 5;
         for (int i = 0; i <= numTicks; i++)
         {
            var xVal = minX + (maxX - minX) * i / numTicks;
            var x = MarginLeft + (PlotWidth * i / numTicks);
            sb.AppendLine($"  <text x=\"{x}\" y=\"{PlotBottom + 20}\" text-anchor=\"middle\" font-size=\"11\" fill=\"#333\">{formatNumber(xVal)}</text>");
         }

         for (int i = 0; i <= numTicks; i++)
         {
            var yVal = maxY - (maxY - minY) * i / numTicks;
            var y = MarginTop + (PlotHeight * i / numTicks);
            var displayVal = chartData.UseLogScale ? Math.Pow(10, yVal) : yVal;
            sb.AppendLine($"  <text x=\"{MarginLeft - 10}\" y=\"{y + 4}\" text-anchor=\"end\" font-size=\"11\" fill=\"#333\">{formatNumber((float)displayVal)}</text>");
         }

         sb.AppendLine($"  <text x=\"{MarginLeft + PlotWidth / 2}\" y=\"{PlotBottom + 45}\" text-anchor=\"middle\" font-size=\"12\" fill=\"#333\">{escapeXml(chartData.XAxisLabel)}</text>");

         sb.AppendLine($"  <text x=\"{MarginLeft - 55}\" y=\"{MarginTop + PlotHeight / 2}\" text-anchor=\"middle\" font-size=\"12\" fill=\"#333\" transform=\"rotate(-90, {MarginLeft - 55}, {MarginTop + PlotHeight / 2})\">{escapeXml(chartData.YAxisLabel)}</text>");
      }

      private void appendCurve(StringBuilder sb, CurveData curve, float minX, float maxX, float minY, float maxY, bool useLogScale)
      {
         if (curve?.XValues == null || curve.YValues == null || curve.XValues.Length == 0)
            return;

         var points = new StringBuilder();
         var validPoints = 0;

         for (int i = 0; i < Math.Min(curve.XValues.Length, curve.YValues.Length); i++)
         {
            var xVal = curve.XValues[i];
            var yVal = curve.YValues[i];

            if (float.IsNaN(xVal) || float.IsNaN(yVal) || float.IsInfinity(xVal) || float.IsInfinity(yVal))
               continue;

            if (useLogScale && yVal <= 0)
               continue;

            var transformedY = useLogScale ? (float)Math.Log10(yVal) : yVal;

            var x = MarginLeft + (xVal - minX) / (maxX - minX) * PlotWidth;
            var y = PlotBottom - (transformedY - minY) / (maxY - minY) * PlotHeight;

            x = Math.Max(MarginLeft, Math.Min(PlotRight, x));
            y = Math.Max(MarginTop, Math.Min(PlotBottom, y));

            if (validPoints > 0) points.Append(" ");
            points.Append($"{x.ToString("F1", CultureInfo.InvariantCulture)},{y.ToString("F1", CultureInfo.InvariantCulture)}");
            validPoints++;
         }

         if (validPoints > 1)
         {
            var colorHex = colorToHex(curve.Color);
            sb.AppendLine($"  <polyline fill=\"none\" stroke=\"{colorHex}\" stroke-width=\"2\" points=\"{points}\"/>");
         }
      }

      private void appendLegend(StringBuilder sb, ChartData chartData)
      {
         var legendX = PlotRight + 10;
         var legendY = MarginTop + 20;

         sb.AppendLine($"  <rect x=\"{legendX}\" y=\"{legendY - 15}\" width=\"120\" height=\"50\" fill=\"white\" stroke=\"#ccc\" stroke-width=\"1\" rx=\"3\"/>");

         var color1 = colorToHex(chartData.Curve1.Color);
         sb.AppendLine($"  <line x1=\"{legendX + 5}\" y1=\"{legendY}\" x2=\"{legendX + 25}\" y2=\"{legendY}\" stroke=\"{color1}\" stroke-width=\"2\"/>");
         sb.AppendLine($"  <text x=\"{legendX + 30}\" y=\"{legendY + 4}\" font-size=\"11\" fill=\"#333\">{escapeXml(truncate(chartData.Curve1.Name, 12))}</text>");

         var color2 = colorToHex(chartData.Curve2.Color);
         sb.AppendLine($"  <line x1=\"{legendX + 5}\" y1=\"{legendY + 20}\" x2=\"{legendX + 25}\" y2=\"{legendY + 20}\" stroke=\"{color2}\" stroke-width=\"2\"/>");
         sb.AppendLine($"  <text x=\"{legendX + 30}\" y=\"{legendY + 24}\" font-size=\"11\" fill=\"#333\">{escapeXml(truncate(chartData.Curve2.Name, 12))}</text>");
      }

      private void appendTitle(StringBuilder sb, string title, bool useLogScale)
      {
         var scaleIndicator = useLogScale ? " (Log Scale)" : " (Linear Scale)";
         var displayTitle = truncate(title, 60) + scaleIndicator;
         sb.AppendLine($"  <text x=\"{Width / 2}\" y=\"20\" text-anchor=\"middle\" font-size=\"14\" font-weight=\"bold\" fill=\"#333\">{escapeXml(displayTitle)}</text>");
      }

      private string formatNumber(float value)
      {
         var absValue = Math.Abs(value);
         if (absValue == 0) return "0";
         if (absValue >= 1000000) return (value / 1000000).ToString("0.##", CultureInfo.InvariantCulture) + "M";
         if (absValue >= 1000) return (value / 1000).ToString("0.##", CultureInfo.InvariantCulture) + "K";
         if (absValue >= 1) return value.ToString("0.##", CultureInfo.InvariantCulture);
         if (absValue >= 0.01) return value.ToString("0.###", CultureInfo.InvariantCulture);
         return value.ToString("0.##E+0", CultureInfo.InvariantCulture);
      }

      private string colorToHex(Color color)
      {
         return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
      }

      private string escapeXml(string text)
      {
         if (string.IsNullOrEmpty(text)) return "";
         return text
            .Replace("&", "&amp;")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;")
            .Replace("\"", "&quot;")
            .Replace("'", "&apos;");
      }

      private string truncate(string text, int maxLength)
      {
         if (string.IsNullOrEmpty(text)) return "";
         return text.Length <= maxLength ? text : text.Substring(0, maxLength - 3) + "...";
      }
   }
}
