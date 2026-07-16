using InstallationValidator.Core.Domain;

namespace InstallationValidator.Core.Reporting.Markdown
{
   public class ValidationStateReportMarkdownBuilder : MarkdownBuilder<ValidationStateReport>
   {
      public override void Build(ValidationStateReport validationStateReport, MarkdownReportContext context)
      {
         var color = validationStateReport.ValidationColor();

         if (!string.IsNullOrEmpty(validationStateReport.Caption))
         {
            context.AppendLine($"**{validationStateReport.Caption}**");
         }

         context.AppendColoredStatus(validationStateReport.State.ToString(), color);
      }
   }
}
