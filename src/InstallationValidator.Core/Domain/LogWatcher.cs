using System;
using System.Collections.Generic;
using System.IO;
using FluentNHibernate.Utils;
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
      private bool _disposed;
      private readonly string[] _foldersToWatch;
      private readonly List<FileSystemWatcher> _fileSystemWatchers;
      private StreamReader _sr;
      private FileSystemWatcher _logFileWatcher;

      /// <summary>
      ///    Creates a log file watcher that will update the <paramref name="validationLogger"/> when either the <paramref name="logFile"/> changes
      ///    or when one of the <paramref name="additionalFoldersToWatch"/> changes
      /// </summary>
      public LogWatcher(string logFile, IValidationLogger validationLogger, params string[] additionalFoldersToWatch)
      {
         _logFile = logFile;
         _foldersToWatch = additionalFoldersToWatch;
         _validationLogger = validationLogger;
         _fileSystemWatchers = new List<FileSystemWatcher>();
         configureFileSystemWatcher(logFile);

         subscribe();
      }

      private void configureFileSystemWatcher(string logFile)
      {
         var folderFromFileFullPath = FileHelper.FolderFromFileFullPath(logFile);
         var fileNameEndExtension = Path.GetFileName(_logFile);

         FileHelper.DeleteFile(logFile);

         _logFileWatcher = new FileSystemWatcher(string.IsNullOrEmpty(folderFromFileFullPath) ? "./" : folderFromFileFullPath, fileNameEndExtension)
         {
            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime | NotifyFilters.FileName | NotifyFilters.Size
         };

         _fileSystemWatchers.Add(_logFileWatcher);

         _foldersToWatch.Each(folder =>
         {
            _fileSystemWatchers.Add(new FileSystemWatcher(folder, "*.json"));
         });
         
      }

      private void forAllWatchers(Action<FileSystemWatcher> actionForFileSystemWatcher)
      {
         _fileSystemWatchers.Each(actionForFileSystemWatcher);
      }

      private void subscribe()
      {
         _logFileWatcher.Created += onCreated;
         forAllWatchers(watcher =>
         {
            watcher.Changed += onChanged;
         });
      }

      private void unsubscribe()
      {
         _logFileWatcher.Created -= onCreated;
         forAllWatchers(watcher =>
         {
            watcher.Changed -= onChanged;
         });
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
         if(logFileCreated())
            appendTextToLog();
      }

      private bool logFileCreated()
      {
         return _sr != null;
      }

      private void appendTextToLog()
      {
         _validationLogger.AppendRawText(readFileText());
      }

      public virtual void Watch()
      {
         forAllWatchers(watcher => watcher.EnableRaisingEvents = true);
      }

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
         forAllWatchers(watcher => watcher.Dispose());
         _sr?.Close();
         _sr?.Dispose();
      }
   }
}