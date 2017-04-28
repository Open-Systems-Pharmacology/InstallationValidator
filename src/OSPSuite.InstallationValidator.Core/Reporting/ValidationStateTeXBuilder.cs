using System;
using OSPSuite.Core.Domain;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.TeXReporting.Builder;

namespace OSPSuite.InstallationValidator.Core.Reporting
{
   public class ValidationStateTeXBuilder : OSPSuiteTeXBuilder<ValidationState>
   {
      private readonly ITeXBuilderRepository _builderRepository;

      public ValidationStateTeXBuilder(ITeXBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(ValidationState validationState, OSPSuiteTracker buildTracker)
      {
         _builderRepository.Report($"Result of the Validation: {validationState}{Environment.NewLine}", buildTracker);
      }
   }
}