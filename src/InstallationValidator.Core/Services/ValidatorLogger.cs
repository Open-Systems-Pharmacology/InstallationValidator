using System.Diagnostics;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Services;

namespace InstallationValidator.Core.Services
{
   public class ValidatorLogger : ILogger
   {
      public void AddToLog(string message, NotificationType messageStatus = NotificationType.None)
      {
         Debug.Print($"{messageStatus} - {message}");
      }
   }
}