using Castle.Facilities.TypedFactory;
using OSPSuite.Core;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Reporting;
using OSPSuite.Core.Serialization;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Infrastructure;
using OSPSuite.Infrastructure.Container.Castle;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.Infrastructure.Services;
using OSPSuite.InstallationValidator.Core.Domain;
using OSPSuite.InstallationValidator.Core.Reporting;
using OSPSuite.InstallationValidator.Core.Services;
using OSPSuite.Presentation;
using OSPSuite.Presentation.Services;
using OSPSuite.Utility.Compression;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;
using ReportingRegister = OSPSuite.TeXReporting.ReportingRegister;

namespace OSPSuite.InstallationValidator.Core
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

         container.Register<FolderInfo, FolderInfo>();
         container.Register<IComparisonStrategy, PointwiseComparisonStrategy>();
         container.Register<IApplicationConfiguration, IInstallationValidatorConfiguration, InstallationValidatorConfiguration>(LifeStyle.Singleton);

         registerDimensions(container);

         registerAbstractFactories(container);
      }

      private void registerCoreDependencies(IContainer container)
      {
//         container.AddRegister(x => x.FromType<CoreRegister>());
//         container.AddRegister(x => x.FromType<PresenterRegister>());
//         container.AddRegister(x => x.FromType<InfrastructureRegister>());
         
         container.Register<ICompression, SharpLibCompression>();
         container.Register<IStringCompression, StringCompression>();

         container.Register<IUnitSystemXmlSerializerRepository, UnitSystemXmlSerializerRepository>(LifeStyle.Singleton);
         container.Resolve<IUnitSystemXmlSerializerRepository>().PerformMapping();
         container.Register<IDimensionFactoryPersistor, DimensionFactoryPersistor>();

         container.Register<IExceptionManager, ExceptionManager>(LifeStyle.Singleton);
         container.Register<IEventPublisher, EventPublisher>(LifeStyle.Singleton);
         container.Register<DirectoryMapSettings, DirectoryMapSettings>(LifeStyle.Singleton);
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
         container.AddRegister(x => x.FromType<Infrastructure.Reporting.ReportingRegister>());

         container.AddScanner(scan =>
         {
            scan.AssemblyContainingType<InstallationValidationResultReporter>();
            scan.IncludeNamespaceContainingType<InstallationValidationResultReporter>();
            scan.WithConvention<ReporterRegistrationConvention>();
         });
         container.Register<IReportTemplateRepository, ReportTemplateRepository>();
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

         return container;
      }
   }
}