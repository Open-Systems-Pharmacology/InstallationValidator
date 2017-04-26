namespace OSPSuite.InstallationValidator.Core.Events
{
   public abstract class LogUpdatedEvent
   {
      public string NewText { get; }

      protected LogUpdatedEvent(string newText)
      {
         NewText = newText;
      }

   }
   public class LogResetEvent : LogUpdatedEvent
   {
      public LogResetEvent(string newText) : base(newText)
      {
      }
   }

   public class LogAppendedEvent : LogUpdatedEvent
   {
      public LogAppendedEvent(string newText) : base(newText)
      {
      }
   }
}
