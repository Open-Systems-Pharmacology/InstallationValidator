using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using System;
using System.Collections.Generic;

namespace InstallationValidator.Core.Reporting.Markdown
{
   public interface IMarkdownBuilder : ISpecification<Type>
   {
      void Build(object objectToReport, MarkdownReportContext context);
   }

   public interface IMarkdownBuilder<in T> : IMarkdownBuilder
   {
      void Build(T objectToReport, MarkdownReportContext context);
   }

   public abstract class MarkdownBuilder<T> : IMarkdownBuilder<T>
   {
      public bool IsSatisfiedBy(Type type) => type.IsAnImplementationOf<T>();

      public void Build(object objectToReport, MarkdownReportContext context)
      {
         Build((T)objectToReport, context);
      }

      public abstract void Build(T objectToReport, MarkdownReportContext context);
   }

}
