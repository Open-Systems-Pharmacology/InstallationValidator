using System;
using System.Diagnostics;

namespace InstallationValidator.Core
{
   public static class ValidationFileHelper
   {
      // TODO - move to Utility
      public static Func<string,string> GetVersion = binaryExecutablePath => FileVersionInfo.GetVersionInfo(binaryExecutablePath).ProductVersion;
   }
}
