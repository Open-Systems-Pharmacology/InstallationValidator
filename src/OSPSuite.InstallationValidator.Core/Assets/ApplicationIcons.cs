using System.Drawing;
using OSPSuite.Assets;
using OSPSuite.InstallationValidator.Core.Properties;

namespace OSPSuite.InstallationValidator.Core.Assets
{
   public static class ApplicationIcons
   {
      public static readonly ApplicationIcon ValidationToolIcon = createIconFrom(Icons.ValidationToolIcon, "ValidationToolIcon");

      private static ApplicationIcon createIconFrom(Icon icon, string iconName)
      {
         return new ApplicationIcon(icon)
         {
            IconName = iconName.ToUpperInvariant(),
         };
      }
   }
}