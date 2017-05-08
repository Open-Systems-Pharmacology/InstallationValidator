using System;
using InstallationValidator.Core.Assets;
using InstallationValidator.Core.Presentation;
using OSPSuite.Core.Extensions;

namespace InstallationValidator.Core.Extensions
{
   public static class ComparisonPresenterExtensions
   {
      public static void LogText(this IComparisonPresenter presenter, string textToLog, bool isHtml = true)
      {
         if (isHtml)
            presenter.LogHTML(textToLog);
         else
            presenter.ComparisonView.AppendText(textToLog);
      }

      public static void ResetLog(this IComparisonPresenter presenter)
      {
         presenter.ComparisonView.ResetText(string.Empty);
      }

      public static void LogHTML(this IComparisonPresenter presenter, string htmlToLog)
      {
         presenter.ComparisonView.AppendHTML(htmlToLog);
      }

      public static void LogException(this IComparisonPresenter presenter, Exception e, string issueTrackerUrl)
      {
         presenter.LogLine();
         presenter.LogHTML(Exceptions.ExceptionViewDescription(issueTrackerUrl));
         presenter.LogLine();
         presenter.LogLine(e.ExceptionMessageWithStackTrace());
         presenter.LogLine();
      }

      public static void LogLine(this IComparisonPresenter presenter, string textToLog = "", bool isHtml = true)
      {
         if (isHtml)
            presenter.LogHTML($"<br>{textToLog}");
         else
            presenter.LogText($"{Environment.NewLine}{textToLog}", isHtml: false);
      }

   }
}
