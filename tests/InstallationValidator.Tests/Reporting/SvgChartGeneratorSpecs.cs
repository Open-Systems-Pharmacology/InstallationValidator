using System.Drawing;
using InstallationValidator.Core.Reporting.Charts;
using NUnit.Framework;
using OSPSuite.BDDHelper;

namespace InstallationValidator.Reporting
{
   public abstract class concern_for_SvgChartGenerator : ContextSpecification<ISvgChartGenerator>
   {
      protected ChartData _chartData;
      protected string _result;

      protected override void Context()
      {
         sut = new SvgChartGenerator();
         _chartData = createTestChartData();
      }

      protected ChartData createTestChartData(bool useLogScale = false)
      {
         return new ChartData
         {
            Title = "Test Chart",
            XAxisLabel = "Time [h]",
            YAxisLabel = "Concentration [mg/L]",
            UseLogScale = useLogScale,
            Curve1 = new CurveData
            {
               Name = "Reference",
               XValues = new[] { 0f, 1f, 2f, 3f, 4f },
               YValues = new[] { 1f, 2f, 4f, 8f, 16f },
               Color = Color.CornflowerBlue
            },
            Curve2 = new CurveData
            {
               Name = "Local",
               XValues = new[] { 0f, 1f, 2f, 3f, 4f },
               YValues = new[] { 1f, 2.1f, 4.2f, 8.5f, 17f },
               Color = Color.OrangeRed
            }
         };
      }
   }

   public class When_generating_a_linear_scale_chart : concern_for_SvgChartGenerator
   {
      protected override void Because()
      {
         _result = sut.GenerateLineChart(_chartData);
      }

      [Observation]
      public void should_return_valid_svg()
      {
         StringAssert.Contains("<svg", _result);
         StringAssert.Contains("</svg>", _result);
      }

      [Observation]
      public void should_include_chart_title()
      {
         StringAssert.Contains("Test Chart", _result);
         StringAssert.Contains("Linear Scale", _result);
      }

      [Observation]
      public void should_include_both_curves()
      {
         StringAssert.Contains("<polyline", _result);
         StringAssert.Contains("6495ED", _result); // CornflowerBlue hex
         StringAssert.Contains("FF4500", _result); // OrangeRed hex
      }

      [Observation]
      public void should_include_axis_labels()
      {
         StringAssert.Contains("Time [h]", _result);
         StringAssert.Contains("[mg/L]", _result);
      }

      [Observation]
      public void should_include_legend()
      {
         StringAssert.Contains("Reference", _result);
         StringAssert.Contains("Local", _result);
      }
   }

   public class When_generating_a_log_scale_chart : concern_for_SvgChartGenerator
   {
      protected override void Context()
      {
         base.Context();
         _chartData = createTestChartData(useLogScale: true);
      }

      protected override void Because()
      {
         _result = sut.GenerateLineChart(_chartData);
      }

      [Observation]
      public void should_return_valid_svg()
      {
         StringAssert.Contains("<svg", _result);
         StringAssert.Contains("</svg>", _result);
      }

      [Observation]
      public void should_indicate_log_scale_in_title()
      {
         StringAssert.Contains("Log Scale", _result);
      }
   }

   public class When_generating_chart_with_empty_data : concern_for_SvgChartGenerator
   {
      protected override void Context()
      {
         base.Context();
         _chartData.Curve1.XValues = new float[0];
         _chartData.Curve1.YValues = new float[0];
         _chartData.Curve2.XValues = new float[0];
         _chartData.Curve2.YValues = new float[0];
      }

      protected override void Because()
      {
         _result = sut.GenerateLineChart(_chartData);
      }

      [Observation]
      public void should_return_valid_svg_without_errors()
      {
         StringAssert.Contains("<svg", _result);
         StringAssert.Contains("</svg>", _result);
      }
   }

   public class When_generating_chart_with_nan_values : concern_for_SvgChartGenerator
   {
      protected override void Context()
      {
         base.Context();
         _chartData.Curve1.YValues = new[] { 1f, float.NaN, 4f, 8f, 16f };
      }

      protected override void Because()
      {
         _result = sut.GenerateLineChart(_chartData);
      }

      [Observation]
      public void should_handle_nan_values_gracefully()
      {
         StringAssert.Contains("<svg", _result);
         StringAssert.Contains("</svg>", _result);
         StringAssert.DoesNotContain("NaN", _result);
      }
   }

   public class When_generating_log_scale_chart_with_zero_values : concern_for_SvgChartGenerator
   {
      protected override void Context()
      {
         base.Context();
         _chartData = createTestChartData(useLogScale: true);
         _chartData.Curve1.YValues = new[] { 0f, 1f, 2f, 3f, 4f };
      }

      protected override void Because()
      {
         _result = sut.GenerateLineChart(_chartData);
      }

      [Observation]
      public void should_skip_zero_values_for_log_scale()
      {
         StringAssert.Contains("<svg", _result);
         StringAssert.Contains("</svg>", _result);
      }
   }
}
