using System;
using System.Diagnostics;

namespace OSPSuite.InstallationValidator.Core.Domain
{
   public class StartableProcess
   {
      private readonly Process _process;
      private bool _exited;

      public StartableProcess(string filePath, string arguments = null)
      {
         _process = new Process
         {
            StartInfo = new ProcessStartInfo
            {
               FileName = filePath,
               Arguments = arguments ?? string.Empty
            }
         };
         subscribe();
      }

      /// <summary>
      /// <returns>true</returns> if the process exited normally. If the process was stopped, returns false
      /// </summary>
      public bool ExitedNormally => _exited;

      /// <summary>
      /// <returns>the exit code of the process</returns> if <see cref="ExitedNormally"/> returns true.
      /// throws an <seealso cref="InvalidOperationException"/> if the process has not exited or was aborted abnormally
      /// </summary>
      public int ReturnCode => _process.ExitCode;

      public virtual void Start()
      {
         _process?.Start();
      }

      public virtual void Stop()
      {
         if (_process == null || _exited)
            return;

         unsubscribe();
         _process.Kill();
         _process.Dispose();
      }

      private void processExitedNormally(object sender, EventArgs eventArgs)
      {
         _exited = true;
         unsubscribe();
      }

      private void subscribe()
      {
         _process.EnableRaisingEvents = true;
         _process.Exited += processExitedNormally;
      }

      private void unsubscribe()
      {
         _process.Exited -= processExitedNormally;
      }
   }
}
