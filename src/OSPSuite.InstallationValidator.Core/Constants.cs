using System.IO;

namespace OSPSuite.InstallationValidator.Core
{
   public static class Constants
   {
      public static readonly string ISSUE_TRACKER_URL = "https://github.com/Open-Systems-Pharmacology/OSPSuite.InstallationValidator/issues";
      public static readonly string APPLICATION_FOLDER_PATH = @"Open Systems Pharmacology\InstallationValidator";
      public static double MAX_DEVIATION_TIME = 0.0001; //0.01%
      public static double MAX_DEVIATION_OUTPUT = 0.02; //2%
      
      public static class Tools
      {
         // TODO - move that to core
         public static readonly string PKSIM_BATCH_TOOL = "PKSim.BatchTool.exe";
         // TODO - that should be a result of how we intend to install the json files
         public static readonly string BATCH_INPUTS =  Path.Combine("Inputs", "BatchFiles");
         public static readonly string BATCH_OUTPUTS = Path.Combine("Outputs", "BatchFiles");

         public static readonly string BATCH_LOG = "batch.log";
      }

      public static class Filter
      {
         //TODO MOVE TO CORE
         public static readonly string JSON_FILTER = $"*{OSPSuite.Core.Domain.Constants.Filter.JSON_EXTENSION}";
      }
   }
}
