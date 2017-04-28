using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.InstallationValidator.Core.Domain
{
   public class FolderInfo
   {
      private readonly string _filter;
      protected readonly List<string> _fileNames = new List<string>();
      public string Folder { get; }

      public FolderInfo(string folder, string filter = null)
      {
         _filter = filter;
         Folder = folder;
      }

      public IReadOnlyList<string> FileNames => _fileNames;

      public Task ComputeFiles()
      {
         return Task.Run(() => { AddAllFilesInFolder(); });
      }

      protected virtual void AddAllFilesInFolder()
      {
         var directory = new DirectoryInfo(Folder);
         var allProjectFiles = directory.GetFiles(_filter, SearchOption.TopDirectoryOnly);
         allProjectFiles.Each(f => { _fileNames.Add(f.Name); });
      }

      public bool HasFile(string fileName)
      {
         return _fileNames.Contains(fileName);
      }
   }
}