namespace OSPSuite.InstallationValidator.Core.Assets
{
   public static class Constants
   {
      public class Captions
      {
         public static readonly string OutputFolder = "Output Folder";
         public static readonly string SelectOutputFolder = "Select Output Folder";
      }

      public class Validation
      {
            public static string FolderDoesNotExist(string fileFullPath)
            {
               return $"Folder '{fileFullPath}' does not exist";
            }
         }
   }
}
