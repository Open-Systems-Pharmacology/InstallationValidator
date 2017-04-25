using System;
using System.Windows.Forms;
using OSPSuite.InstallationValidator.Bootstrap;
using OSPSuite.InstallationValidator.Core.Presentation;
using OSPSuite.Presentation.Services;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.InstallationValidator
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

            ApplicationStartup.Initialize();
            var mainPresenter = IoC.Resolve<IMainPresenter>();
            Application.Run(mainPresenter.BaseView.DowncastTo<Form>());
         }
         catch (Exception e)
         {
            MessageBox.Show(ExceptionManager.ExceptionMessageWithStackTraceFrom(e), "Unhandled Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            e.LogError();
         }
      }
   }
}
