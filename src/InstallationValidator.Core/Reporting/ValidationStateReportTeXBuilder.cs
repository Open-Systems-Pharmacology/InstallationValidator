using System;
using System.Collections.Generic;
using InstallationValidator.Core.Domain;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.TeXReporting.Items;

namespace InstallationValidator.Core.Reporting
{
   public class ValidationStateReportTeXBuilder : OSPSuiteTeXBuilder<ValidationStateReport>
   {
      private readonly ITeXBuilderRepository _builderRepository;

      public ValidationStateReportTeXBuilder(ITeXBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(ValidationStateReport validationStateReport, OSPSuiteTracker buildTracker)
      {
         var color = validationStateReport.ValidationColor();
         var report = new List<object> {new ColorText($"{validationStateReport.State}", color), Environment.NewLine};

         if (!string.IsNullOrEmpty(validationStateReport.Caption))
            report.Insert(0, validationStateReport.Caption);

         _builderRepository.Report(report, buildTracker);
      }
   }
}