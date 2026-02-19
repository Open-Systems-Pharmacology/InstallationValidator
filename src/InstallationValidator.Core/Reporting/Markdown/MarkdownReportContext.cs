using System.Drawing;
using System.Linq;
using System.Text;

namespace InstallationValidator.Core.Reporting.Markdown
{
   public class MarkdownReportContext
   {
      public StringBuilder Content { get; } = new StringBuilder();
      public int CurrentHeadingLevel { get; set; } = 1;

      public void AppendHeading(string text, int level)
      {
         Content.AppendLine();
         Content.AppendLine($"{new string('#', level)} {text}");
         Content.AppendLine();
      }

      public void AppendHeading(string text)
      {
         AppendHeading(text, CurrentHeadingLevel);
      }

      public void AppendParagraph(string text)
      {
         Content.AppendLine(text);
         Content.AppendLine();
      }

      public void AppendLine(string text)
      {
         Content.AppendLine(text);
      }

      public void AppendBold(string label, string value)
      {
         Content.AppendLine($"**{label}**: {value}");
         Content.AppendLine();
      }

      public void AppendColoredStatus(string text, Color color)
      {
         var colorHex = $"#{color.R:X2}{color.G:X2}{color.B:X2}";
         Content.AppendLine($"<span style=\"color:{colorHex}; font-weight:bold;\">{escapeHtml(text)}</span>");
         Content.AppendLine();
      }

      public void AppendSvg(string svgContent)
      {
         Content.AppendLine();
         Content.AppendLine(svgContent);
         Content.AppendLine();
      }

      public void AppendHorizontalRule()
      {
         Content.AppendLine();
         Content.AppendLine("---");
         Content.AppendLine();
      }

      public void AppendListItem(string text)
      {
         Content.AppendLine($"- {text}");
      }

      public void AppendBlankLine()
      {
         Content.AppendLine();
      }

      public void AppendTable(string[] headers, string[][] rows)
      {
         Content.AppendLine();
         Content.AppendLine("| " + string.Join(" | ", headers.Select(escapeTableCell)) + " |");
         Content.AppendLine("| " + string.Join(" | ", new string[headers.Length].Select(_ => "---")) + " |");
         foreach (var row in rows)
         {
            Content.AppendLine("| " + string.Join(" | ", row.Select(escapeTableCell)) + " |");
         }
         Content.AppendLine();
      }

      private string escapeTableCell(string text)
      {
         if (string.IsNullOrEmpty(text)) return "";
         return text.Replace("|", "\\|");
      }

      private string escapeHtml(string text)
      {
         if (string.IsNullOrEmpty(text)) return "";
         return text
            .Replace("&", "&amp;")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;");
      }

      public override string ToString()
      {
         return Content.ToString();
      }
   }
}
