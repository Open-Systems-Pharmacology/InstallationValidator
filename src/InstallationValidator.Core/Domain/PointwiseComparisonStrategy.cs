﻿using System;
using System.Linq;
using InstallationValidator.Core.Assets;
using OSPSuite.Core.Batch;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Extensions;

namespace InstallationValidator.Core.Domain
{
   public class PointwiseComparisonStrategy : IComparisonStrategy
   {
      public OutputComparisonResult CompareOutputs(BatchOutputComparison outputValues1, BatchOutputComparison outputValues2, ComparisonSettings comparisonSettings)
      {
         var comparisonResult = outputConsistencyCheckComparisonResult(outputValues1, outputValues2, comparisonSettings);
         if (comparisonResult != null)
            return comparisonResult;

         var deviation = calculateDeviation(outputValues1, outputValues2);

         return deviation > Constants.MAX_DEVIATION_OUTPUT
            ? outputDeviationTooLargeComparisonResult(outputValues1, outputValues2, deviation, comparisonSettings)
            : validOutputComparison(outputValues1, outputValues2, deviation, comparisonSettings);
      }

      private static OutputComparisonResult validOutputComparison(BatchOutputComparison outputValues1, BatchOutputComparison outputValues2, double deviation, ComparisonSettings comparisonSettings)
      {
         if (!shouldGenerateOutputsFor(comparisonSettings, outputValues1.Path))
            return new OutputComparisonResult(new OutputComparisonResultParams(outputValues1.Path), comparisonSettings, ValidationState.Valid)
            {
               Deviation = deviation
            };

         return outputComparisonResultFrom(outputValues1, outputValues2, deviation, comparisonSettings, ValidationState.Valid);
      }

      private static bool shouldGenerateOutputsFor(ComparisonSettings comparisonSettings, string outputValues1Path)
      {
         //we need to remove one before last entry. So we need at least 3 entries in the path
         var path = outputValues1Path.ToPathArray().ToList();
         if (path.Count < 2)
            return false;

         path.RemoveAt(path.Count - 2);

         var consolidatedPath = path.ToPathString();
         return comparisonSettings.PredefinedOutputPaths.Contains(consolidatedPath);
      }

      public TimeComparisonResult CompareTime(BatchSimulationComparison simulation1, BatchSimulationComparison simulation2, ComparisonSettings comparisonSettings)
      {
         var timeComparison = timeConsistencyCheckComparisonResult(simulation1, simulation2);
         if (timeComparison != null)
            return timeComparison;

         var deviation = calculateDeviation(simulation1.Times, simulation2.Times);

         timeComparison = deviation > Constants.MAX_DEVIATION_TIME
            ? timeDeviationTooLargeComparisonResult(deviation)
            : new TimeComparisonResult(ValidationState.Valid);

         timeComparison.Deviation = deviation;
         return timeComparison;
      }

      private TimeComparisonResult timeConsistencyCheckComparisonResult(BatchSimulationComparison simulation1, BatchSimulationComparison simulation2)
      {
         var time1 = simulation1.Times;
         var time2 = simulation2.Times;

         if (time1?.Values == null)
            return undefinedTimeComparisonResult(simulation1);

         if (time2?.Values == null)
            return undefinedTimeComparisonResult(simulation2);

         if (time1.Values.Length != time2.Values.Length)
            return differentTimeLengthComparisonResult(simulation1, time1.Values, time2.Values);

         return null;
      }

      private OutputComparisonResult outputConsistencyCheckComparisonResult(BatchOutputComparison outputValues1, BatchOutputComparison outputValues2, ComparisonSettings comparisonSettings)
      {
         var values1 = outputValues1.OutputValues;
         var values2 = outputValues2.OutputValues;

         if (values1.Values == null)
            return undefinedValuesComparisonResult(outputValues1, comparisonSettings);

         if (values2.Values == null)
            return undefinedValuesComparisonResult(outputValues2, comparisonSettings);

         if (values1.Values.Length != values2.Values.Length)
            return differentOutputLengthComparisonResult(outputValues1, values1, values2, comparisonSettings);

         return null;
      }

      private double calculateDeviation(BatchOutputComparison outputValues1, BatchOutputComparison outputValues2)
      {
         var thresholdToUse = outputValues1.OutputValues.ComparisonThreshold;
         if (thresholdToUse == 0)
            thresholdToUse = outputValues2.OutputValues.ComparisonThreshold;

         return calculateDeviation(outputValues1.OutputValues, outputValues2.OutputValues, thresholdToUse);
      }

      private double calculateDeviation(BatchValues array1, BatchValues array2, double? threshold = null)
      {
         if (array1.Values.Length != array2.Values.Length)
            throw new InvalidArgumentException(Validation.ArraysHaveDifferentLength(array1.Values.Length, array2.Values.Length));

         //values are not necessarily saved 
         return array1.Values.Select((x, i) => relativeDeviation(x, array2.Values[i], threshold)).Max();
      }

      private double relativeDeviation(float value1, float value2, double? threshold = null)
      {
         if (float.IsNaN(value1) && float.IsNaN(value2))
            return 0;

         if (float.IsNaN(value1) || float.IsNaN(value2))
            return float.PositiveInfinity;

         if (value1 == 0 && value2 == 0)
            return 0;

         var deviation = Math.Abs((value1 - value2) / value1);

         if (!threshold.HasValue)
            return deviation;

         if (value1 <= threshold.Value && value2 <= threshold.Value)
            return 0;

         return deviation;
      }

      private static TimeComparisonResult timeDeviationTooLargeComparisonResult(double deviation)
      {
         return new TimeComparisonResult(ValidationState.Invalid, Validation.DeviationForTimeGreaterThanMaxTolearance(deviation, Constants.MAX_DEVIATION_TIME));
      }

      private static OutputComparisonResult outputDeviationTooLargeComparisonResult(BatchOutputComparison outputValues1, BatchOutputComparison outputValues2, double deviation, ComparisonSettings comparisonSettings)
      {
         return outputComparisonResultFrom(outputValues1, outputValues2, deviation, comparisonSettings, ValidationState.Invalid, Validation.DeviationForVariableGreaterThanMaxTolerance(outputValues1.Path, deviation, Constants.MAX_DEVIATION_OUTPUT));
      }

      private static OutputComparisonResult outputComparisonResultFrom(BatchOutputComparison outputValues1, BatchOutputComparison outputValues2, double deviation, ComparisonSettings comparisonSettings, ValidationState state, string message = null)
      {
         var comparisonParams = new OutputComparisonResultParams(outputValues1.Path)
         {
            TimeDisplayUnit = outputValues1.Times.Unit,
            ValuesDimension = outputValues1.OutputValues.Dimension,
            ValuesDisplayUnit = outputValues1.OutputValues.Unit
         };

         return new OutputComparisonResult(comparisonParams, comparisonSettings, state, message)
         {
            Output1 = outputResultFrom(outputValues1, comparisonSettings.FolderPathCaption1),
            Output2 = outputResultFrom(outputValues2, comparisonSettings.FolderPathCaption2),
            Deviation = deviation
         };
      }

      private static OutputResult outputResultFrom(BatchOutputComparison outputValue, string caption)
      {
         return new OutputResult(outputValue.Times.Values, outputValue.Values)
         {
            Caption = caption
         };
      }

      private static OutputComparisonResult differentOutputLengthComparisonResult(BatchOutputComparison outputValues1, BatchOutputValues values1, BatchOutputValues values2, ComparisonSettings comparisonSettings)
      {
         return new OutputComparisonResult(new OutputComparisonResultParams(outputValues1.Path), comparisonSettings, ValidationState.Invalid, Validation.ValueArraysHaveDifferentLength(outputValues1.SimulationName, outputValues1.Path, values1.Values.Length, values2.Values.Length));
      }

      private static OutputComparisonResult undefinedValuesComparisonResult(BatchOutputComparison batchOutputComparison, ComparisonSettings comparisonSettings)
      {
         return new OutputComparisonResult(new OutputComparisonResultParams(batchOutputComparison.Path), comparisonSettings, ValidationState.Invalid, Validation.ValueArrayDoesNotExist(batchOutputComparison.SimulationName, batchOutputComparison.Folder, batchOutputComparison.Path));
      }

      private static TimeComparisonResult differentTimeLengthComparisonResult(BatchSimulationComparison simulation1, float[] time1, float[] time2)
      {
         return new TimeComparisonResult(ValidationState.Invalid, Validation.TimeArraysHaveDifferentLength(simulation1.Name, time1.Length, time2.Length));
      }

      private static TimeComparisonResult undefinedTimeComparisonResult(BatchSimulationComparison simulation1)
      {
         return new TimeComparisonResult(ValidationState.Invalid, Validation.TimeArrayDoesNotExist(simulation1.Name, simulation1.Folder));
      }
   }
}