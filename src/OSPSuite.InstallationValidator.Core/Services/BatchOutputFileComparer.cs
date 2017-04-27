using OSPSuite.InstallationValidator.Core.Domain;

namespace OSPSuite.InstallationValidator.Core.Services
{
   public interface IBatchOutputFileComparer
   {
      OutputFileComparisonResult Compare(string fileName, string folderPath1, string folderPath2);
   }

   public class BatchOutputFileComparer : IBatchOutputFileComparer
   {
      public OutputFileComparisonResult Compare(string fileName, string folderPath1, string folderPath2)
      {
         return new OutputFileComparisonResult(fileName, folderPath1, folderPath2);
      }
   }
}