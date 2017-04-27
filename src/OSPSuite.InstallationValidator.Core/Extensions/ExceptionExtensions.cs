using System;
using System.Reflection;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.InstallationValidator.Core.Extensions
{
   // TODO - move to core and refactor ExceptionManager to use extensions
   public static class ExceptionExtensions
   {
      public static string ExceptionMessage(this Exception ex)
      {
         if (isWrapperException(ex))
            return ExceptionMessage(ex.InnerException);

         return $"{ex.FullMessage()}{Environment.NewLine}{Environment.NewLine}{Captions.ContactSupport(OSPSuite.Core.Domain.Constants.FORUM_SITE)}";
      }

      private static bool isWrapperException(Exception ex)
      {
         return ex.IsAnImplementationOf<TargetInvocationException>() || ex.IsAnImplementationOf<AggregateException>();
      }

      public static string ExceptionMessageWithStackTrace(this Exception ex)
      {
         return $"{ExceptionMessage(ex)}{Environment.NewLine}{Environment.NewLine}Stack trace:{Environment.NewLine}{ex.FullStackTrace()}";
      }
   }
}
