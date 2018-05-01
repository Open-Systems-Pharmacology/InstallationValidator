using System;
using System.Drawing;
using System.Text;
using InstallationValidator.Core.Domain;
using OSPSuite.Core.Domain;

namespace InstallationValidator.Core.Assets
{
   public static class Captions
   {
      public static readonly string OutputFolder = "Specify the folder where validation outputs will be generated";
      public static readonly string SelectOutputFolder = "Select Output Folder";
      public static readonly string StartValidation = "Start Validation";
      public static readonly string StopValidation = "Stop Validation";
      public static string TheValidationWasCanceled = "The validation was canceled";
      public static readonly string ReallyCancelInstallationValidation = "Really cancel installation validation?";

      public static string ValidationDescription
      {
         get
         {
            var sb = new StringBuilder();
            sb.AppendLine($"A validation of the <b>{OSPSuite.Core.Domain.Constants.SUITE_NAME}</b> installation is performed to ensure that the software works fully as intended when installed in the computing environment.");
            sb.AppendLine();
            sb.AppendLine("The key functionalities are tested through comparison of reference simulation outputs with locally calculated outputs for a set of predefined test simulations.");
            sb.AppendLine();
            sb.AppendLine("The validation report will include:");
            sb.AppendLine("  - overall validation result (<b>Valid/ Invalid</b>)");
            sb.AppendLine("  - validation result for every test simulation (<b>Valid/ Invalid</b>)");
            sb.AppendLine("  - summary of the deviations for each <b>Invalid</b> simulation (numerical values and comparison plots)");
            sb.AppendLine("  - selected comparison plots for each <b>Valid</b> simulation");
            sb.AppendLine("  - information about installed software versions and local computing environment");
            return sb.ToString();
         }
      }

      public static readonly string StopComparison = "Stop Comparison";
      public static readonly string StartComparison = "Start Comparison";
      public static readonly string TheComparisonWasCanceled = "The comparison was canceled";

      public static string SimulationComparisonViewTitle = $"{OSPSuite.Core.Domain.Constants.SUITE_NAME} - Simulation Output Comparer";
      public static string MainViewTitle = $"{OSPSuite.Core.Domain.Constants.SUITE_NAME} - Installation Validator";
      public static readonly string Installation = "Installation";
      public static readonly string Computed = "Computed";

      public const string DefaultCaptionFolder1 = "Old";
      public const string DefaultCaptionFolder2 = "New";
      public static readonly string ComparisonFolder1 = "Old Results";
      public static readonly string ComparisonFolder2 = "New Results";
      public static readonly string ReallyCancelFolderComparison = "Really cancel folder comparison?";
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
      public static readonly string OverallComparisonResult = InBold("Overall Comparison Result:");
      public static readonly string NumberOfComparedFiles = InBold("Number of compared files:");
      public static readonly string ComparisonCompleted = InBold("Comparison completed");

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
         return InBold(InColor(validationState.ToString(), validationState.ValidationColor()));
      }
   }

   public static class Exceptions
   {
      public static readonly string CopyToClipboard = "Copy to Clipboard";
      public const string OutputNotDefined = "Output not defined!";
   }

   public static class Validation
   {
      private static readonly DoubleFormatter _formatter = new DoubleFormatter();

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

      public static string FolderPathTooLong(string folderPath, int maximumFolderPathLength)
      {
         return $"The folder path '{folderPath}' is too long. It must be less than or equal to {maximumFolderPathLength} characters";
      }
   }

   public static class Reporting
   {
      public static readonly string InstallationValidation = "Installation Validation";
      public static readonly string InstallationValidationResults = "Installation Validation Results";
      public static readonly string BatchComparisonResults = "Comparison Results";
      public static readonly string ValidationSummary = "Validation Summary";
      public static readonly string DateAndTime = "Date and Time";
      public static readonly string BatchOutputFolder = "Local Outputs Location";
      public static readonly string ComputerName = "Computer Name";
      public static readonly string OperatingSystem = "Operating System";
      public static readonly string Architecture = "Architecture";
      public static readonly string RunningOnTerminalSession = "Running on Terminal Session";
      public static readonly string RunningOnVirtualMachine = "Running on Virtual Machine";
      public static readonly string ApplicationVersions = "Application Versions";
      public static readonly string OverallComparisonResult = "Overall Comparison Result";
      public static readonly string NumberOfComparedFiles = "Number of Compared Files";
      public static readonly string OverallValidationResult = "Overall Validation Result";
      public static readonly string FailedValidations = "Failed Validations";
      public static readonly string InputConfigurationFolder = "Input Configuration Folder";
      public static readonly string BatchRunDuration = "Run Duration";
      public static readonly string ValidationResult = "Result of the validation: ";
      public static readonly string Simulation = "Simulation";
      public static readonly string Deviation = "Deviation";
      public static readonly string OutputPath = "Output Path";
      public static readonly string TimeComparisonValidation = "Time Comparison Validation";
      public static readonly string OutputComparisonValidation = "Output Comparison Validation";
      public static readonly string MissingFileValidation = "Missing File Validation";
      public static readonly string FolderComparisonResults = "Folder Comparison Results";
      public static readonly string FolderComparison = "Folder Comparison";
      public static string InvalidSimulations(int number, int total) => numberOfSimulationsWithState(number, total, ValidationState.Invalid);
      public static string ValidWithWarningSimulations(int number, int total) => numberOfSimulationsWithState(number, total, ValidationState.ValidWithWarnings);
      public static string ValidSimulations(int number, int total) => numberOfSimulationsWithState(number, total, ValidationState.Valid);

      private static string numberOfSimulationsWithState(int number, int total, ValidationState state) => $"{state} Simulations ({number}/{total})";

      public static string AbsoluteToleranceIs(string tolerance) => $"Absolute Tolerance: {tolerance}";
      public static string RelativeToleranceIs(string tolerance) => $"Relative Tolerance: {tolerance}";

      public static string InstallationValidationPerformedIn(string startTime, string endTime, string delay)
      {
         return $"Start time: {startTime}{Environment.NewLine}End time: {endTime}{Environment.NewLine}Validation performed in {delay}";
      }

      public static string ComparisonFolder(string folerName) => $"{folerName} Folder";

      public static string MissingFileValidationMessage(string fileName, string folderContainingFile, string folderWithoutFile) =>
         $"{fileName} was contained in folder:{Environment.NewLine}{folderContainingFile}{Environment.NewLine}but was missing in folder:{Environment.NewLine}{folderWithoutFile}";
   }
}