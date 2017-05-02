using System;
using System.Drawing;
using System.Text;
using OSPSuite.Core.Domain;
using OSPSuite.Utility.Format;

namespace OSPSuite.InstallationValidator.Core.Assets
{
   public static class Captions
   {
      public static readonly string OutputFolder = "Specify the folder where validation outputs will be generated";
      public static readonly string SelectOutputFolder = "Select Output Folder";
      public static readonly string StartValidation = "Start Validation";
      public static readonly string StopValidation = "Stop Validation";
      public static string TheValidationWasCanceled = "The validation was canceled";
      public static readonly string ReallyCancelInstallationValidation = "Really cancel installation validation?";

      public static readonly string ValidationDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, " +
                                                            "sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. " +
                                                            "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi " +
                                                            "ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in " +
                                                            "voluptate velit esse cillum dolore eu fugiat nulla pariatur. " +
                                                            "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

      public static string MainViewTitle = "Open Systems Pharmacology Suite - Installation Validator";
      public const string New = "New";
      public const string Old = "Old";
   }

   public static class Logs
   {
      public static readonly string StartingBatchCalculation = InBold("Starting batch calculations...");
      public static readonly string StartingComparison = InBold("Starting output comparisons...");
      public static readonly string StartingReport = InBold("Starting report creation...");
      public static readonly string ValidationCompleted = InBold("Validation completed");
      public static readonly string Valid = InGreen("Valid");
      public static readonly string Invalid = InRed("Invalid");
      public static readonly string ValidWithWarnings = InOrange("Valid with warnings");

      public static string ComparingFilles(string file) => $"Comparing files '{file}'...";

      public static string InBold(string stringToFormat)
      {
         return InHtml(stringToFormat, "b");
      }

      public static string InHtml(string stringToFormat, string marker)
      {
         return $"<{marker}>{stringToFormat}</{marker}>";
      }

      public static string InGreen(string stringToFormat)
      {
         return InColor(stringToFormat, Color.Green);
      }

      public static string InRed(string stringToFormat)
      {
         return InColor(stringToFormat, Color.Red);
      }

      public static string InOrange(string stringToFormat)
      {
         return InColor(stringToFormat, Color.Orange);
      }

      public static string InColor(string stringToFormat, Color color)
      {
         return $"<span style='color:rgb({color.R},{color.G},{color.B})'>{stringToFormat}</span>";
      }

      public static string ReportCreatedUnder(string reportConfigurationReportFile)
      {
         return $"Installation report was created and saved under {InBold(reportConfigurationReportFile)}";
      }

      public static string StateDisplayFor(ValidationState validationState)
      {
         if (validationState == ValidationState.Invalid)
            return Invalid;

         if (validationState == ValidationState.ValidWithWarnings)
            return ValidWithWarnings;

         return Valid;
      }
   }

   public static class Exceptions
   {
      public static readonly string CopyToClipboard = "Copy to Clipboard";
      public const string OutputNotDefined = "Output not defined!";

      // TODO - change the implementation in Core and modify ExceptionView to use a RichEditControl instead of a label if the text does not display correctly
      public static string ExceptionViewDescription(string issueTrackerUrl)
      {
         var sb = new StringBuilder();
         appendLine("oops...something went terribly wrong.", sb);
         appendLine(string.Empty, sb);
         appendLine("To best address the error, please enter an issue in our issue tracker:", sb);
         sb.Append("<ol>");
         appendListItem($"Visit <b>{issueTrackerUrl}</b> or click on the link below", sb);
         appendListItem("Click on the <b>New Issue</b> button", sb);
         appendListItem("Describe the steps you took prior to the problem emerging", sb);
         appendListItem($"Copy the information below by using the <b>{CopyToClipboard}</b> button and paste it in the issue description", sb);
         appendListItem("if possible, attach your project file to the issue (do not attach confidential information)", sb);
         sb.Append("</ol>");
         appendLine(string.Empty, sb);
         appendLine("Note: A GitHub account is required to create an issue", sb);
         return sb.ToString();
      }

      private static void appendListItem(string listItem, StringBuilder sb)
      {
         sb.Append($"<li>{listItem}</li>");
      }

      private static void appendLine(string lineToAppend, StringBuilder sb)
      {
         sb.Append($"<p>{lineToAppend}</p>");
      }
   }

   public static class Validation
   {
      private static readonly NumericFormatter<double> _formatter = new NumericFormatter<double>(NumericFormatterOptions.Instance);

      public static string FolderDoesNotExist(string fileFullPath) => $"Folder '{fileFullPath}' does not exist";

      public static string ArraysHaveDifferentLength(int length1, int length2) => $"Array used in comparison have different lengths ({length1} vs {length2}";

      public static string TimeArraysHaveDifferentLength(string simulationName, int length1, int length2)
      {
         return $"Time arrays for simulation {simulationName} have different lengths ({length1} vs {length2}";
      }

      public static string ValueArraysHaveDifferentLength(string simulationName, string outputPath, int length1, int length2)
      {
         return $"Values arrays for output '{outputPath}' from simulation '{simulationName}' have different lengths ({length1} vs {length2}";
      }

      public static string TimeArrayDoesNotExist(string simulationName, string folder)
      {
         return $"Time array is undefined for simulation {simulationName} defined in '{folder}'";
      }

      public static string ValueArrayDoesNotExist(string simulationName, string folder, string outputPath)
      {
         return $"Value array is undefined for output '{outputPath}' from simulation '{simulationName}' defined in '{folder}'";
      }

      public static string DeviationForTimeGreaterThanMaxTolearance(double deviation, double maxDeviationTime)
      {
         return DeviationForVariableGreaterThanMaxTolerance("Time", deviation, maxDeviationTime);
      }

      public static string DeviationForVariableGreaterThanMaxTolerance(string variable, double deviation, double maxDeviation)
      {
         return $"Deviation for '{variable}' is {_formatter.Format(deviation * 100)}% and is greater than the allowed max. tolerance of {_formatter.Format(maxDeviation * 100)}%";
      }

      public static string OutputIsMissingFromSimulation(string outputPath, string simulationName, string folder)
      {
         return $"Output '{outputPath}' is missing from simulation '{simulationName}' defined in '{folder}'";
      }
   }

   public static class Reporting
   {
      public static readonly string ValidationReport = "Validation Report";
      public static readonly string InstallationValidationResults = "Installation Validation Results";
      public static readonly string BatchRunSummary = "Batch Run Summary";
      public static readonly string BatchComparisonResults = "Batch Comparison Results";
      public static readonly string RunSummary = "Summary of Batch Run";
      public static readonly string DateAndTime = "Date and Time";

      public static readonly string BatchOutputFolder = "Batch Output Location";
      public static readonly string ComputerName = "Computer Name";
      public static readonly string OperatingSystem = "Operating System";
      public static readonly string ApplicationVersions = "Application Versions";
      public static readonly string ComparisonFolders = "Folders for Comparison";
      public static readonly string OverallComparisonResult = "Overall Comparison Result";
      public static readonly string FailedValidations = "Failed Validations";
      public static readonly string InputConfigurationFolder = "Input Configuration Folder";
      public static readonly string BatchRunDuration = "Batch Run Duration";

      public static string InstallationValidationPerformedIn(string startTime, string endTime, string delay)
      {
         return $"Start time: {startTime}{Environment.NewLine}End time: {endTime}{Environment.NewLine}Validation performed in {delay}";
      }

      public static readonly string Deviation = "Deviation";
      public static readonly string OutputPath = "Output Path";
      public static readonly string TimeComparisonValidation = "Time Comparison Validation";
      public static readonly string OutputComparisonValidation = "Output Comparison Validation";
      public static readonly string MissingFileValidation = "Missing File Validation";
   }
}