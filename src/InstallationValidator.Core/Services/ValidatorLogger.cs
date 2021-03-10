using System.Diagnostics;
using Microsoft.Extensions.Logging;
using OSPSuite.Core.Services;

namespace InstallationValidator.Core.Services
{
   public class ValidatorLogger : IOSPSuiteLogger
   {
      public void AddToLog(string message, LogLevel logLevel, string categoryName)
      {
         Debug.Print($"{logLevel} - {message}");
      }
   }
}