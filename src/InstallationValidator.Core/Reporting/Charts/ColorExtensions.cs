using System.Drawing;

namespace InstallationValidator.Core.Reporting.Charts
{
   public static class ColorExtensions
   {
      public static string ToHexString(this Color color)
         => $"#{color.R:X2}{color.G:X2}{color.B:X2}";
   }
}
