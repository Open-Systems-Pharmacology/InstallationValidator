using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Utility.Extensions;

namespace InstallationValidator.Core.Domain
{
   public class BatchComparisonResult : IWithValidationState
   {
      private readonly List<FileComparisonResult> _fileComparisonResults = new List<FileComparisonResult>();

      public ComparisonSettings ComparisonSettings = new ComparisonSettings();

      public void AddFileComparisons(IEnumerable<FileComparisonResult> fileComparisonResults)
      {
         fileComparisonResults.Each(AddFileComparison);
      }

      public void AddFileComparison(FileComparisonResult fileComparisonResult)
      {
         _fileComparisonResults.Add(fileComparisonResult);
      }

      public IReadOnlyList<FileComparisonResult> FileComparisonResults => _fileComparisonResults;

      public ValidationState State => _fileComparisonResults.CombineStates();

      public string FolderPathCaption1 => ComparisonSettings.FolderPathCaption1;

      public string FolderPathCaption2 => ComparisonSettings.FolderPathCaption2;

      public string FolderPath1 => ComparisonSettings.FolderPath1;

      public string FolderPath2 => ComparisonSettings.FolderPath2;
   }
}