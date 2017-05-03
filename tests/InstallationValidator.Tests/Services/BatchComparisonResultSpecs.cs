using InstallationValidator.Core.Domain;
using InstallationValidator.Helpers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;

namespace InstallationValidator.Services
{
   public abstract class concern_for_BatchComparisonResult : ContextSpecification<BatchComparisonResult>
   {
      protected override void Context()
      {
         sut = new BatchComparisonResult();
      }
   }

   public class When_computing_the_state_of_a_batch_comparison_result : concern_for_BatchComparisonResult
   {
      [Observation]
      public void should_return_valid_if_no_file_was_compared()
      {
         sut.State.ShouldBeEqualTo(ValidationState.Valid);
      }

      [Observation]
      public void should_return_valid_if_all_file_comparisons_are_valid()
      {
         sut.AddFileComparison(new StateFileComparisonResult(ValidationState.Valid));
         sut.AddFileComparison(new StateFileComparisonResult(ValidationState.Valid));
         sut.State.ShouldBeEqualTo(ValidationState.Valid);
      }

      [Observation]
      public void should_return_valid_with_warning_if_at_least_ine_file_comparisons_has_warning()
      {
         sut.AddFileComparison(new StateFileComparisonResult(ValidationState.Valid));
         sut.AddFileComparison(new StateFileComparisonResult(ValidationState.ValidWithWarnings));
         sut.State.ShouldBeEqualTo(ValidationState.ValidWithWarnings);
      }

      [Observation]
      public void should_return_invalid_if_at_least_ine_file_comparisons_is_invalid()
      {
         sut.AddFileComparison(new StateFileComparisonResult(ValidationState.Valid));
         sut.AddFileComparison(new StateFileComparisonResult(ValidationState.ValidWithWarnings));
         sut.AddFileComparison(new StateFileComparisonResult(ValidationState.Invalid));
         sut.State.ShouldBeEqualTo(ValidationState.Invalid);
      }
   }
}