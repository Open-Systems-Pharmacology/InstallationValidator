using System;
using System.Collections.Generic;

namespace InstallationValidator.Core.Reporting.Markdown
{
   public interface IMarkdownBuilder
   {
      void Build(object objectToReport, MarkdownReportContext context);
      Type SupportedType { get; }
   }

   public interface IMarkdownBuilder<in T> : IMarkdownBuilder
   {
      void Build(T objectToReport, MarkdownReportContext context);
   }

   public abstract class MarkdownBuilder<T> : IMarkdownBuilder<T>
   {
      public Type SupportedType => typeof(T);

      public void Build(object objectToReport, MarkdownReportContext context)
      {
         Build((T)objectToReport, context);
      }

      public abstract void Build(T objectToReport, MarkdownReportContext context);
   }

   public interface IMarkdownBuilderRepository
   {
      void Report(object objectToReport, MarkdownReportContext context);
      void Report(IEnumerable<object> objectsToReport, MarkdownReportContext context);
   }
}
