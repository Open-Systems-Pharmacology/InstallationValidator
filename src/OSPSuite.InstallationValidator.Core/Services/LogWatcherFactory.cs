using OSPSuite.InstallationValidator.Core.Domain;

namespace OSPSuite.InstallationValidator.Core.Services
{
   public interface ILogWatcherFactory
   {
      LogWatcher CreateLogWatcher(string logFile);
   }
}