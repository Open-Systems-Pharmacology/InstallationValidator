using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using InstallationValidator.Core;
using InstallationValidator.Views;
using OSPSuite.Assets;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Mappers;
using OSPSuite.UI.Services;
using OSPSuite.UI.Views;
using OSPSuite.Utility;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Format;
using IMainView = InstallationValidator.Core.Presentation.Views.IMainView;

namespace InstallationValidator.Bootstrap
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

         //Path for #124 that should be removed when fixed is implementing into Utility
         FileHelper.GetVersion = getVersionPatch;
      }

      private string getVersionPatch(string binaryExecutablePath)
      {
         if (!FileHelper.FileExists(binaryExecutablePath))
            return null;

         var versionInfo = FileVersionInfo.GetVersionInfo(binaryExecutablePath);
         return string.IsNullOrEmpty(versionInfo.ProductVersion) ? versionInfo.FileVersion : versionInfo.ProductVersion;
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