using System;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.InstallationValidator.Core.Domain;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.TeXReporting.Items;

namespace OSPSuite.InstallationValidator.Core.Reporting
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

      private string missingFileReport(MissingFileComparisonResult fileComparisonResult)
      {
         return $"{fileComparisonResult.FileName} was contained in folder:{Environment.NewLine}{fileComparisonResult.FolderContainingFile}{Environment.NewLine}but was missing in folder:{Environment.NewLine}{fileComparisonResult.FolderWithoutFile}";
      }
   }
}