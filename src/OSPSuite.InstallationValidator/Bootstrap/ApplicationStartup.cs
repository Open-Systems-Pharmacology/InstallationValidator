using System.Threading;
using System.Windows.Forms;
using OSPSuite.Assets;
using OSPSuite.Core.Services;
using OSPSuite.InstallationValidator.Core;
using OSPSuite.InstallationValidator.Views;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Mappers;
using OSPSuite.UI.Services;
using OSPSuite.UI.Views;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Format;
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
         NumericFormatterOptions.Instance.DecimalPlace = 2;
      }

      private IContainer initializeContainer()
      {
         return ValidatorRegister.Initialize();
      }

      private void registerAllInContainer(IContainer container)
      {
         using (container.OptimizeDependencyResolution())
         {
            container.RegisterImplementationOf(getCurrentContext());
            container.AddRegister(x => x.FromType<ValidatorRegister>());

            container.Register<IMainView, MainView>();
            container.Register<IExceptionView, ExceptionView>(LifeStyle.Singleton);
            container.Register<IDialogCreator, DialogCreator>();
            container.Register<IDialogResultToViewResultMapper, DialogResultToViewResultMapper>();
         }
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