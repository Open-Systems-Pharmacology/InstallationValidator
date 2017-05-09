using System;
using InstallationValidator.Core.Assets;
using InstallationValidator.Core.Presentation;
using OSPSuite.Core.Extensions;

namespace InstallationValidator.Core.Extensions
{
   public static class LoggerPresenterExtensions
   {
      public static void LogText(this ILoggerPresenter presenter, string textToLog, bool isHtml = true)
      {
         if (isHtml)
            presenter.LogHTML(textToLog);
         else
            presenter.LoggerView.AppendText(textToLog);
      }

      public static void ResetLog(this ILoggerPresenter presenter)
      {
         presenter.LoggerView.ResetText(string.Empty);
      }

      public static void LogHTML(this ILoggerPresenter presenter, string htmlToLog)
      {
         presenter.LoggerView.AppendHTML(htmlToLog);
      }

      public static void LogException(this ILoggerPresenter presenter, Exception e, string issueTrackerUrl)
      {
         presenter.LogLine();
         presenter.LogHTML(Exceptions.ExceptionViewDescription(issueTrackerUrl));
         presenter.LogLine();
         presenter.LogLine(e.ExceptionMessageWithStackTrace());
         presenter.LogLine();
      }

      public static void LogLine(this ILoggerPresenter presenter, string textToLog = "", bool isHtml = true)
      {
         if (isHtml)
            presenter.LogHTML($"<br>{textToLog}");
         else
            presenter.LogText($"{Environment.NewLine}{textToLog}", isHtml: false);
      }

   }
}
