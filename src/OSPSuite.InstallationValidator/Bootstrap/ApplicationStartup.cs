using System.Threading;
using System.Windows.Forms;
using OSPSuite.Assets;
using OSPSuite.Core;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Container.Castle;
using OSPSuite.InstallationValidator.Core;
using OSPSuite.InstallationValidator.Core.Presentation;
using OSPSuite.InstallationValidator.Views;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Mappers;
using OSPSuite.UI.Services;
using OSPSuite.UI.Views;
using OSPSuite.Utility.Container;
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
         ApplicationIcons.DefaultIcon = Core.Assets.ApplicationIcons.ValidationToolIcon;
      }

      private IContainer initializeContainer()
      {
         var container = new CastleWindsorContainer();
         IoC.InitializeWith(container);
         container.RegisterImplementationOf(getCurrentContext());
         return container;
      }

      private void registerAllInContainer(IContainer container)
      {
         container.Register<IMainView, MainView>();
         container.Register<IMainPresenter, MainPresenter>();
         container.Register<IExceptionManager, ExceptionManager>(LifeStyle.Singleton);
         container.Register<IExceptionView, ExceptionView>(LifeStyle.Singleton);
         container.Register<ILogView, LogView>();
         container.Register<ILogPresenter, LogPresenter>();
         container.Register<IDialogCreator, DialogCreator>();
         container.Register<IDialogResultToViewResultMapper, DialogResultToViewResultMapper>();
         container.Register<DirectoryMapSettings, DirectoryMapSettings>();
         container.Register<IApplicationConfiguration, ApplicationConfiguration>(LifeStyle.Singleton);
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
