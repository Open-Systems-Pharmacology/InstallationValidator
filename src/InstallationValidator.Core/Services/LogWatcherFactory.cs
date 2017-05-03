using InstallationValidator.Core.Domain;

namespace InstallationValidator.Core.Services
{
   public interface ILogWatcherFactory
   {
      ILogWatcher CreateLogWatcher(string logFile);
   }
}