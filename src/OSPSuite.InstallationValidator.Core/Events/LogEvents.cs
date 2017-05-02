namespace OSPSuite.InstallationValidator.Core.Events
{
   public class AppendTextToLogEvent
   {
      public string Text { get; }
      public bool IsHtml { get; }

      public AppendTextToLogEvent(string text, bool isHtml = true)
      {
         Text = text;
         IsHtml = isHtml;
      }
   }

   public class AppendLineToLogEvent
   {
      public string Line { get; }
      public bool IsHtml { get; }

      public AppendLineToLogEvent(string line, bool isHtml = true)
      {
         Line = line;
         IsHtml = isHtml;
      }
   }
}