using InstallationValidator.Core.Domain;

namespace InstallationValidator.Core.Services
{
   public interface IStartableProcessFactory
   {
      StartableProcess CreateStartableProcess(string filePath, params string[] arguments);
   }
}
