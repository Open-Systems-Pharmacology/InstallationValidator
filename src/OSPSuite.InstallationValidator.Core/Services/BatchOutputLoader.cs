using System.IO;
using Newtonsoft.Json;
using OSPSuite.Core.Batch;

namespace OSPSuite.InstallationValidator.Core.Services
{
   public interface IBatchOutputLoader
   {
      BatchSimulationExport LoadFrom(string fileName);
   }

   public class BatchOutputLoader : IBatchOutputLoader
   {
      public BatchSimulationExport LoadFrom(string fileName)
      {
         var serializer = new JsonSerializer();

         using (var sr = new StreamReader(fileName))
         using (var reader = new JsonTextReader(sr))
         {
            return serializer.Deserialize<BatchSimulationExport>(reader);
         }
      }
   }
}