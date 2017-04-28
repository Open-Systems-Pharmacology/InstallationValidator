namespace OSPSuite.InstallationValidator.Core.Extensions
{
   public static class StringExtensions
   {
      public static string SurroundWith(this string stringToSurround, string surroundString)
      {
         return $"{surroundString}{stringToSurround}{surroundString}";
      }

      public static string InQuotes(this string stringToSurround)
      {
         return stringToSurround.SurroundWith("\"");
      }
   }
}
