using System.Collections.Generic;
using System.Linq;
using InstallationValidator.Core.Domain;
using OSPSuite.Core.Domain;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.TeXReporting.Items;

namespace InstallationValidator.Core.Reporting
{
   public class OutputFileComparisonResultTeXBuilder : FileComparisonResultTeXBuilder<OutputFileComparisonResult>
   {
      private readonly DoubleFormatter _doubleFormatter;

      public OutputFileComparisonResultTeXBuilder(ITeXBuilderRepository builderRepository) : base(builderRepository)
      {
         _doubleFormatter = new DoubleFormatter();
      }

      public override void Build(OutputFileComparisonResult fileComparisonResult, OSPSuiteTracker buildTracker)
      {
         base.Build(fileComparisonResult, buildTracker);
         var objectsToReport = new List<object>();

         if (!fileComparisonResult.IsValid())
         {
            objectsToReport.Add(Assets.Reporting.AbsoluteToleranceIs(_doubleFormatter.Format(fileComparisonResult.AbsTol)));
            objectsToReport.Add(new LineBreak());
            objectsToReport.Add(Assets.Reporting.RelativeToleranceIs(_doubleFormatter.Format(fileComparisonResult.RelTol)));
         }

         if (!fileComparisonResult.TimeComparison.IsValid())
            objectsToReport.Add(fileComparisonResult.TimeComparison);

         objectsToReport.AddRange(fileComparisonResult.OutputComparisonResults.Where(x => !x.IsValid()));

         _builderRepository.Report(objectsToReport, buildTracker);
      }
   }
}