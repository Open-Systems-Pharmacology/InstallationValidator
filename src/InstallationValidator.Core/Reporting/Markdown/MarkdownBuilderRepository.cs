using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;

namespace InstallationValidator.Core.Reporting.Markdown
{
   public class MarkdownBuilderRepository : IMarkdownBuilderRepository
   {
      private readonly IEnumerable<IMarkdownBuilder> _builders;

      public MarkdownBuilderRepository(IEnumerable<IMarkdownBuilder> builders)
      {
         _builders = builders;
      }

      public void Report(object objectToReport, MarkdownReportContext context)
      {
         if (objectToReport == null) return;

         var builder = findBuilderFor(objectToReport.GetType());
         if (builder != null)
         {
            builder.Build(objectToReport, context);
         }
      }

      public void Report(IEnumerable<object> objectsToReport, MarkdownReportContext context)
      {
         objectsToReport?.Each(x=> Report(x, context));
      }

      private IMarkdownBuilder findBuilderFor(Type type)
      {
         return _builders.FirstOrDefault(b => b.SupportedType == type)
                ?? _builders.FirstOrDefault(b => b.SupportedType.IsAssignableFrom(type));
      }
   }
}
