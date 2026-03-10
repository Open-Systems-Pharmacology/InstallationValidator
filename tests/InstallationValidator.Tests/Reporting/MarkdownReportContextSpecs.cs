using System.Drawing;
using InstallationValidator.Core.Reporting.Markdown;
using NUnit.Framework;
using OSPSuite.BDDHelper;

namespace InstallationValidator.Reporting
{
   public abstract class concern_for_MarkdownReportContext : ContextSpecification<MarkdownReportContext>
   {
      protected override void Context()
      {
         sut = new MarkdownReportContext();
      }
   }

   public class When_appending_a_heading : concern_for_MarkdownReportContext
   {
      protected override void Because()
      {
         sut.AppendHeading("Test Heading", 3);
      }

      [Observation]
      public void should_produce_correct_hash_prefix()
      {
         StringAssert.Contains("### Test Heading", sut.ToString());
      }
   }

   public class When_appending_a_table : concern_for_MarkdownReportContext
   {
      protected override void Because()
      {
         sut.AppendTable(
            new[] { "Name", "Value" },
            new[]
            {
               new[] { "A", "1" },
               new[] { "B", "2" }
            });
      }

      [Observation]
      public void should_produce_valid_gfm_header_row()
      {
         StringAssert.Contains("| Name | Value |", sut.ToString());
      }

      [Observation]
      public void should_produce_header_separator_row()
      {
         StringAssert.Contains("| --- | --- |", sut.ToString());
      }

      [Observation]
      public void should_produce_data_rows()
      {
         var content = sut.ToString();
         StringAssert.Contains("| A | 1 |", content);
         StringAssert.Contains("| B | 2 |", content);
      }
   }

   public class When_appending_colored_status : concern_for_MarkdownReportContext
   {
      protected override void Because()
      {
         sut.AppendColoredStatus("Invalid", Color.Red);
      }

      [Observation]
      public void should_produce_span_with_color_style()
      {
         StringAssert.Contains("<span style=\"color:#FF0000; font-weight:bold;\">", sut.ToString());
      }

      [Observation]
      public void should_contain_html_escaped_text()
      {
         StringAssert.Contains("Invalid</span>", sut.ToString());
      }
   }

   public class When_appending_bold : concern_for_MarkdownReportContext
   {
      protected override void Because()
      {
         sut.AppendBold("Deviation", "0.44");
      }

      [Observation]
      public void should_produce_bold_label_with_value()
      {
         StringAssert.Contains("**Deviation**: 0.44", sut.ToString());
      }
   }

   public class When_accumulating_content : concern_for_MarkdownReportContext
   {
      protected override void Because()
      {
         sut.AppendHeading("Title", 1);
         sut.AppendParagraph("Some text.");
         sut.AppendBold("Key", "Value");
      }

      [Observation]
      public void should_return_full_accumulated_content()
      {
         var content = sut.ToString();
         StringAssert.Contains("# Title", content);
         StringAssert.Contains("Some text.", content);
         StringAssert.Contains("**Key**: Value", content);
      }
   }
}
