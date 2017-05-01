using System.Collections.Generic;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.InstallationValidator.Core.Domain;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.TeXReporting.Items;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.InstallationValidator.Core.Reporting
{
   public class FileComparisonResultsTeXBuilder : OSPSuiteTeXBuilder<IEnumerable<FileComparisonResult>>
   {
      private readonly ITeXBuilderRepository _texBuilderRepository;

      public FileComparisonResultsTeXBuilder(ITeXBuilderRepository texBuilderRepository)
      {
         _texBuilderRepository = texBuilderRepository;
      }

      public override void Build(IEnumerable<FileComparisonResult> comparisonResults, OSPSuiteTracker buildTracker)
      {
         comparisonResults.Each(comparisonResult => { _texBuilderRepository.Report(comparisonResult, buildTracker); });
      }
   }

   public abstract class FileComparisonResultTeXBuilder<T> : OSPSuiteTeXBuilder<T> where T : FileComparisonResult
   {
      protected readonly ITeXBuilderRepository _builderRepository;

      protected FileComparisonResultTeXBuilder(ITeXBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(T fileComparisonResult, OSPSuiteTracker buildTracker)
      {
         var subParagraph = new SubParagraph(titleFor(fileComparisonResult));
         var report = new List<object> {subParagraph, fileComparisonResult.State};
         buildTracker.Track(subParagraph);

         _builderRepository.Report(report, buildTracker);
      }

      private string titleFor(T fileComparisonResult)
      {
         return FileHelper.FileNameFromFileFullPath(fileComparisonResult.FileName);
      }
   }
}