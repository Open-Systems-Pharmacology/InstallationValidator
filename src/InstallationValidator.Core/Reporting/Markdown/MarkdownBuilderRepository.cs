using System.Collections.Generic;
using OSPSuite.Utility;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;

namespace InstallationValidator.Core.Reporting.Markdown
{
   public interface IMarkdownBuilderRepository
   {
      void Report(object objectToReport, MarkdownReportContext context);
      void Report(IEnumerable<object> objectsToReport, MarkdownReportContext context);
   }

   public class MarkdownBuilderRepository : BuilderRepository<IMarkdownBuilder>, IMarkdownBuilderRepository
   {
      public MarkdownBuilderRepository(IContainer container) : base(container, typeof(IMarkdownBuilder<>))
      {
      }

      public void Report(object objectToReport, MarkdownReportContext context)
      {
         if (objectToReport == null) return;

         var builder = BuilderFor(objectToReport);
         builder.Build(objectToReport, context);
      }

      public void Report(IEnumerable<object> objectsToReport, MarkdownReportContext context)
      {
         objectsToReport?.Each(x => Report(x, context));
      }
   }
}