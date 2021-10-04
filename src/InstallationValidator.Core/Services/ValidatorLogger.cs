using System.Diagnostics;
using Microsoft.Extensions.Logging;
using OSPSuite.Core.Services;

namespace InstallationValidator.Core.Services
{
   public class ValidatorLogger : IOSPSuiteLogger
   {
      public string DefaultCategoryName { get; set; } = Constants.PRODUCT_NAME;

      public void AddToLog(string message, LogLevel logLevel, string categoryName)
      {
         Debug.Print($"{logLevel} - {message}");
      }

   }
}