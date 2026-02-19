using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using InstallationValidator.Core;
using InstallationValidator.Core.Domain;
using InstallationValidator.Core.Reporting.Charts;
using InstallationValidator.Core.Reporting.Markdown;
using InstallationValidator.Core.Services;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Utility;

namespace InstallationValidator.Reporting
{
   [Category("Reporting")]
   public abstract class concern_for_MarkdownReporting : ContextSpecification<IMarkdownReportingTask>
   {
      protected DirectoryInfo _reportsDir;
      protected ComparisonSettings _comparisonSettings;
      protected ISvgChartGenerator _svgChartGenerator;
      protected IMarkdownBuilderRepository _builderRepository;
      protected FakeValidationLogger _validationLogger;
      protected FakeConfiguration _configuration;

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

         _svgChartGenerator = new SvgChartGenerator();
         _validationLogger = new FakeValidationLogger();
         _configuration = new FakeConfiguration();

         // Use real MarkdownBuilderRepository with real builders
         // Note: InstallationValidationResultMarkdownBuilder needs the repository itself,
         // so we create the repository first with a temporary list, then add the builder
         var builders = new List<IMarkdownBuilder>();
         _builderRepository = new MarkdownBuilderRepository(builders);

         // Add all real builders
         builders.Add(new ValidationStateReportMarkdownBuilder());
         builders.Add(new ValidationRunSummaryMarkdownBuilder(_builderRepository));
         builders.Add(new OperatingSystemInfoMarkdownBuilder());
         builders.Add(new TimeComparisonResultMarkdownBuilder());
         builders.Add(new OutputComparisonResultMarkdownBuilder(_svgChartGenerator));
         builders.Add(new OutputFileComparisonResultMarkdownBuilder(_builderRepository));
         builders.Add(new BatchComparisonResultMarkdownBuilder(_builderRepository));
         builders.Add(new InstallationValidationResultMarkdownBuilder(_builderRepository));

         sut = new MarkdownReportingTask(_builderRepository, _validationLogger, _configuration);
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

   // Test helpers
   public class FakeValidationLogger : IValidationLogger
   {
      public void AppendLine(string line) { }
      public void AppendText(string text) { }
      public void AppendRawText(string text) { }
   }

   public class FakeConfiguration : IInstallationValidatorConfiguration
   {
      public string OSPSuiteNameWithVersion => "OSPSuite 12.0";
      public string PKSimPath => "";
      public string MoBiPath => "";
      public string DimensionFilePath { get; set; } = "";
      public string WatermarkTextOrPath => "";
      public string FullVersion => "12.0.0";
      public string MajorVersion => "12";
      public string IssueTrackerUrl => "";
      public string ProductName => "Installation Validator";
      public string ProductNameWithTrademark => "Installation Validator";
      public string UserSettingsFilePath => "";
      public string ProductDisplayName => "Installation Validator";
      public string ApplicationSettingsFilePath => "";
      public string CurrentUserFolderPath => "";
      public string AllUsersFolderPath => "";
      public string PKParameterFilePath => "";
      public string ChartLayoutTemplateFolderPath => "";
      public string BatchInputsFolderPath => "";
      public string BatchOutputsFolderPath => "";
      public string PKSimCLIPath => "";
      public string PKSimBinaryExecutablePath => "";
      public string MoBiBinaryExecutablePath => "";
      public string DefaultOutputPath => "";
      public string TeXTemplateFolderPath => "";
      public string PKParametersFilePath { get; set; } = "";
      public string LicenseAgreementFilePath => "";
      public string FullVersionDisplay => "12.0.0";
      public string Version => "12.0.0";
      public int InternalVersion => 1;
      public int Major => 12;
      public int Minor => 0;
      public int Build => 0;
      public string ReleaseDescription => "";
      public Origin Product => Origins.Other;
      public IEnumerable<string> UserSettingsFilePaths => Array.Empty<string>();
      public IEnumerable<string> ApplicationSettingsFilePaths => Array.Empty<string>();
      public string WatermarkOptionLocation => "";
      public string IconName => "";
   }

}
