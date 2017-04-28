using System.Collections.Generic;
using OSPSuite.InstallationValidator.Core.Domain;

namespace OSPSuite.InstallationValidator.Helpers
{
   public class FolderInfoForSpecs : FolderInfo
   {
      private readonly IReadOnlyList<string> _testFiles;

      public FolderInfoForSpecs(string folder, IReadOnlyList<string> testFiles ) : base(folder)
      {
         _testFiles = testFiles;
      }

      protected override void AddAllFilesInFolder()
      {
         _fileNames.AddRange(_testFiles);
      }
   }
}