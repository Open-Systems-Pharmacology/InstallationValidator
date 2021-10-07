using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InstallationValidator.Core.Assets;
using InstallationValidator.Core.Domain;
using OSPSuite.Utility.Extensions;
using static OSPSuite.Core.Domain.Constants;

namespace InstallationValidator.Core.Services
{
   public interface IBatchComparisonTask
   {
      Task<BatchComparisonResult> StartComparison(string folderPath, CancellationToken token);
      Task<BatchComparisonResult> StartComparison(ComparisonSettings comparisonSettings, CancellationToken token);
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
         return StartComparison(new ComparisonSettings
         {
            FolderPath1 = _validatorConfiguration.BatchOutputsFolderPath,
            FolderPath2 = folderPath,
            FolderPathCaption1 = Captions.Installation,
            FolderPathCaption2 = Captions.Computed,
            GenerateResultsForValidSimulation = true,
            PredefinedOutputPaths = Constants.PREDEFINED_OUTPUT_PATHS
         }, token);
      }

      public async Task<BatchComparisonResult> StartComparison(ComparisonSettings comparisonSettings, CancellationToken token)
      {
         var folderInfo1 = _folderInfoFactory.CreateFor(comparisonSettings.FolderPath1, Filter.JSON_FILTER);
         var folderInfo2 = _folderInfoFactory.CreateFor(comparisonSettings.FolderPath2, Filter.JSON_FILTER);

         var tasks = new[] {folderInfo1.ComputeFiles(), folderInfo2.ComputeFiles()};

         await Task.WhenAll(tasks);
         token.ThrowIfCancellationRequested();

         var comparison = new BatchComparisonResult
         {
            ComparisonSettings = comparisonSettings
         };

         comparison.AddFileComparisons(allMissingFilesFrom(folderInfo1, folderInfo2));
         comparison.AddFileComparisons(allMissingFilesFrom(folderInfo2, folderInfo1));
         token.ThrowIfCancellationRequested();

         if (comparisonSettings.Exclusions.Any())
         {
            _validationLogger.AppendLine(Logs.UsingExclusions);
            comparisonSettings.Exclusions.Each(x => _validationLogger.AppendLine(x));
            _validationLogger.AppendLine();
         }

         foreach (var file in folderInfo1.FileNames.Where(folderInfo2.HasFile))
         {
            _validationLogger.AppendLine(Logs.ComparingFiles(file));
            var fileComparison = await compareFile(file, comparisonSettings, token);
            comparison.AddFileComparison(fileComparison);
            _validationLogger.AppendText(Logs.StateDisplayFor(fileComparison.State));
         }

         _validationLogger.AppendLine();
         _validationLogger.AppendLine($"{Logs.NumberOfComparedFiles} {comparison.NumberOfComparedFiles}");
         _validationLogger.AppendLine($"{Logs.OverallComparisonResult} {Logs.StateDisplayFor(comparison.State)}");

         return comparison;
      }

      private Task<OutputFileComparisonResult> compareFile(string fileName, ComparisonSettings comparisonSettings, CancellationToken token)
      {
         return _batchOutputFileComparer.Compare(fileName, comparisonSettings, token);
      }

      private IEnumerable<FileComparisonResult> allMissingFilesFrom(FolderInfo folderInfo1, FolderInfo folderInfo2)
      {
         return folderInfo1.FileNames.Where(x => !folderInfo2.HasFile(x))
            .Select(x => new MissingFileComparisonResult(x, folderInfo1.Folder, folderInfo2.Folder));
      }
   }
}