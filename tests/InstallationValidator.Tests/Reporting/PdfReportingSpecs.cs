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
using OSPSuite.Utility.Container;
using QuestPDF.Infrastructure;

namespace InstallationValidator.Reporting
{
   [Category("Reporting")]
   public abstract class concern_for_PdfReporting : ContextForIntegration<IPdfReportingTask>
   {
      protected DirectoryInfo _reportsDir;
      protected ComparisonSettings _comparisonSettings;

      public override void GlobalContext()
      {
         base.GlobalContext();
         sut ??= IoC.Resolve<IPdfReportingTask>();
      }

      public override void Cleanup()
      {
         base.Cleanup();
         if (_reportsDir?.Exists == true)
         {
            foreach (var file in Directory.GetFiles(_reportsDir.FullName, "*.pdf"))
            {
               try { File.Delete(file); } catch { }
            }
         }
      }

      protected override void Context()
      {
         QuestPDF.Settings.License = LicenseType.Community;

         _reportsDir = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports"));
         if (!_reportsDir.Exists)
            _reportsDir.Create();

         foreach (var file in Directory.GetFiles(_reportsDir.FullName, "*.pdf"))
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

   public class When_creating_a_pdf_report_for_installation_validation : concern_for_PdfReporting
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
         _reportPath = Directory.GetFiles(_reportsDir.FullName, "*.pdf").FirstOrDefault();
      }

      [Observation]
      public void should_create_pdf_file()
      {
         _reportPath.ShouldNotBeNull();
         FileHelper.FileExists(_reportPath).ShouldBeTrue();
      }

      [Observation]
      public void should_create_non_empty_pdf_file()
      {
         new FileInfo(_reportPath).Length.ShouldBeGreaterThan(0);
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

   public class When_creating_a_pdf_report_with_missing_file_results : concern_for_PdfReporting
   {
      private InstallationValidationResult _installationValidationResult;
      private string _reportPath;

      protected override void Context()
      {
         base.Context();
         var batchComparisonResult = new BatchComparisonResult();

         var missingFileResult = new MissingFileComparisonResult("missing_sim.pkml", @"C:\folder1\output", @"C:\folder2\output");

         batchComparisonResult.AddFileComparisons(new FileComparisonResult[]
         {
            missingFileResult
         });

         var batchRunSummary = new ValidationRunSummary
         {
            OutputFolder = _reportsDir.FullName,
            InputFolder = "input configuration folder",
            EndTime = DateTime.Now,
            MoBiVersion = "12.0",
            PKSimVersion = "12.0",
            StartTime = DateTime.Now.AddMinutes(-2)
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
         _reportPath = Directory.GetFiles(_reportsDir.FullName, "*.pdf").FirstOrDefault();
      }

      [Observation]
      public void should_create_pdf_file()
      {
         _reportPath.ShouldNotBeNull();
         FileHelper.FileExists(_reportPath).ShouldBeTrue();
      }

      [Observation]
      public void should_create_non_empty_pdf_file()
      {
         new FileInfo(_reportPath).Length.ShouldBeGreaterThan(0);
      }
   }

   public class When_creating_a_pdf_report_for_batch_comparison : concern_for_PdfReporting
   {
      private BatchComparisonResult _batchComparisonResult;
      private string _reportPath;

      protected override void Context()
      {
         base.Context();
         _batchComparisonResult = new BatchComparisonResult();

         var outputFileComparisonResult = new OutputFileComparisonResult("sim1.pkml", "folder1", "folder2");
         outputFileComparisonResult.TimeComparison = new TimeComparisonResult(ValidationState.Valid, "valid");

         var validOutput = new OutputComparisonResult(
            new OutputComparisonResultParams("Organism|Liver|Concentration") { ValuesDimension = "Mass" },
            _comparisonSettings,
            ValidationState.Valid,
            "OK")
         {
            Deviation = 0.001,
            Output1 = new OutputResult(new[] { 0f, 1f, 2f }, new[] { 1f, 2f, 3f }) { Caption = "Old" },
            Output2 = new OutputResult(new[] { 0f, 1f, 2f }, new[] { 1f, 2f, 3f }) { Caption = "New" }
         };

         outputFileComparisonResult.AddOutputComparison(validOutput);
         _batchComparisonResult.AddFileComparisons(new FileComparisonResult[] { outputFileComparisonResult });
      }

      protected override void Because()
      {
         sut.CreateReport(_batchComparisonResult, _reportsDir.FullName, _reportsDir.FullName, false).Wait();
         _reportPath = Directory.GetFiles(_reportsDir.FullName, "*.pdf").FirstOrDefault();
      }

      [Observation]
      public void should_create_pdf_file()
      {
         _reportPath.ShouldNotBeNull();
         FileHelper.FileExists(_reportPath).ShouldBeTrue();
      }

      [Observation]
      public void should_create_non_empty_pdf_file()
      {
         new FileInfo(_reportPath).Length.ShouldBeGreaterThan(0);
      }
   }
}
