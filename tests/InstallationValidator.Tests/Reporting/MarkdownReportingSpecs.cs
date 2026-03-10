using System;
using System.IO;
using System.Linq;
using InstallationValidator.Core.Domain;
using InstallationValidator.Core.Services;
using InstallationValidator.IntegrationTests;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Utility;

namespace InstallationValidator.Reporting
{
   [Category("Reporting")]
   public abstract class concern_for_MarkdownReporting : ContextForIntegration<IMarkdownReportingTask>
   {
      protected DirectoryInfo _reportsDir;
      protected ComparisonSettings _comparisonSettings;

      public override void Cleanup()
      {
         base.Cleanup();
         if (_reportsDir?.Exists == true)
         {
            foreach (var file in Directory.GetFiles(_reportsDir.FullName, "*.md"))
            {
               try { File.Delete(file); } catch { }
            }
         }
      }

      protected override void Context()
      {
         _reportsDir = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports"));
         if (!_reportsDir.Exists)
            _reportsDir.Create();

         // Clean up any existing .md files before the test
         foreach (var file in Directory.GetFiles(_reportsDir.FullName, "*.md"))
         {
            try { File.Delete(file); } catch { }
         }

         _comparisonSettings = new ComparisonSettings
         {
            FolderPath1 = "F1",
            FolderPath2 = "F2",
         };
      }
   }

   public class When_creating_a_markdown_report : concern_for_MarkdownReporting
   {
      private InstallationValidationResult _installationValidationResult;
      private string _reportPath;

      protected override void Context()
      {
         base.Context();
         var batchComparisonResult = new BatchComparisonResult();

         var outputFileComparisonResult = new OutputFileComparisonResult("test_file.txt", "folder1", "folder2");
         var outputComparisonResult = createOutputComparisonResult();
         outputFileComparisonResult.AddOutputComparison(outputComparisonResult);
         outputFileComparisonResult.TimeComparison = new TimeComparisonResult(ValidationState.Valid, "valid");

         batchComparisonResult.AddFileComparisons(new FileComparisonResult[]
         {
            outputFileComparisonResult
         });

         var batchRunSummary = new ValidationRunSummary
         {
            OutputFolder = _reportsDir.FullName,
            InputFolder = "input configuration folder",
            EndTime = DateTime.Now,
            MoBiVersion = "12.0",
            PKSimVersion = "12.0",
            StartTime = DateTime.Now.AddMinutes(-5)
         };

         _installationValidationResult = new InstallationValidationResult
         {
            ComparisonResult = batchComparisonResult,
            RunSummary = batchRunSummary
         };
      }

      protected override void Because()
      {
         sut.CreateReport(_installationValidationResult, _reportsDir.FullName, false).Wait();
         _reportPath = Directory.GetFiles(_reportsDir.FullName, "*.md").FirstOrDefault();
      }

      [Observation]
      public void should_create_markdown_file()
      {
         _reportPath.ShouldNotBeNull();
         FileHelper.FileExists(_reportPath).ShouldBeTrue();
      }

      [Observation]
      public void should_contain_report_content()
      {
         var content = File.ReadAllText(_reportPath);
         StringAssert.Contains("Installation Validation", content);
      }

      [Observation]
      public void should_contain_svg_chart()
      {
         var content = File.ReadAllText(_reportPath);
         StringAssert.Contains("<svg", content);
      }

      [Observation]
      public void should_contain_validation_state()
      {
         var content = File.ReadAllText(_reportPath);
         StringAssert.Contains("Invalid", content);
      }

      [Observation]
      public void should_contain_deviation_section()
      {
         var content = File.ReadAllText(_reportPath);
         StringAssert.Contains("Deviation", content);
      }

      private OutputComparisonResult createOutputComparisonResult()
      {
         return new OutputComparisonResult(
            new OutputComparisonResultParams("Organism|Brain|Concentration") { ValuesDimension = "Mass" },
            _comparisonSettings,
            ValidationState.Invalid,
            "Deviation exceeds tolerance")
         {
            Deviation = 0.44,
            Output1 = new OutputResult(getTimes(), getValues(x => 2 * x)) { Caption = "Reference" },
            Output2 = new OutputResult(getTimes(), getValues(x => x)) { Caption = "Local" }
         };
      }

      private static float[] getValues(Func<float, float> transform)
      {
         return getTimes().Select(transform).ToArray();
      }

      private static float[] getTimes()
      {
         return new[] { 0.0f, 1.1f, 1.2f, 1.3f, 1.4f };
      }
   }
}
