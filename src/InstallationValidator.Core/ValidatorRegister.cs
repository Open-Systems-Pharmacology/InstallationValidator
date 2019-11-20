using Castle.Facilities.TypedFactory;
using InstallationValidator.Core.Domain;
using InstallationValidator.Core.Reporting;
using InstallationValidator.Core.Services;
using OSPSuite.Core;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure;
using OSPSuite.Infrastructure.Container.Castle;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.Presentation.Services;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;
using ReportingRegister = OSPSuite.TeXReporting.ReportingRegister;

namespace InstallationValidator.Core
{
   public class ValidatorRegister : IRegister
   {
      public void RegisterInContainer(IContainer container)
      {
         registerCoreDependencies(container);

         container.AddScanner(x =>
         {
            x.AssemblyContainingType<ValidatorRegister>();
            x.ExcludeType<InstallationValidatorConfiguration>();
            x.WithConvention(new OSPSuiteRegistrationConvention(registerConcreteType: true));
            x.RegisterAs(LifeStyle.Transient);
         });

         registerReportingComponents(container);

         container.AddRegister(x => x.FromType<InfrastructureRegister>());
         container.Register<FolderInfo, FolderInfo>();
         container.Register<IComparisonStrategy, PointwiseComparisonStrategy>();

         registerDimensions(container);

         registerAbstractFactories(container);
      }

      private void registerCoreDependencies(IContainer container)
      {
         container.Register<IUnitSystemXmlSerializerRepository, UnitSystemXmlSerializerRepository>(LifeStyle.Singleton);
         container.Resolve<IUnitSystemXmlSerializerRepository>().PerformMapping();
         container.Register<IDimensionFactoryPersistor, DimensionFactoryPersistor>();

         container.Register<IExceptionManager, ExceptionManager>(LifeStyle.Singleton);
         container.Register<IEventPublisher, EventPublisher>(LifeStyle.Singleton);
         container.Register<DirectoryMapSettings, DirectoryMapSettings>(LifeStyle.Singleton);
         container.Register<StartableProcess, StartableProcess>();
         container.Register<ILogger, ValidatorLogger>(LifeStyle.Singleton);
      }

      private static void registerDimensions(IContainer container)
      {
         container.Register<IDimensionFactory, DimensionFactory>(LifeStyle.Singleton);

         var dimensionFactory = container.Resolve<IDimensionFactory>();
         var persister = container.Resolve<IDimensionFactoryPersistor>();
         var configuration = container.Resolve<IInstallationValidatorConfiguration>();

         persister.Load(dimensionFactory, configuration.DimensionFilePath);
         dimensionFactory.AddDimension(OSPSuite.Core.Domain.Constants.Dimension.NO_DIMENSION);
      }

      private static void registerReportingComponents(IContainer container)
      {
         container.AddRegister(x => x.FromType<ReportingRegister>());
         container.AddRegister(x => x.FromType<OSPSuite.Infrastructure.Reporting.InfrastructureReportingRegister>());

         container.AddScanner(scan =>
         {
            scan.AssemblyContainingType<InstallationValidationResultReporter>();
            scan.IncludeNamespaceContainingType<InstallationValidationResultReporter>();
            scan.WithConvention<ReporterRegistrationConvention>();
         });
      }

      private static void registerAbstractFactories(IContainer container)
      {
         container.RegisterFactory<IStartableProcessFactory>();
         container.RegisterFactory<ILogWatcherFactory>();
         container.RegisterFactory<IFolderInfoFactory>();
      }

      public static IContainer Initialize()
      {
         var container = new CastleWindsorContainer();
         IoC.InitializeWith(container);
         container.WindsorContainer.AddFacility<TypedFactoryFacility>();
         container.WindsorContainer.AddFacility<EventRegisterFacility>();

         container.RegisterImplementationOf(container.DowncastTo<IContainer>());
         container.Register<IApplicationConfiguration, IInstallationValidatorConfiguration, InstallationValidatorConfiguration>(LifeStyle.Singleton);

         return container;
      }
   }
}