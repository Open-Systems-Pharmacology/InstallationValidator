using InstallationValidator.Core.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO;
using OSPSuite.Utility;
using OSPSuite.Utility.Validation;

namespace InstallationValidator.Core.Presentation.DTO
{
   public class FolderDTO : ValidatableDTO
   {
      private string _folderPath;

      public string FolderPath
      {
         get { return _folderPath; }
         set
         {
            _folderPath = value;
            OnPropertyChanged();
         }
      }

      public FolderDTO()
      {
         Rules.Add(AllRules.TargetFolderNotEmpty);
         Rules.Add(AllRules.TargetFolderExists);
      }

      private static class AllRules
      {
         public static IBusinessRule TargetFolderNotEmpty
         {
            get { return GenericRules.NonEmptyRule<FolderDTO>(x => x.FolderPath); }
         }

         public static IBusinessRule TargetFolderExists
         {
            get
            {
               return CreateRule.For<FolderDTO>()
                  .Property(x => x.FolderPath)
                  .WithRule((item, folderPath) => DirectoryHelper.DirectoryExists(folderPath))
                  .WithError((item, folderPath) => Validation.FolderDoesNotExist(folderPath));
            }
         }
      }
   }
}