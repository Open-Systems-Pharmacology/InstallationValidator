using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO;
using OSPSuite.Utility;
using OSPSuite.Utility.Validation;

namespace OSPSuite.InstallationValidator.Core.Presentation.DTO
{
   public class FolderDTO : ValidatableDTO
   {
      private string _targetFolder;

      public string TargetFolder
      {
         get { return _targetFolder; }
         set
         {
            _targetFolder = value;
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
            get { return GenericRules.NonEmptyRule<FolderDTO>(x => x.TargetFolder); }
         }

         public static IBusinessRule TargetFolderExists
         {
            get
            {
               return CreateRule.For<FolderDTO>()
                  .Property(x => x.TargetFolder)
                  .WithRule((item, folderPath) => DirectoryHelper.DirectoryExists(folderPath))
                  .WithError((item, folderPath) => Assets.Constants.Validation.FolderDoesNotExist(folderPath));
            }
         }
      }
   }
}