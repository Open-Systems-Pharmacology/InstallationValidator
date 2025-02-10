using System;
using InstallationValidator.Core.Assets;
using InstallationValidator.Core.Extensions;
using OSPSuite.Core.Domain;

namespace InstallationValidator.Core.Domain
{
   public abstract class ValueComparisonResult : IWithValidationState
   {
      public ValidationState State { get; }
      public string Message { get; }
      public double Deviation { get; set; }

      protected ValueComparisonResult(ValidationState state, string message = null)
      {
         State = state;
         Message = message ?? string.Empty;
      }
   }

   public class TimeComparisonResult : ValueComparisonResult
   {
      public TimeComparisonResult(ValidationState state, string message = null) : base(state, message)
      {
      }
   }

   public class OutputComparisonResultParams
   {
      public string Path { get; }

      /// <summary>
      ///    Unit in which times should be displayed
      /// </summary>
      public string TimeDisplayUnit { get; set; }

      /// <summary>
      ///    Unit in which values should be displayed
      /// </summary>
      public string ValuesDisplayUnit { get; set; }

      /// <summary>
      ///    Dimension in which values should be displayed
      /// </summary>
      public string ValuesDimension { get; set; }

      public OutputComparisonResultParams(string path)
      {
         Path = path;
      }
   }

   public class OutputComparisonResult : ValueComparisonResult
   {
      public ComparisonSettings ComparisonSettings { get; }

      private readonly OutputComparisonResultParams _comparisonParams;

      /// <summary>
      ///    Values of output 1. Can be null typically for a valid state
      /// </summary>
      public OutputResult Output1 { get; set; } = new NullOutputResult();

      /// <summary>
      ///    Values of output 2. Can be null typically for a valid state
      /// </summary>
      public OutputResult Output2 { get; set; } = new NullOutputResult();

      public OutputComparisonResult(OutputComparisonResultParams comparisonParams, ComparisonSettings comparisonSettings, ValidationState state, string message = null) : base(state, message)
      {
         ComparisonSettings = comparisonSettings;
         _comparisonParams = comparisonParams;
      }

      public bool HasData => !Output1.IsNullOutput() && !Output2.IsNullOutput();

      public string Path => _comparisonParams.Path;

      public string TimeDisplayUnit => _comparisonParams.TimeDisplayUnit;

      public string ValuesDimension => _comparisonParams.ValuesDimension;

      public string ValuesDisplayUnit => _comparisonParams.ValuesDisplayUnit;
   }

   public class OutputResult
   {
      public string Caption { get; set; }
      public float[] Times { get; }

      public float[] Values { get; }

      public OutputResult(float[] times, float[] values)
      {
         Times = times;
         Values = values;
      }
   }

   public class NullOutputResult : OutputResult
   {
      public NullOutputResult() : base(Array.Empty<float>(), Array.Empty<float>())
      {
      }
   }

   public class MissingOutputComparisonResult : OutputComparisonResult
   {
      public MissingOutputComparisonResult(string path, ComparisonSettings comparisonSettings, string simulationName, string folderPath, string folderType)
         : base(new OutputComparisonResultParams(path), comparisonSettings, ValidationState.Invalid, Validation.OutputIsMissingFromSimulation(path, simulationName, folderPath, folderType))
      {
      }
   }
}