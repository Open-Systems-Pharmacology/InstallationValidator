using OSPSuite.InstallationValidator.Core.Domain;

namespace OSPSuite.InstallationValidator.Core.Services
{
   public interface IStartableProcessFactory
   {
      StartableProcess CreateStartableProcess(string filePath, string arguments = null);
   }
}
