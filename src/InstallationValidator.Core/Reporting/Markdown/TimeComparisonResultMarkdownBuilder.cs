using InstallationValidator.Core.Domain;
using OSPSuite.Core.Domain;

namespace InstallationValidator.Core.Reporting.Markdown
{
   public class TimeComparisonResultMarkdownBuilder : MarkdownBuilder<TimeComparisonResult>
   {
      private readonly DoubleFormatter _doubleFormatter = new DoubleFormatter();

      public override void Build(TimeComparisonResult timeComparisonResult, MarkdownReportContext context)
      {
         context.AppendHeading(Assets.Reporting.TimeComparisonValidation, 4);
         context.AppendParagraph(timeComparisonResult.Message);
         context.AppendBold(Assets.Reporting.Deviation, _doubleFormatter.Format(timeComparisonResult.Deviation));
      }
   }
}
