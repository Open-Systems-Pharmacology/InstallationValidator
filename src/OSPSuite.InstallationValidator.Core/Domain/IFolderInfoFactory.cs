namespace OSPSuite.InstallationValidator.Core.Domain
{
   public interface IFolderInfoFactory
   {
      FolderInfo CreateFor(string folder, string filter);
   }
}