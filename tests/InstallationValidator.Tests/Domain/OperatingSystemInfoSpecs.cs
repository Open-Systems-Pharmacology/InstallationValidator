using InstallationValidator.Core.Domain;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;

namespace InstallationValidator.Domain
{
   public class TestableOperatingSystemInfo : OperatingSystemInfo
   {
      public string FixProductNameForWindows11(string productName, string currentBuildNumber)
      {
         // Use reflection to access the private method
         var method = typeof(OperatingSystemInfo).GetMethod("fixProductNameForWindows11", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
         if (method != null) 
            return (string)method.Invoke(this, new object[] { productName, currentBuildNumber });
         else
            return string.Empty;
      }
   }

   public abstract class concern_for_OperatingSystemInfo : ContextSpecification<TestableOperatingSystemInfo>
   {
      protected override void Context()
      {
         sut = new TestableOperatingSystemInfo();
      }
   }

   public class When_adjusting_the_product_info : concern_for_OperatingSystemInfo
   {
      private const string WINDOWS10_BUILD_NUMBER = "21000";
      private const string WINDOWS11_BUILD_NUMBER = "22000";

      [Observation]
      public void Should_return_original_product_name_when_current_build_number_is_null_or_empty()
      {
         sut.FixProductNameForWindows11("Windows 10 Pro", null).ShouldBeEqualTo("Windows 10 Pro");
         sut.FixProductNameForWindows11("Windows 10 Pro", "").ShouldBeEqualTo("Windows 10 Pro");
      }

      [Observation]
      public void Should_return_original_product_name_when_product_name_contains_server()
      {
         sut.FixProductNameForWindows11("Windows Server 2019", WINDOWS10_BUILD_NUMBER).ShouldBeEqualTo("Windows Server 2019");
         sut.FixProductNameForWindows11("Windows Server 2016", WINDOWS11_BUILD_NUMBER).ShouldBeEqualTo("Windows Server 2016");
      }

      [Observation]
      public void Should_replace_windows_10_with_windows_11_when_build_number_is_at_least_22000()
      {
         sut.FixProductNameForWindows11("Windows 10 Home", WINDOWS11_BUILD_NUMBER).ShouldBeEqualTo("Windows 11 Home");
      }

      [Observation]
      public void Should_return_original_product_name_when_build_number_is_less_than_22000()
      {
         sut.FixProductNameForWindows11("Windows 10 Pro", WINDOWS10_BUILD_NUMBER).ShouldBeEqualTo("Windows 10 Pro");
      }

      [Observation]
      public void Should_return_original_product_name_when_build_number_is_not_a_number()
      {
         sut.FixProductNameForWindows11("Windows 10 Pro", "notanumber").ShouldBeEqualTo("Windows 10 Pro");
      }
   }

}