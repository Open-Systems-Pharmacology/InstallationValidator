using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OSPSuite.InstallationValidator.Core.Domain;
namespace OSPSuite.InstallationValidator.Core.Services
{
   public interface IBatchComparisonTask
   {
      Task<BatchComparisonResult> StartComparison(string folderPath, CancellationToken token);
      Task<BatchComparisonResult> StartComparison(string folderPath1, string folderPath2, CancellationToken token);
   }

   public class BatchComparisonTask : IBatchComparisonTask
   {
      private readonly IInstallationValidatorConfiguration _validatorConfiguration;
      private readonly IFolderInfoFactory _folderInfoFactory;
      private readonly IBatchOutputFileComparer _batchOutputFileComparer;

      public BatchComparisonTask(IInstallationValidatorConfiguration validatorConfiguration, IFolderInfoFactory folderInfoFactory, IBatchOutputFileComparer batchOutputFileComparer)
      {
         _validatorConfiguration = validatorConfiguration;
         _folderInfoFactory = folderInfoFactory;
         _batchOutputFileComparer = batchOutputFileComparer;
      }

      public Task<BatchComparisonResult> StartComparison(string folderPath, CancellationToken token)
      {
         return StartComparison(_validatorConfiguration.BatchOutputsFolderPath, folderPath, token);
      }

      public async Task<BatchComparisonResult> StartComparison(string folderPath1, string folderPath2, CancellationToken token)
      {
         var folderInfo1 = _folderInfoFactory.CreateFor(folderPath1, Constants.Filter.JSON_FILTER);
         var folderInfo2 = _folderInfoFactory.CreateFor(folderPath2, Constants.Filter.JSON_FILTER);

         var tasks = new[] {folderInfo1.ComputeFiles(), folderInfo2.ComputeFiles()};

         await Task.WhenAll(tasks);
         token.ThrowIfCancellationRequested();

         var comparison = new BatchComparisonResult
         {
            FolderPath1 = folderPath1,
            FolderPath2 = folderPath2
         };

         comparison.AddFileComparisons(allMissingFilesFrom(folderInfo1, folderInfo2));
         comparison.AddFileComparisons(allMissingFilesFrom(folderInfo2, folderInfo1));
         token.ThrowIfCancellationRequested();

         foreach (var file in folderInfo1.FileNames.Where(folderInfo2.HasFile))
         {
            token.ThrowIfCancellationRequested();
            comparison.AddFileComparison(compareFile(file, folderPath1, folderPath2));
         }

         return comparison;
      }

      private FileComparisonResult compareFile(string fileName, string folderPath1, string folderPath2)
      {
         return _batchOutputFileComparer.Compare(fileName, folderPath1, folderPath2);
      }

      private IEnumerable<FileComparisonResult> allMissingFilesFrom(FolderInfo folderInfo1, FolderInfo folderInfo2)
      {
         return folderInfo1.FileNames.Where(x => !folderInfo2.HasFile(x))
            .Select(x => new MissingFileComparisonResult(x, folderInfo1.Folder, folderInfo2.Folder));
      }
   }
}