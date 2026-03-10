using System;
using System.Linq;
using InstallationValidator.Core.Domain;
using InstallationValidator.Core.Reporting.Charts;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Color = System.Drawing.Color;
using IContainer = QuestPDF.Infrastructure.IContainer;

namespace InstallationValidator.Core.Reporting.Pdf
{
   public class PdfReportDocument : IDocument
   {
      private readonly InstallationValidationResult _validationResult;
      private readonly BatchComparisonResult _comparisonResult;
      private readonly string _title;
      private readonly string _subtitle;
      private readonly ISvgChartGenerator _svgChartGenerator;
      private readonly DoubleFormatter _doubleFormatter = new DoubleFormatter();

      public PdfReportDocument(InstallationValidationResult validationResult, string title, string subtitle, ISvgChartGenerator svgChartGenerator)
      {
         _validationResult = validationResult;
         _comparisonResult = validationResult?.ComparisonResult;
         _title = title;
         _subtitle = subtitle;
         _svgChartGenerator = svgChartGenerator;
      }

      public PdfReportDocument(BatchComparisonResult comparisonResult, string title, string subtitle, ISvgChartGenerator svgChartGenerator)
      {
         _comparisonResult = comparisonResult;
         _validationResult = null;
         _title = title;
         _subtitle = subtitle;
         _svgChartGenerator = svgChartGenerator;
      }

      public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
      public DocumentSettings GetSettings() => DocumentSettings.Default;

      public void Compose(IDocumentContainer container)
      {
         container.Page(page =>
         {
            page.Size(PageSizes.A4);
            page.Margin(40);
            page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

            page.Header().Element(composeHeader);
            page.Content().Element(composeContent);
            page.Footer().Element(composeFooter);
         });
      }

      private void composeHeader(IContainer container)
      {
         container.Column(column =>
         {
            column.Item().Text(_title).Bold().FontSize(18).FontColor(Colors.Blue.Darken2);
            column.Item().Text(_subtitle).FontSize(14).FontColor(Colors.Grey.Darken1);
            column.Item().PaddingTop(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
         });
      }

      private void composeFooter(IContainer container)
      {
         container.AlignCenter().Text(text =>
         {
            text.Span("Page ");
            text.CurrentPageNumber();
            text.Span(" of ");
            text.TotalPages();
         });
      }

      private void composeContent(IContainer container)
      {
         container.PaddingVertical(20).Column(column =>
         {
            column.Spacing(15);

            if (_validationResult != null)
            {
               composeInstallationValidationResult(column);
            }
            else if (_comparisonResult != null)
            {
               composeBatchComparisonResult(column);
            }
         });
      }

      private void composeInstallationValidationResult(ColumnDescriptor column)
      {
         column.Item().Text(Assets.Reporting.InstallationValidationResults).Bold().FontSize(14);

         column.Item().Text(Assets.Reporting.OverallValidationResult).Bold().FontSize(12);
         composeValidationState(column, _validationResult.State);

         composeRunSummary(column, _validationResult.RunSummary);
         composeBatchComparisonResult(column);
      }

      private void composeRunSummary(ColumnDescriptor column, ValidationRunSummary summary)
      {
         column.Item().PaddingTop(10).Text(Assets.Reporting.ValidationSummary).Bold().FontSize(12);

         var timeSpent = summary.EndTime - summary.StartTime;
         column.Item().Text(text =>
         {
            text.Span(Assets.Reporting.BatchRunDuration + ": ").Bold();
            text.Span($"Start time: {summary.StartTime.ToIsoFormat()}");
         });
         column.Item().Text($"End time: {summary.EndTime.ToIsoFormat()}");
         column.Item().Text($"Validation performed in {timeSpent.ToDisplay()}");

         column.Item().Text(text =>
         {
            text.Span(Assets.Reporting.InputConfigurationFolder + ": ").Bold();
            text.Span(summary.InputFolder);
         });

         column.Item().Text(text =>
         {
            text.Span(Assets.Reporting.BatchOutputFolder + ": ").Bold();
            text.Span(summary.OutputFolder);
         });

         column.Item().Text(text =>
         {
            text.Span(Assets.Reporting.ApplicationVersions + ": ").Bold();
            text.Span($"PK-Sim {summary.PKSimVersion}, MoBi {summary.MoBiVersion}");
         });

         column.Item().Text(text =>
         {
            text.Span(Assets.Reporting.LanguageSettings + ": ").Bold();
            text.Span($"{summary.CultureInfo.EnglishName} ({summary.CultureInfo.Name})");
         });

         composeOperatingSystemInfo(column, summary.OperatingSystem);
      }

      private void composeOperatingSystemInfo(ColumnDescriptor column, OperatingSystemInfo os)
      {
         column.Item().PaddingTop(5).Text(Assets.Reporting.OperatingSystem).Bold();
         column.Item().PaddingLeft(10).Column(osCol =>
         {
            osCol.Item().Text($"{Assets.Reporting.ComputerName}: {os.ComputerName}");
            osCol.Item().Text($"OS: {os.FriendlyName}");
            osCol.Item().Text($"{Assets.Reporting.Architecture}: {os.Architecture}");
            osCol.Item().Text($"{Assets.Reporting.RunningOnVirtualMachine}: {(os.IsRunningOnVirtualMachine ? "Yes" : "No")}");
            osCol.Item().Text($"{Assets.Reporting.RunningOnTerminalSession}: {(os.IsRunningOnTerminalSession ? "Yes" : "No")}");
         });
      }

      private void composeBatchComparisonResult(ColumnDescriptor column)
      {
         if (_comparisonResult == null) return;

         column.Item().PaddingTop(10).Text(Assets.Reporting.BatchComparisonResults).Bold().FontSize(12);

         column.Item().Text(text =>
         {
            text.Span(Assets.Reporting.ComparisonFolder(_comparisonResult.FolderPathCaption1) + ": ").Bold();
            text.Span(_comparisonResult.FolderPath1);
         });

         column.Item().Text(text =>
         {
            text.Span(Assets.Reporting.ComparisonFolder(_comparisonResult.FolderPathCaption2) + ": ").Bold();
            text.Span(_comparisonResult.FolderPath2);
         });

         if (_comparisonResult.ComparisonSettings.Exclusions.Any())
         {
            column.Item().Text(Assets.Reporting.UsingExclusions + ":").Bold();
            foreach (var exclusion in _comparisonResult.ComparisonSettings.Exclusions)
            {
               column.Item().PaddingLeft(10).Text($"• {exclusion}");
            }
         }

         composeSimulationsByState(column, ValidationState.Invalid, Assets.Reporting.InvalidSimulations);
         composeSimulationsByState(column, ValidationState.ValidWithWarnings, Assets.Reporting.ValidWithWarningSimulations);
         composeSimulationsByState(column, ValidationState.Valid, Assets.Reporting.ValidSimulations);
      }

      private void composeSimulationsByState(ColumnDescriptor column, ValidationState state, Func<int, int, string> sectionName)
      {
         var simulations = _comparisonResult.FileComparisonResults
            .Where(x => x.Is(state))
            .OrderBy(x => x.FileName)
            .ToList();

         if (!simulations.Any()) return;

         column.Item().PaddingTop(10).Text(sectionName(simulations.Count, _comparisonResult.FileComparisonResults.Count)).Bold().FontSize(11);

         foreach (var sim in simulations)
         {
            composeFileComparisonResult(column, sim);
         }
      }

      private void composeFileComparisonResult(ColumnDescriptor column, FileComparisonResult result)
      {
         column.Item().PaddingTop(5).PaddingLeft(10).Column(simCol =>
         {
            simCol.Item().Text($"{Assets.Reporting.Simulation}: {FileHelper.FileNameFromFileFullPath(result.FileName)}").Bold();

            simCol.Item().Row(row =>
            {
               row.AutoItem().Text(Assets.Reporting.ValidationResult);
               row.AutoItem().Text(result.State.ToString()).FontColor(toQuestColor(result.State.ValidationColor()));
            });

            if (result is OutputFileComparisonResult outputResult)
            {
               composeOutputFileResult(simCol, outputResult);
            }
            else if (result is MissingFileComparisonResult missingResult)
            {
               composeMissingFileResult(simCol, missingResult);
            }
         });
      }

      private void composeMissingFileResult(ColumnDescriptor column, MissingFileComparisonResult result)
      {
         column.Item().PaddingTop(5).Text(Assets.Reporting.MissingFileValidation).Bold();
         column.Item().Text($"{result.FileName} was contained in folder:");
         column.Item().PaddingLeft(10).Text(result.FolderContainingFile);
         column.Item().Text("but was missing in folder:");
         column.Item().PaddingLeft(10).Text(result.FolderWithoutFile);
      }

      private void composeOutputFileResult(ColumnDescriptor column, OutputFileComparisonResult result)
      {
         if (!result.IsValid())
         {
            column.Item().Text(Assets.Reporting.AbsoluteToleranceIs(_doubleFormatter.Format(result.AbsTol)));
            column.Item().Text(Assets.Reporting.RelativeToleranceIs(_doubleFormatter.Format(result.RelTol)));
         }

         if (!result.TimeComparison.IsValid())
         {
            column.Item().PaddingTop(5).Text(Assets.Reporting.TimeComparisonValidation).Bold();
            column.Item().Text(result.TimeComparison.Message);
            column.Item().Text($"{Assets.Reporting.Deviation}: {_doubleFormatter.Format(result.TimeComparison.Deviation)}");
         }

         var invalidOutputs = result.OutputComparisonResults.Where(x => !x.IsValid());
         invalidOutputs.Each(x => composeOutputComparison(column, x));

         var validOutputsWithData = result.OutputComparisonResults.Where(x => x.IsValid()).Where(x => x.HasData);
         validOutputsWithData.Each(x => composeOutputComparison(column, x));
      }

      private void composeOutputComparison(ColumnDescriptor column, OutputComparisonResult output)
      {
         column.Item().PaddingTop(5).Text($"{Assets.Reporting.OutputPath}: {output.Path}").Bold();
         column.Item().Text(output.Message);
         column.Item().Text($"{Assets.Reporting.Deviation}: {_doubleFormatter.Format(output.Deviation)}");

         if (output.HasData)
         {
            var logChart = createChartData(output, useLogScale: true);
            var logSvg = _svgChartGenerator.GenerateLineChart(logChart);
            column.Item().PaddingTop(5).Svg(logSvg).FitWidth();

            if (!output.IsValid())
            {
               var linearChart = createChartData(output, useLogScale: false);
               var linearSvg = _svgChartGenerator.GenerateLineChart(linearChart);
               column.Item().PaddingTop(5).Svg(linearSvg).FitWidth();
            }
         }
      }

      private void composeValidationState(ColumnDescriptor column, ValidationState state)
      {
         column.Item().Text(state.ToString()).FontColor(toQuestColor(state.ValidationColor())).Bold();
      }

      private string toQuestColor(Color color)
      {
         return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
      }

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
