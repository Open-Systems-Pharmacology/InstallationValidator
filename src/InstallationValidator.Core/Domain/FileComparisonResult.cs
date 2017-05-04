using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Utils;
using OSPSuite.Core.Domain;

namespace InstallationValidator.Core.Domain
{
   public abstract class FileComparisonResult : IWithValidationState
   {
      public string FileName { get; }
      public string Folder1 { get; }
      public string Folder2 { get; }
      public virtual ValidationState State { get; } = ValidationState.Invalid;

      protected FileComparisonResult(string fileName, string folder1, string folder2)
      {
         FileName = fileName;
         Folder1 = folder1;
         Folder2 = folder2;
      }
   }

   public class MissingFileComparisonResult : FileComparisonResult
   {
      public MissingFileComparisonResult(string fileName, string folderContainingFile, string folderWithoutFile) : base(fileName, folderContainingFile, folderWithoutFile)
      {
      }

      public string FolderContainingFile => Folder1;
      public string FolderWithoutFile => Folder2;
   }

   public class OutputFileComparisonResult : FileComparisonResult
   {
      private readonly List<OutputComparisonResult> _outputComparisonResults = new List<OutputComparisonResult>();
      public TimeComparisonResult TimeComparison { get; set; }
      public double AbsTol { get; set; }
      public double RelTol { get; set; }

      public OutputFileComparisonResult(string fileName, string folder1, string folder2) : base(fileName, folder1, folder2)
      {
      }

      public void AddOutputComparisons(IEnumerable<OutputComparisonResult> comparisonResults)
      {
         comparisonResults.Each(AddOutputComparison);
      }

      public void AddOutputComparison(OutputComparisonResult comparisonResult)
      {
         _outputComparisonResults.Add(comparisonResult);
      }

      public IReadOnlyList<OutputComparisonResult> OutputComparisonResults => _outputComparisonResults;

      public override ValidationState State => allWithValidationStates.CombineStates();

      private IReadOnlyList<IWithValidationState> allWithValidationStates
      {
         get
         {
            var withValidStates = _outputComparisonResults.Cast<IWithValidationState>().ToList();
            if (TimeComparison != null)
               withValidStates.Add(TimeComparison);

            return withValidStates;
         }
      }
   }
}