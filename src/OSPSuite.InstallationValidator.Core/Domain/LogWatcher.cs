using System;
using System.IO;
using OSPSuite.InstallationValidator.Core.Events;
using OSPSuite.Utility;
using OSPSuite.Utility.Events;

namespace OSPSuite.InstallationValidator.Core.Domain
{
   public interface ILogWatcher: IDisposable
   {
      void Watch();
   }
   public class LogWatcher : ILogWatcher
   {
      private readonly string _logFile;
      private readonly IEventPublisher _eventPublisher;
      private FileSystemWatcher _fileSystemWatcher;
      private StreamReader _sr;

      public LogWatcher(string logFile, IEventPublisher eventPublisher)
      {
         _logFile = logFile;
         _eventPublisher = eventPublisher;

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

         raiseAppendEvent();
      }

      private string readFileText()
      {
         return _sr.ReadToEnd();
      }

      private void onChanged(object sender, FileSystemEventArgs e)
      {
         raiseAppendEvent();
      }

      private void raiseAppendEvent()
      {
         _eventPublisher.PublishEvent(new LogAppendedEvent(readFileText()));
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