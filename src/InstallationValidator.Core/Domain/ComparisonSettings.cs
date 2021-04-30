using System.Collections.Generic;
using InstallationValidator.Core.Assets;

namespace InstallationValidator.Core.Domain
{
   public class ComparisonSettings
   {
      /// <summary>
      ///    First folder used for comparison
      /// </summary>
      public string FolderPath1 { get; set; }

      /// <summary>
      ///    Second folder used for comparison
      /// </summary>
      public string FolderPath2 { get; set; }

      public string FolderPathCaption1 { get; set; } = Captions.DefaultCaptionFolder1;

      public string FolderPathCaption2 { get; set; } = Captions.DefaultCaptionFolder2;

      public bool GenerateResultsForValidSimulation { get; set; } = false;

      public IReadOnlyList<string> PredefinedOutputPaths { get; set; } = new List<string>();

      /// <summary>
      /// Number of curves to add to the report when file being compared are not the same
      /// </summary>
      public byte? NumberOfCurves { get; set; }


      /// <summary>
      /// Should curve added to the model structure be ignored. Default is <code>false</code>
      /// </summary>
      public bool IgnoreAddedCurves { get; set; }

      /// <summary>
      /// Should curve removed from the model structure be ignored. Default is <code>false</code>
      /// </summary>
      public bool IgnoreRemovedCurves { get; set; }

   }
}