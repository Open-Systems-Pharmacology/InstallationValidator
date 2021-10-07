using InstallationValidator.Core.Domain;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using static OSPSuite.Core.Domain.Constants;
namespace InstallationValidator.Domain
{
   public abstract class concern_for_ComparisonSettings : ContextSpecification<ComparisonSettings>
   {
      protected string _path;

      protected override void Context()
      {
         sut = new ComparisonSettings();
         _path = "Organism|Liver|Intracellular|Drug|Concentration";
      }
   }

   public class When_checking_if_a_path_can_be_used_for_comparison_when_no_exclusion_is_defined : concern_for_ComparisonSettings
   {
      [Observation]
      public void should_be_able_to_compare()
      {
         sut.CanCompare(_path).ShouldBeTrue();
      }
   }

   public class When_checking_if_a_path_can_be_used_for_comparison_when_the_exclusion_list_does_not_contain_a_matching_entry : concern_for_ComparisonSettings
   {
      protected override void Context()
      {
         base.Context();
         sut.Exclusions = new[] {"Organism|Liver|Interstitial|Drug|Concentration"};
      }

      [Observation]
      public void should_be_able_to_compare()
      {
         sut.CanCompare(_path).ShouldBeTrue();
      }
   }

   public class When_checking_if_a_path_can_be_used_for_comparison_when_the_exclusion_list_does_contain_a_matching_entry : concern_for_ComparisonSettings
   {
      protected override void Context()
      {
         base.Context();
         sut.Exclusions = new[] { _path };
      }

      [Observation]
      public void should_not_be_able_to_compare()
      {
         sut.CanCompare(_path).ShouldBeFalse();
      }
   }

   public class When_checking_if_a_path_can_be_used_for_comparison_when_the_exclusion_list_does_contain_a_matching_entry_by_regex : concern_for_ComparisonSettings
   {
      [Observation]
      public void should_not_be_able_to_compare()
      {
         sut.Exclusions = new[] {WILD_CARD_RECURSIVE  };
         sut.CanCompare(_path).ShouldBeFalse();

         sut.Exclusions = new[] { "**|Drug|**" };
         sut.CanCompare(_path).ShouldBeFalse();

         sut.Exclusions = new[] { "**|Intra*|**" };
         sut.CanCompare(_path).ShouldBeFalse();
      }
   }
}