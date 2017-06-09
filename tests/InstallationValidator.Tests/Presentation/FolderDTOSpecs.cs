using System;
using InstallationValidator.Core.Presentation.DTO;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility;
using OSPSuite.Utility.Validation;

namespace InstallationValidator.Presentation
{
   public abstract class concern_for_FolderDTO : ContextSpecification<FolderDTO>
   {
      private Func<string, bool> _directoryExists;

      public override void GlobalCleanup()
      {
         base.GlobalCleanup();
         DirectoryHelper.DirectoryExists = _directoryExists;
      }

      public override void GlobalContext()
      {
         base.GlobalContext();
         _directoryExists = DirectoryHelper.DirectoryExists;
      }

      protected override void Context()
      {
         sut = new FolderDTO();
      }
   }

   public class When_the_target_directory_path_is_too_long : concern_for_FolderDTO
   {
      protected override void Context()
      {
         sut = new FolderDTO {FolderPath = "A very long path with many, many, many, many, " +
                                                                    "many, many, many, many, many, many, many, many, " +
                                                                    "many, many, many, many, many, many characters"
         };
         DirectoryHelper.DirectoryExists = path => true;
      }

      [Observation]
      public void the_dto_is_invalidated()
      {
         sut.IsValid().ShouldBeEqualTo(false);
      }
   }

   public class When_the_target_directory_is_empty : concern_for_FolderDTO
   {
      protected override void Context()
      {
         base.Context();
         DirectoryHelper.DirectoryExists = path => true;
      }

      [Observation]
      public void the_dto_is_invalidated()
      {
         sut.IsValid().ShouldBeEqualTo(false);
      }
   }

   public class When_the_target_directory_does_not_exist_and_target_folder_should_exist : concern_for_FolderDTO
   {
      protected override void Context()
      {
         base.Context();
         sut.FolderPath = "a folder";
         DirectoryHelper.DirectoryExists = path => false;
      }

      [Observation]
      public void the_dto_is_invalidated()
      {
         sut.IsValid().ShouldBeEqualTo(false);
      }
   }

   public class When_the_target_directory_does_not_exist_and_target_folder_does_not_have_to_exist : concern_for_FolderDTO
   {
      protected override void Context()
      {
         base.Context();
         sut = new FolderDTO(folderMustExist: false) {FolderPath = "a folder"};
         DirectoryHelper.DirectoryExists = path => false;
      }

      [Observation]
      public void the_dto_is_invalidated()
      {
         sut.IsValid().ShouldBeEqualTo(true);
      }
   }

   public class When_the_target_directory_does_exist : concern_for_FolderDTO
   {
      protected override void Context()
      {
         base.Context();
         DirectoryHelper.DirectoryExists = path => true;
         sut.FolderPath = "a folder";
      }

      [Observation]
      public void the_dto_is_invalidated()
      {
         sut.IsValid().ShouldBeEqualTo(true);
      }
   }
}