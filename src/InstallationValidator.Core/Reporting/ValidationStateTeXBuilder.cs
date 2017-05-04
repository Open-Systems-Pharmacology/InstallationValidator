using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using OSPSuite.Core.Domain;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.TeXReporting.Items;
using OSPSuite.TeXReporting.TeX;
using OSPSuite.Utility.Extensions;

namespace InstallationValidator.Core.Reporting
{
   public class ValidationStateTeXBuilder : OSPSuiteTeXBuilder<ValidationState>
   {
      private readonly ITeXBuilderRepository _builderRepository;

      public ValidationStateTeXBuilder(ITeXBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(ValidationState validationState, OSPSuiteTracker buildTracker)
      {
         var color = colorFor(validationState);
         _builderRepository.Report(new object[] {Assets.Reporting.ValidationResult, new ColorText($"{validationState}", color), Environment.NewLine}, buildTracker);
      }

      private Color colorFor(ValidationState validationState)
      {
         switch (validationState)
         {
            case ValidationState.Invalid:
               return Color.Red;
            case ValidationState.ValidWithWarnings:
               return Color.Orange;
            case ValidationState.Valid:
               return Color.Green;
            default:
               return Color.Empty;
         }
      }
   }

   public class ColorText : Text
   {
      public Color Color { get; }

      public ColorText(string content, Color color, params object[] items) : base(content, items)
      {
         Color = color;
      }
   }

   // TODO - make TextTeXBuilder from reporting public so it can be inherited
   public class ColorTextTeXBuilder : TeXChunkBuilder<ColorText>
   {
      private readonly ITeXBuilderRepository _builderRepository;

      public ColorTextTeXBuilder(ITeXBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(ColorText text, BuildTracker tracker)
      {
         base.Build(text, tracker);
      }

      public override string TeXChunk(ColorText text)
      {
         if (string.IsNullOrEmpty(text.Content))
            return string.Empty;

         string newText;
         if (text.Items.Count > 0)
         {
            newText = text.Content;
            for (var i = 0; i < text.Items.Count; i++)
               newText = newText.Replace($"{{{i}}}", $"@{i}@");

            newText = text.Converter.StringToTeX(newText);

            for (var i = 0; i < text.Items.Count; i++)
               newText = newText.Replace($"@{i}@", $"{{{i}}}");

            var texChunks = new List<object>();
            text.Items.Each(item => texChunks.Add(_builderRepository.ChunkFor(item)));

            newText = string.Format(newText, texChunks.ToArray());
         }
         else
         {
            newText = text.Converter.StringToTeX(text.Content);
         }

         return colorizedText(alignedText(text.Alignment, styledText(text.FontStyle, newText)), text.Color);
      }

      // TODO - when the TextTeXBuilder from Utility is inherited, this is the only method to keep
      private string colorizedText(string text, Color theColor)
      {
         return theColor.IsEmpty ? text : $"\\definecolor{{theColor}}{{rgb}}{{{theColor.R},{theColor.G}, {theColor.B}}}\\textcolor{{theColor}}{{{text}}}";
      }

      private string alignedText(Text.Alignments alignment, string text)
      {
         var tex = new StringBuilder();
         const string FORMAT = "{0}{1}{2}";
         switch (alignment)
         {
            case Text.Alignments.centered:
               tex.AppendFormat(FORMAT, Helper.Begin(Helper.Environments.center), text,
                  Helper.End(Helper.Environments.center));
               return tex.ToString();
            case Text.Alignments.flushleft:
               tex.AppendFormat(FORMAT, Helper.Begin(Helper.Environments.flushleft), text,
                  Helper.End(Helper.Environments.flushleft));
               return tex.ToString();
            case Text.Alignments.flushright:
               tex.AppendFormat(FORMAT, Helper.Begin(Helper.Environments.flushright), text,
                  Helper.End(Helper.Environments.flushright));
               return tex.ToString();
            default:
               return text;
         }
      }

      private string styledText(Text.FontStyles fontStyle, string text)
      {
         switch (fontStyle)
         {
            case Text.FontStyles.bold:
               return Helper.Bold(text);
            case Text.FontStyles.italic:
               return Helper.Italic(text);
            case Text.FontStyles.slanted:
               return Helper.Slanted(text);
            default:
               return text;
         }
      }
   }
}