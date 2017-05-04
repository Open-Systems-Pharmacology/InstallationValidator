using InstallationValidator.Core.Domain;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.TeXReporting.Items;

namespace InstallationValidator.Core.Reporting
{
   public class MissingFileComparisonResultTeXBuilder : FileComparisonResultTeXBuilder<MissingFileComparisonResult>
   {
      public MissingFileComparisonResultTeXBuilder(ITeXBuilderRepository builderRepository) : base(builderRepository)
      {
      }

      public override void Build(MissingFileComparisonResult fileComparisonResult, OSPSuiteTracker buildTracker)
      {
         base.Build(fileComparisonResult, buildTracker);
         _builderRepository.Report(new object[]
         {
            new LineBreak(),
            Assets.Reporting.MissingFileValidation,
            new LineBreak(), missingFileReport(fileComparisonResult)
         }, buildTracker);
      }

      private string missingFileReport(MissingFileComparisonResult missingFileComparisonResult)
      {
         return Assets.Reporting.MissingFileValidationMessage(missingFileComparisonResult.FileName, missingFileComparisonResult.FolderContainingFile, missingFileComparisonResult.FolderWithoutFile);
      }
   }
}