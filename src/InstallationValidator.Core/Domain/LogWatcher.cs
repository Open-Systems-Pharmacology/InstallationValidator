using System;
using System.IO;
using OSPSuite.Utility;

namespace InstallationValidator.Core.Domain
{
   public interface ILogWatcher: IDisposable
   {
      void Watch();
   }
   public class LogWatcher : ILogWatcher
   {
      private readonly string _logFile;
      private readonly IValidationLogger _validationLogger;
      private FileSystemWatcher _fileSystemWatcher;
      private StreamReader _sr;

      public LogWatcher(string logFile, IValidationLogger validationLogger)
      {
         _logFile = logFile;
         _validationLogger = validationLogger;

         configureFileSystemWatcher(logFile);

         subscribe();
      }

      private void configureFileSystemWatcher(string logFile)
      {
         var folderFromFileFullPath = FileHelper.FolderFromFileFullPath(logFile);
         var fileNameEndExtension = Path.GetFileName(_logFile);

         FileHelper.DeleteFile(logFile);
         _fileSystemWatcher = new FileSystemWatcher(string.IsNullOrEmpty(folderFromFileFullPath) ? "./" : folderFromFileFullPath, fileNameEndExtension)
         {
            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime | NotifyFilters.FileName | NotifyFilters.Size
         };
      }

      private void subscribe()
      {
         _fileSystemWatcher.Changed += onChanged;
         _fileSystemWatcher.Created += onCreated;
      }

      private void unsubscribe()
      {
         _fileSystemWatcher.Changed -= onChanged;
         _fileSystemWatcher.Created -= onCreated;
      }

      private void onCreated(object sender, FileSystemEventArgs e)
      {
         var logFileStream = new FileStream(_logFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
         _sr = new StreamReader(logFileStream);

         appendTextToLog();
      }

      private string readFileText()
      {
         return _sr.ReadToEnd();
      }

      private void onChanged(object sender, FileSystemEventArgs e)
      {
         appendTextToLog();
      }

      private void appendTextToLog()
      {
         _validationLogger.AppendRawText(readFileText());
      }

      public virtual void Watch()
      {
         _fileSystemWatcher.EnableRaisingEvents = true;
      }

      private bool _disposed;

      public void Dispose()
      {
         if (_disposed) return;

         Cleanup();
         GC.SuppressFinalize(this);
         _disposed = true;
      }

      ~LogWatcher()
      {
         Cleanup();
      }

      protected virtual void Cleanup()
      {
         unsubscribe();
         _fileSystemWatcher.Dispose();
         _sr?.Close();
         _sr?.Dispose();
      }
   }
}