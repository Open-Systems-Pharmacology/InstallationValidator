using InstallationValidator.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility;
using static OSPSuite.Core.Domain.Constants;

namespace InstallationValidator.Core.Services
{
   public interface IOutputResultToDataRepositoryMapper
   {
      DataRepository MapFrom(OutputComparisonResult comparisonResult, OutputResult outputResult);
   }

   public class OutputResultToDataRepositoryMapper : IOutputResultToDataRepositoryMapper
   {
      private readonly IDimensionFactory _dimensionFactory;

      public OutputResultToDataRepositoryMapper(IDimensionFactory dimensionFactory)
      {
         _dimensionFactory = dimensionFactory;
      }

      public DataRepository MapFrom(OutputComparisonResult comparisonResult, OutputResult outputResult)
      {
         var repository = new DataRepository();
         var timeDimension = _dimensionFactory.Dimension(TIME);
         var timeGrid = new BaseGrid(TIME, timeDimension)
         {
            Values = outputResult.Times,
            DisplayUnit = displayUnitFor(timeDimension, comparisonResult.TimeDisplayUnit)
         };

         var valueDimension = _dimensionFactory.Dimension(comparisonResult.ValuesDimension);
         var outputColumn = new DataColumn("Values", valueDimension, timeGrid)
         {
            Values = outputResult.Values,
            Name = outputResult.Caption,
            DisplayUnit = displayUnitFor(valueDimension, comparisonResult.ValuesDisplayUnit)
         };

         repository.Add(outputColumn);

         return repository;
      }

      private Unit displayUnitFor(IDimension dimension, string unitName) => dimension.UnitOrDefault(unitName);
   }
}