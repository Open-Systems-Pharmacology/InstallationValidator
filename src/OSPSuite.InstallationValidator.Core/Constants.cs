using System.IO;

namespace OSPSuite.InstallationValidator.Core
{
   public static class Constants
   {
      public static readonly string ISSUE_TRACKER_URL = "https://github.com/Open-Systems-Pharmacology/OSPSuite.InstallationValidator/issues";
      public static readonly string APPLICATION_FOLDER_PATH = @"Open Systems Pharmacology\InstallationValidator";

      public static class Tools
      {
         // TODO - move that to core
         public static readonly string PKSIM_BATCH_TOOL = "PKSim.BatchTool.exe";
         // TODO - that should be a result of how we intend to install the json files
         public static readonly string BATCH_INPUTS =  Path.Combine("Inputs", "BatchFiles");
         public static readonly string BATCH_OUTPUTS = Path.Combine("Outputs", "BatchFiles");

         public static readonly string BATCH_LOG = "batch.log";
      }
   }
}
