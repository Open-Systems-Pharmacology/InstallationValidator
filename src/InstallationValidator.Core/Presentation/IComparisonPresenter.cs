using InstallationValidator.Core.Events;
using InstallationValidator.Core.Presentation.Views;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Events;

namespace InstallationValidator.Core.Presentation
{
   public interface IComparisonPresenter : IListener<AppendTextToLogEvent>, IListener<AppendLineToLogEvent>, IPresenter
   {
      IComparisonView ComparisonView { get; }
   }
}