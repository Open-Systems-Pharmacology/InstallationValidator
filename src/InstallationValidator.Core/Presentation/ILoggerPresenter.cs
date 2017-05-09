using InstallationValidator.Core.Events;
using InstallationValidator.Core.Presentation.Views;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Events;

namespace InstallationValidator.Core.Presentation
{
   public interface ILoggerPresenter : IPresenter,
      IListener<AppendTextToLogEvent>,
      IListener<AppendLineToLogEvent>
   {
      ILoggerView LoggerView { get; }
   }
}