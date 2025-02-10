using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using OSPSuite.Core.Batch;
using OSPSuite.Core.Domain.UnitSystem;

namespace InstallationValidator.Core.Services
{
   public interface IBatchOutputLoader
   {
      BatchSimulationExport LoadFrom(string fileName);
   }

   public class BatchOutputLoader : IBatchOutputLoader
   {
      private readonly IDimensionFactory _dimensionFactory;

      public BatchOutputLoader(IDimensionFactory dimensionFactory)
      {
         _dimensionFactory = dimensionFactory;
      }

      public BatchSimulationExport LoadFrom(string fileName)
      {
         var serializer = new JsonSerializer();

         try
         {
            using (var sr = new StreamReader(fileName))
            using (var reader = new JsonTextReader(sr))
            {
               return serializer.Deserialize<BatchSimulationExport>(reader);
            }
         }
         catch (Exception e)
         {
            //try to load the old way and convert to new way. Temporary for v11
            //required to load again since the reader was consumed
            using (var sr = new StreamReader(fileName))
            using (var reader = new JsonTextReader(sr))
            {
               var oldBatchSimulationExport = serializer.Deserialize<OldBatchSimulationExport>(reader);
               return mapToBatchSimulationExport(oldBatchSimulationExport);
            }
         }
      }

      private BatchSimulationExport mapToBatchSimulationExport(OldBatchSimulationExport oldBatchSimulationExport)
      {
         //convert to batch simulation export
         var batchSimulationExport = new BatchSimulationExport
         {
            Name = oldBatchSimulationExport.Name,
            AbsTol = oldBatchSimulationExport.AbsTol,
            RelTol = oldBatchSimulationExport.RelTol,
            Times = new BatchValues
            {
               Unit = "h",
               Values = oldBatchSimulationExport.Times,
               Dimension = "Time"
            },
            ParameterValues = oldBatchSimulationExport.ParameterValues,
         };

         foreach (var outputValues in oldBatchSimulationExport.OutputValues)
         {
            var dimension = _dimensionFactory.Dimension(outputValues.Dimension);
            batchSimulationExport.OutputValues.Add(new BatchOutputValues
            {
               Path = outputValues.Path,
               Values = outputValues.Values,
               Dimension = outputValues.Dimension,
               Unit = dimension.DefaultUnitName,
               ComparisonThreshold = outputValues.ComparisonThreshold
            });
         }

        

         return batchSimulationExport;
      }
   }

   class OldBatchSimulationExport
   {
      public string Name { get; set; }

      /// <summary>
      ///    Time Array used in the simulation
      /// </summary>
      public float[] Times { get; set; }

      /// <summary>
      ///    Absolute tolerance used in the simulation
      /// </summary>
      public double AbsTol { get; set; }

      /// <summary>
      ///    Relative tolerance used in the simulation
      /// </summary>
      public double RelTol { get; set; }

      public List<BatchOutputValues> OutputValues { get; set; } = new List<BatchOutputValues>();
      public List<ParameterValue> ParameterValues { get; set; } = new List<ParameterValue>();
   }


}