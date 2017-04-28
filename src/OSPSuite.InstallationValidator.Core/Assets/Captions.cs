using System.Text;

namespace OSPSuite.InstallationValidator.Core.Assets
{
   public static class Captions
   {
      public static readonly string OutputFolder = "Output Folder";
      public static readonly string SelectOutputFolder = "Select Output Folder";
      public static readonly string Start = "Start";
      public static string TheValidationWasCanceled = "The validation was canceled";
      public static readonly string ReallyCancelInstallationValidation = "Really cancel installation validation?";
      public static readonly string StartingBatchCalculation = "Starting batch calculations...";
      public static readonly string StartingComparison = "Starting output comparisons...";

      public static readonly string ValidationDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, " +
                                                            "sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. " +
                                                            "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi " +
                                                            "ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in " +
                                                            "voluptate velit esse cillum dolore eu fugiat nulla pariatur. " +
                                                            "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
   }

   public static class Exceptions
   {
      public static string ExceptionSupportMessage(string issueTrackerUrl)
      {
         var sb = new StringBuilder();
         sb.AppendLine("oops...something went terribly wrong.");
         sb.AppendLine();
         sb.AppendLine("To best address the error, please enter an issue in our issue tracker:");
         sb.AppendLine($"    1 - Visit {issueTrackerUrl}");
         sb.AppendLine("    2 - Click on the 'New Issue' button");
         sb.AppendLine("    3 - Describe the steps you took prior to the problem emerging");
         sb.AppendLine("    4 - Copy the information below and paste it in the issue description");
         sb.AppendLine("    5 - if possible, attach your project file to the issue (do not attach confidential information)");
         sb.AppendLine();
         sb.AppendLine("Note: A GitHub account is required to create an issue");
         return sb.ToString();
      }

      public const string OutputNotDefined = "Output not defined!";
      public static string ArraysHaveDifferentLength(int length1, int length2) => $"Array used in comparison have different lengths ({length1} vs {length2}";
   }

   public static class Validation
   {
      public static string FolderDoesNotExist(string fileFullPath) => $"Folder '{fileFullPath}' does not exist";

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
         return $"Deviation for '{variable}' is {deviation} and is greater than the allowed max. tolerance of {maxDeviation}";
      }

      public static string OutputIsMissingFromSimulation(string outputPath, string simulationName, string folder)
      {
         return $"Output '{outputPath}' is missing from simulation '{simulationName}' defined in '{folder}'";
      }
   }
}