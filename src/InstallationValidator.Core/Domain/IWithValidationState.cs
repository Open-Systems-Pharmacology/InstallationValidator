using OSPSuite.Core.Domain;

namespace InstallationValidator.Core.Domain
{
   public interface IWithValidationState
   {
      ValidationState State { get; }
   }

 
}