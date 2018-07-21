using System.Collections.Generic;
using System.IO;
using System.Linq;
using OSPSuite.Core.Extensions;

namespace InstallationValidator.Core
{
   public static class Constants
   {
      public static readonly string ISSUE_TRACKER_URL = "https://github.com/Open-Systems-Pharmacology/InstallationValidator/issues";
      public static readonly string APPLICATION_FOLDER_PATH = @"Open Systems Pharmacology\InstallationValidator";
      public const double MAX_DEVIATION_TIME = 0.0001; //0.01%
      public const double MAX_DEVIATION_OUTPUT = 0.02; //2%
      public static readonly string PRODUCT_NAME = "Installation Validator";
      public static readonly string PRODUCT_NAME_WITH_TRADEMARK = $"{PRODUCT_NAME}®";
      public static readonly string DEFAULT_SKIN = "Metropolis";
      public const int BUTTON_HEIGHT = 48;

      public static IReadOnlyList<string> PREDEFINED_OUTPUT_PATHS = new[]
      {
         concentrationFor("PeripheralVenousBlood", "Plasma (Peripheral Venous Blood)"),
      };

      private static string concentrationFor(params string [] pathElements)
      {
         var path = pathElements.ToList();
         path.Insert(0, "Organism");
         return path.ToPathString();
      }

      public static class Tools
      {
         public static readonly string PKSIM_CLI = "PKSim.CLI.exe";
         public static readonly string BATCH_INPUTS = Path.Combine("Inputs", "BatchFiles");
         public static readonly string BATCH_OUTPUTS = Path.Combine("Outputs", "BatchFiles");
         public static readonly string CALCULATION_OUTPUTS = "Outputs";
         public static readonly string BATCH_LOG = "batch.log";
      }
   }
}