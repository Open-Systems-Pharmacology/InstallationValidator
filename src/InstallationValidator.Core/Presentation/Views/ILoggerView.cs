using OSPSuite.Presentation.Views;

namespace InstallationValidator.Core.Presentation.Views
{
   public interface ILoggerView : IView
   {
      void ResetText(string empty);
      void AppendText(string textToLog);
      void AppendHTML(string htmlToLog);
   }
}