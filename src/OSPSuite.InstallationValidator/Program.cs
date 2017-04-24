using System;
using System.Windows.Forms;
using OSPSuite.InstallationValidator.Views;

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
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         Application.Run(new MainView());
      }
   }
}
