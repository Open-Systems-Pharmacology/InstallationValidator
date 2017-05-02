﻿using System;
using System.IO;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Reporting;
using OSPSuite.Core.Services;
using OSPSuite.InstallationValidator.Core.Domain;
using OSPSuite.TeXReporting;
using OSPSuite.Utility;

namespace OSPSuite.InstallationValidator.IntegrationTests
{
   [Category("Reporting")]
   public abstract class concern_for_Reporting : ContextForIntegration<IReportingTask>
   {
      private DirectoryInfo _reportsDir;
      private ReportConfiguration _reportConfiguration;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _reportsDir = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports"));
         if (!_reportsDir.Exists)
            _reportsDir.Create();

         _reportConfiguration = new ReportConfiguration
         {
            Title = "Testing Reports",
            Author = "Unit Tests Engine",
            Keywords = new[] { "Tests", "PKReporting", "SBSuite" },
            Software = "SBSuite",
            SubTitle = "SubTitle",
            SoftwareVersion = "5.2",
            ContentFileName = "Content",
            DeleteWorkingDir = true,
            ColorStyle = ReportSettings.ReportColorStyles.Color,
            Template = new ReportTemplate { Path = TEXTemplateFolder() }
         };
      }

      public static string TEXTemplateFolder()
      {
         return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..\\Templates", "StandardTemplate");
      }

      public void CreateReportAndValidate(object objectToReport, string reportName)
      {
         if (objectToReport == null) return;
         _reportConfiguration.ReportFile = Path.Combine(_reportsDir.FullName, $"{reportName}.pdf");
         _reportConfiguration.SubTitle = reportName;
         sut.CreateReportAsync(objectToReport, _reportConfiguration).Wait();
         FileHelper.FileExists(_reportConfiguration.ReportFile).ShouldBeTrue();
      }
   }

   public class When_creating_a_report : concern_for_Reporting
   {
      [Observation]
      public void should_have_created_a_valid_pdf_report_for_individual()
      {
         var batchComparisonResult = new BatchComparisonResult();

         var outputFileComparisonResult = new OutputFileComparisonResult("a file.txt", "a folder", "another folder");
         var timeFileComparisonResult = new OutputFileComparisonResult("a file.txt", "a folder", "another folder");
         var outputComparisonResult = new OutputComparisonResult("the path", ValidationState.Invalid, "the message") { Deviation = 44.0 };
         var timeComparisonResult = new TimeComparisonResult(ValidationState.Invalid, "the time message") { Deviation = 1.0 };
         timeFileComparisonResult.TimeComparison = timeComparisonResult;
         timeFileComparisonResult.AddOutputComparison(new OutputComparisonResult("valid", ValidationState.Valid, ""));
         outputFileComparisonResult.AddOutputComparison(outputComparisonResult);
         outputFileComparisonResult.TimeComparison = new TimeComparisonResult(ValidationState.Valid, "valid");


         batchComparisonResult.AddFileComparisons(new FileComparisonResult[]
         {
            new MissingFileComparisonResult("a missing file.txt", "a folder", "another folder"),
            outputFileComparisonResult,
            timeFileComparisonResult
         });
         var batchRunSummary = new BatchRunSummary
         {
            BatchOutputFolder = "a folder",
            ComputerName = "a computer",
            ConfigurationInputFolder = "input configuration folder",
            EndTime = DateTime.Now,
            MoBiVersion = "1",
            OperatingSystem = Environment.OSVersion,
            PKSimVersion = "1",
            StartTime = DateTime.Now
         };
         var installationValidationResult = new InstallationValidationResult
         {
            ComparisonResult = batchComparisonResult,
            RunSummary = batchRunSummary
         };
         CreateReportAndValidate(installationValidationResult, "validationReport");
      }
   }
}
