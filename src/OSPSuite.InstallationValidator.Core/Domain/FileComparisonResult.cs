using System.Collections.Generic;
using OSPSuite.Core.Domain;

namespace OSPSuite.InstallationValidator.Core.Domain
{
   public abstract class FileComparisonResult : IWithValidationState
   {
      public string FileName { get; }
      public string Folder1 { get; }
      public string Folder2 { get; }
      public virtual ValidationState State { get; }

      protected FileComparisonResult(string fileName, string folder1, string folder2)
      {
         FileName = fileName;
         Folder1 = folder1;
         Folder2 = folder2;
      }
   }

   public class MissingFileComparisonResult : FileComparisonResult
   {
      public override ValidationState State => ValidationState.Invalid;

      public MissingFileComparisonResult(string fileName, string folderContainingFile, string folderWithoutFile) : base(fileName, folderContainingFile, folderWithoutFile)
      {
      }

      public string FolderContainingFile => Folder1;
      public string FolderWithoutFile => Folder2;
   }

   public class OutputFileComparisonResult : FileComparisonResult
   {
      private readonly List<OutputComparisonResult> _outputComparisonResults =new List<OutputComparisonResult>();

      public OutputFileComparisonResult(string fileName, string folder1, string folder2) : base(fileName, folder1, folder2)
      {
      }

      public override ValidationState State => _outputComparisonResults.CombineStates();
   }
}