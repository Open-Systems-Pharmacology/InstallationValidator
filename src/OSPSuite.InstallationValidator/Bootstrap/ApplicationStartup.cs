using System.Threading;
using System.Windows.Forms;
using Castle.Facilities.TypedFactory;
using OSPSuite.Assets;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Container.Castle;
using OSPSuite.InstallationValidator.Core;
using OSPSuite.InstallationValidator.Core.Domain;
using OSPSuite.InstallationValidator.Core.Services;
using OSPSuite.InstallationValidator.Views;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Mappers;
using OSPSuite.UI.Services;
using OSPSuite.UI.Views;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Exceptions;
using IMainView = OSPSuite.InstallationValidator.Core.Presentation.Views.IMainView;

namespace OSPSuite.InstallationValidator.Bootstrap
{
   public class ApplicationStartup
   {
      public static void Initialize()
      {
         new ApplicationStartup().InitializeForStartup();
      }

      public void InitializeForStartup()
      {
         registerAllInContainer(initializeContainer());
         ApplicationIcons.DefaultIcon = ApplicationIcons.Comparison;
      }

      private IContainer initializeContainer()
      {
         var container = ValidatorRegister.Initialize();
         container.RegisterImplementationOf(getCurrentContext());
         container.AddRegister(x=>x.FromType<ValidatorRegister>());
         return container;
      }

      private void registerAllInContainer(IContainer container)
      {
         container.Register<IMainView, MainView>();
         container.Register<IExceptionManager, ExceptionManager>(LifeStyle.Singleton);
         container.Register<IExceptionView, ExceptionView>(LifeStyle.Singleton);
         container.Register<IEventPublisher, EventPublisher>(LifeStyle.Singleton);
         container.Register<IDialogCreator, DialogCreator>();
         container.Register<IDialogResultToViewResultMapper, DialogResultToViewResultMapper>();
         container.Register<DirectoryMapSettings, DirectoryMapSettings>();

         container.RegisterFactory<IStartableProcessFactory>();
         container.RegisterFactory<ILogWatcherFactory>();
         container.RegisterFactory<IFolderInfoFactory>();
      }

      private SynchronizationContext getCurrentContext()
      {
         var context = SynchronizationContext.Current;
         if (context == null)
         {
            context = new WindowsFormsSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(context);
         }
         return SynchronizationContext.Current;
      }
   }
}
