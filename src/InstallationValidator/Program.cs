using System;
using System.Windows.Forms;
using DevExpress.Skins;
using DevExpress.UserSkins;
using InstallationValidator.Bootstrap;
using InstallationValidator.Core.Presentation;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;

namespace InstallationValidator
{
   static class Program
   {
      /// <summary>
      ///    The main entry point for the application.
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
            var mainPresenter = IoC.Resolve<IMainPresenter>();
            Application.Run(mainPresenter.BaseView.DowncastTo<Form>());
         }
         catch (Exception e)
         {
            MessageBox.Show(e.ExceptionMessageWithStackTrace(), "Unhandled Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            e.LogError();
         }
      }
   }
}