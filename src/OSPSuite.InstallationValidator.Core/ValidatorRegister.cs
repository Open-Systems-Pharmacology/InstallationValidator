using Castle.Facilities.TypedFactory;
using OSPSuite.Core;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Reporting;
using OSPSuite.Infrastructure.Container.Castle;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.Infrastructure.Services;
using OSPSuite.InstallationValidator.Core.Domain;
using OSPSuite.InstallationValidator.Core.Reporting;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using ReportingRegister = OSPSuite.TeXReporting.ReportingRegister;

namespace OSPSuite.InstallationValidator.Core
{
   public class ValidatorRegister : IRegister
   {
      public void RegisterInContainer(IContainer container)
      {
         container.AddScanner(x =>
         {
            x.AssemblyContainingType<ValidatorRegister>();
            x.ExcludeType<InstallationValidatorConfiguration>();
            x.WithConvention(new OSPSuiteRegistrationConvention(registerConcreteType: true));
            x.RegisterAs(LifeStyle.Transient);
         });

         container.AddScanner(scan =>
         {
            scan.AssemblyContainingType<InstallationValidationResultReporter>();
            scan.IncludeNamespaceContainingType<InstallationValidationResultReporter>();
            scan.WithConvention<ReporterRegistrationConvention>();
         });

         container.Register<FolderInfo, FolderInfo>();
         container.Register<IComparisonStrategy, PointwiseComparisonStrategy>();
         container.Register<IApplicationConfiguration, IInstallationValidatorConfiguration, InstallationValidatorConfiguration>(LifeStyle.Singleton);

         container.AddRegister(x => x.FromType<ReportingRegister>());
         container.AddRegister(x => x.FromType<Infrastructure.Reporting.ReportingRegister>());

         container.Register<IReportTemplateRepository, ReportTemplateRepository>();
         container.RegisterImplementationOf(container.DowncastTo<IContainer>());
         container.Register<IDimensionFactory, DimensionFactory>();
      }

      public static IContainer Initialize()
      {
         var container = new CastleWindsorContainer();
         IoC.InitializeWith(container);
         container.WindsorContainer.AddFacility<TypedFactoryFacility>();
         container.WindsorContainer.AddFacility<EventRegisterFacility>();
         return container;
      }
   }
}