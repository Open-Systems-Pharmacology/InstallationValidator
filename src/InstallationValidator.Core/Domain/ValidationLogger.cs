using InstallationValidator.Core.Events;
using OSPSuite.Utility.Events;

namespace InstallationValidator.Core.Domain
{
   public interface IValidationLogger
   {
      /// <summary>
      /// Line will be appended as html in the log
      /// </summary>
      void AppendLine(string line="");

      /// <summary>
      /// Text will be appended as html in the log
      /// </summary>
      void AppendText(string text);

      /// <summary>
      /// Text will be appended as is in the log. No formatting will take place
      /// </summary>
      void AppendRawText(string text);
   }

   public class ValidationLogger : IValidationLogger
   {
      private readonly IEventPublisher _eventPublisher;

      public ValidationLogger(IEventPublisher eventPublisher)
      {
         _eventPublisher = eventPublisher;
      }

      public void AppendLine(string line="")
      {
         _eventPublisher.PublishEvent(new AppendLineToLogEvent(line));
      }

      public void AppendText(string text)
      {
         appendText(text, isHtml: true);
      }

      public void AppendRawText(string text)
      {
         appendText(text, isHtml: false);
      }

      private void appendText(string text, bool isHtml)
      {
         _eventPublisher.PublishEvent(new AppendTextToLogEvent(text, isHtml));
      }
   }
}