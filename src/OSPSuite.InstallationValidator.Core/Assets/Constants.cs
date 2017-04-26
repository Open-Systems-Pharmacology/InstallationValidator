namespace OSPSuite.InstallationValidator.Core.Assets
{
   public static class Constants
   {
      public class Captions
      {
         public static readonly string OutputFolder = "Output Folder";
         public static readonly string SelectOutputFolder = "Select Output Folder";
         public static readonly string Start = "Start";
         public static string TheValidationWasCanceled = "The validation was canceled";
      }

      public static class Validation
      {
            public static string FolderDoesNotExist(string fileFullPath)
            {
               return $"Folder '{fileFullPath}' does not exist";
            }
         }

      public static class RegistryPaths
      {
         //TODO - this should come from OSPSuite.Core
         public static string PKSimRegPath => @"Open Systems Pharmacology\PK-Sim\";
      }

      public static class Tools
      {
         // TODO - move that to core
         public static readonly string PKSimBatchTool = "PKSim.BatchTool.exe";
         // TODO - that should be a result of how we intend to install the json files
         public static readonly string BatchInputs = "ValidationInputs";

         public static readonly string BatchLog = "batch.log";
      }
   }
}
