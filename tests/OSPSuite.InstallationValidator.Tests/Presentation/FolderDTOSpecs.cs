using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.InstallationValidator.Core.Presentation.DTO;
using OSPSuite.Utility;
using OSPSuite.Utility.Validation;

namespace OSPSuite.InstallationValidator.Presentation
{
   public abstract class concern_for_FolderDTO : ContextSpecification<FolderDTO>
   {
      protected override void Context()
      {
         sut = new FolderDTO();
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

   public class When_the_target_directory_does_not_exist : concern_for_FolderDTO
   {
      protected override void Context()
      {
         base.Context();
         DirectoryHelper.DirectoryExists = path => false;
      }

      [Observation]
      public void the_dto_is_invalidated()
      {
         sut.IsValid().ShouldBeEqualTo(false);
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
