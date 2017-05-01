using System;
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
         comparisonResults.Each(comparisonResult =>
         {
            _texBuilderRepository.Report(comparisonResult, buildTracker);
         });

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
         var chapter = buildTracker.GetStructureElementRelativeToLast(titleFor(fileComparisonResult), 1);
         var report = new List<object> { chapter, fileComparisonResult.State };
         buildTracker.Track(chapter);

         _builderRepository.Report(report, buildTracker);
      }

      private string titleFor(T fileComparisonResult)
      {
         return FileHelper.FileNameFromFileFullPath(fileComparisonResult.FileName);
      }
   }

   public class OutputFileComparisonResultTexBuilder : FileComparisonResultTeXBuilder<OutputFileComparisonResult>
   {
      public OutputFileComparisonResultTexBuilder(ITeXBuilderRepository builderRepository) : base(builderRepository)
      {
      }

      public override void Build(OutputFileComparisonResult fileComparisonResult, OSPSuiteTracker buildTracker)
      {
         base.Build(fileComparisonResult, buildTracker);
         _builderRepository.Report("Output Comparison", buildTracker);
      }
   }

   public class MissingFileComparisonResultTexBuilder : FileComparisonResultTeXBuilder<MissingFileComparisonResult>
   {
      public MissingFileComparisonResultTexBuilder(ITeXBuilderRepository builderRepository) : base(builderRepository)
      {
      }

      public override void Build(MissingFileComparisonResult fileComparisonResult, OSPSuiteTracker buildTracker)
      {
         base.Build(fileComparisonResult, buildTracker);
         _builderRepository.Report(missingFileReport(fileComparisonResult), buildTracker);
      }

      private string missingFileReport(MissingFileComparisonResult fileComparisonResult)
      {
         return $"{fileComparisonResult.FileName} was contained in folder:{Environment.NewLine}{fileComparisonResult.FolderContainingFile}{Environment.NewLine}but was missing in folder:{Environment.NewLine}{fileComparisonResult.FolderWithoutFile}";
      }
   }
}