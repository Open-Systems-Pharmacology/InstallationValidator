using InstallationValidator.Core.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO;
using OSPSuite.Utility;
using OSPSuite.Utility.Validation;

namespace InstallationValidator.Core.Presentation.DTO
{
   public class FolderComparisonDTO
   {
      public FolderComparisonDTO(FolderDTO firstFolderDTO, FolderDTO secondFolderDTO)
      {
         firstFolder = firstFolderDTO;
         secondFolder = secondFolderDTO;
      }

      public FolderDTO firstFolder { get; }
      public FolderDTO secondFolder { get; }
   }

   public class FolderDTO : ValidatableDTO
   {
      private string _folderPath;

      public FolderDTO(bool folderMustExist = true)
      {
         Rules.Add(AllRules.FolderNotEmpty);
         Rules.Add(AllRules.FolderPathNotTooLong);

         if (folderMustExist)
            Rules.Add(AllRules.FolderExists);
      }

      public string FolderPath
      {
         get { return _folderPath; }
         set
         {
            _folderPath = value;
            OnPropertyChanged();
         }
      }

      private static class AllRules
      {
         private const int MAXIMUM_FOLDER_PATH_LENGTH = 130;

         public static IBusinessRule FolderNotEmpty
         {
            get { return GenericRules.NonEmptyRule<FolderDTO>(x => x.FolderPath); }
         }

         public static IBusinessRule FolderPathNotTooLong
         {
            get
            {
               return CreateRule.For<FolderDTO>()
                  .Property(x => x.FolderPath)
                  .WithRule((item, folderPath) => string.IsNullOrEmpty(folderPath) || folderPath.Length <= MAXIMUM_FOLDER_PATH_LENGTH)
                  .WithError((item, folderPath) => Validation.FolderPathTooLong(folderPath, MAXIMUM_FOLDER_PATH_LENGTH));
            }
         }

         public static IBusinessRule FolderExists
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