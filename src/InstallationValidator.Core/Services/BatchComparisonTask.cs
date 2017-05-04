using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentNHibernate.Utils;
using InstallationValidator.Core.Assets;
using InstallationValidator.Core.Domain;
using static OSPSuite.Core.Domain.Constants;

namespace InstallationValidator.Core.Services
{
   public interface IBatchComparisonTask
   {
      Task<BatchComparisonResult> StartComparison(string folderPath, CancellationToken token);
      Task<BatchComparisonResult> StartComparison(string folderPath1, string folderPath2, CancellationToken token, string folderPathCaption1 = Captions.Old, string folderPathCaption2 = Captions.New);
   }

   public class BatchComparisonTask : IBatchComparisonTask
   {
      private readonly IInstallationValidatorConfiguration _validatorConfiguration;
      private readonly IFolderInfoFactory _folderInfoFactory;
      private readonly IBatchOutputFileComparer _batchOutputFileComparer;
      private readonly IValidationLogger _validationLogger;

      public BatchComparisonTask(IInstallationValidatorConfiguration validatorConfiguration, IFolderInfoFactory folderInfoFactory, IBatchOutputFileComparer batchOutputFileComparer, IValidationLogger validationLogger)
      {
         _validatorConfiguration = validatorConfiguration;
         _folderInfoFactory = folderInfoFactory;
         _batchOutputFileComparer = batchOutputFileComparer;
         _validationLogger = validationLogger;
      }

      public Task<BatchComparisonResult> StartComparison(string folderPath, CancellationToken token)
      {
         return StartComparison(_validatorConfiguration.BatchOutputsFolderPath, folderPath, token, Captions.Installation, Captions.Computed);
      }

      public async Task<BatchComparisonResult> StartComparison(string folderPath1, string folderPath2, CancellationToken token, string folderPathCaption1, string folderPathCaption2)
      {
         var folderInfo1 = _folderInfoFactory.CreateFor(folderPath1, Filter.JSON_FILTER);
         var folderInfo2 = _folderInfoFactory.CreateFor(folderPath2, Filter.JSON_FILTER);

         var tasks = new[] {folderInfo1.ComputeFiles(), folderInfo2.ComputeFiles()};

         await Task.WhenAll(tasks);
         token.ThrowIfCancellationRequested();

         var comparison = new BatchComparisonResult
         {
            FolderPath1 = folderPath1,
            FolderPath2 = folderPath2,
            FolderPathCaption1 = folderPathCaption1,
            FolderPathCaption2 = folderPathCaption2
         };

         comparison.AddFileComparisons(allMissingFilesFrom(folderInfo1, folderInfo2));
         comparison.AddFileComparisons(allMissingFilesFrom(folderInfo2, folderInfo1));
         token.ThrowIfCancellationRequested();

         foreach (var file in folderInfo1.FileNames.Where(folderInfo2.HasFile))
         {
            _validationLogger.AppendLine(Logs.ComparingFilles(file));
            var fileComparison = await compareFile(file, folderPath1, folderPath2, token);
            fileComparison.OutputComparisonResults.Each(x => captionOutputs(x, folderPathCaption1, folderPathCaption2));
            comparison.AddFileComparison(fileComparison);
            _validationLogger.AppendText(Logs.StateDisplayFor(fileComparison.State));
         }

         return comparison;
      }

      private void captionOutputs(OutputComparisonResult outputComparisonResult, string folderPathCaption1, string folderPathCaption2)
      {
         outputComparisonResult.Output1.Caption = folderPathCaption1;
         outputComparisonResult.Output2.Caption = folderPathCaption2;
      }

      private Task<OutputFileComparisonResult> compareFile(string fileName, string folderPath1, string folderPath2, CancellationToken token)
      {
         return _batchOutputFileComparer.Compare(fileName, folderPath1, folderPath2, token);
      }

      private IEnumerable<FileComparisonResult> allMissingFilesFrom(FolderInfo folderInfo1, FolderInfo folderInfo2)
      {
         return folderInfo1.FileNames.Where(x => !folderInfo2.HasFile(x))
            .Select(x => new MissingFileComparisonResult(x, folderInfo1.Folder, folderInfo2.Folder));
      }
   }
}