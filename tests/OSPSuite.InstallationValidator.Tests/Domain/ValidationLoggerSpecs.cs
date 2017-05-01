using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.InstallationValidator.Core.Domain;
using OSPSuite.InstallationValidator.Core.Events;
using OSPSuite.Utility.Events;

namespace OSPSuite.InstallationValidator.Domain
{
   public abstract class concern_for_ValidationLogger : ContextSpecification<IValidationLogger>
   {
      protected IEventPublisher _eventPublisher;
      protected string _text = "BLA";

      protected override void Context()
      {
         _eventPublisher = A.Fake<IEventPublisher>();
         sut = new ValidationLogger(_eventPublisher);
      }
   }

   public class When_the_validation_logger_is_logging_a_line : concern_for_ValidationLogger
   {
      private AppendLineToLogEvent _event;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _eventPublisher.PublishEvent(A<AppendLineToLogEvent>._))
            .Invokes(x => _event = x.GetArgument<AppendLineToLogEvent>(0));

      }
      protected override void Because()
      {
         sut.AppendLine(_text);
      }

      [Observation]
      public void should_raise_the_append_line_event()
      {
         _event.ShouldNotBeNull();
         _event.Line.ShouldBeEqualTo(_text);
         _event.IsHtml.ShouldBeTrue();
      }
   }

   public class When_the_validation_logger_is_logging_some_text : concern_for_ValidationLogger
   {
      private AppendTextToLogEvent _event;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _eventPublisher.PublishEvent(A<AppendTextToLogEvent>._))
            .Invokes(x => _event = x.GetArgument<AppendTextToLogEvent>(0));

      }
      protected override void Because()
      {
         sut.AppendText(_text);
      }

      [Observation]
      public void should_raise_the_append_line_event()
      {
         _event.ShouldNotBeNull();
         _event.Text.ShouldBeEqualTo(_text);
         _event.IsHtml.ShouldBeTrue();
      }
   }

   public class When_the_validation_logger_is_logging_some_raw_text : concern_for_ValidationLogger
   {
      private AppendTextToLogEvent _event;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _eventPublisher.PublishEvent(A<AppendTextToLogEvent>._))
            .Invokes(x => _event = x.GetArgument<AppendTextToLogEvent>(0));

      }
      protected override void Because()
      {
         sut.AppendRawText(_text);
      }

      [Observation]
      public void should_raise_the_append_line_event()
      {
         _event.ShouldNotBeNull();
         _event.Text.ShouldBeEqualTo(_text);
         _event.IsHtml.ShouldBeFalse();
      }
   }
}	