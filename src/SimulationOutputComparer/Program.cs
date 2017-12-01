using System;
using System.Windows.Forms;
using DevExpress.Skins;
using DevExpress.UserSkins;
using InstallationValidator.Core.Presentation;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using SimulationOutputComparer.Bootstrap;

namespace SimulationOutputComparer
{
   static class Program
   {
      /// <summary>
      /// The main entry point for the application.
      /// </summary>
      [STAThread]
      static void Main()
      {
         try
         {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            BonusSkins.Register();
            SkinManager.EnableFormSkins();

            ApplicationStartup.Initialize();
            var simulationComparisonPresenter = IoC.Resolve<ISimulationComparisonPresenter>();
            Application.Run(simulationComparisonPresenter.BaseView.DowncastTo<Form>());
         }
         catch (Exception e)
         {
            MessageBox.Show(e.ExceptionMessageWithStackTrace(), "Unhandled Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }
   }
}
