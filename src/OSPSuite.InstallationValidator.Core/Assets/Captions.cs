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
      }

      public static class Validation
      {
         public static string FolderDoesNotExist(string fileFullPath)
         {
            return $"Folder '{fileFullPath}' does not exist";
         }
      }
   }
}