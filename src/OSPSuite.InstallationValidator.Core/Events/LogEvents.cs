namespace OSPSuite.InstallationValidator.Core.Events
{
   public class LogAppendedEvent
   {
      public string NewText { get; }

      public LogAppendedEvent(string newText)
      {
         NewText = newText;
      }

   }
}
