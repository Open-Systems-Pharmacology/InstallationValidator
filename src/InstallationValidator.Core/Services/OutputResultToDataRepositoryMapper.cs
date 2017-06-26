using InstallationValidator.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility;

namespace InstallationValidator.Core.Services
{
   public interface IOutputResultToDataRepositoryMapper : IMapper<OutputResult, DataRepository>
   {
   }

   public class OutputResultToDataRepositoryMapper : IOutputResultToDataRepositoryMapper
   {
      private readonly IDimensionFactory _dimensionFactory;

      public OutputResultToDataRepositoryMapper(IDimensionFactory dimensionFactory)
      {
         _dimensionFactory = dimensionFactory;
      }

      public DataRepository MapFrom(OutputResult outputResult)
      {
         var repository = new DataRepository();

         var timeGrid = new BaseGrid(OSPSuite.Core.Domain.Constants.TIME, _dimensionFactory.Dimension(OSPSuite.Core.Domain.Constants.TIME))
         {
            Values = outputResult.Times
         };

         var outputColumn = new DataColumn("Values", _dimensionFactory.Dimension(outputResult.Dimension), timeGrid)
         {
            Values = outputResult.Values,
            Name = outputResult.Caption
         };

         repository.Add(outputColumn);

         return repository;
      }
   }
}