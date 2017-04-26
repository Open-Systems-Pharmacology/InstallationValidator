using OSPSuite.InstallationValidator.Core.Domain;

namespace OSPSuite.InstallationValidator.Core.Services
{
   public interface ILogWatcherFactory
   {
      ILogWatcher CreateLogWatcher(string logFile);
   }
}