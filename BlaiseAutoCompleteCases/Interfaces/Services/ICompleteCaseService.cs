using StatNeth.Blaise.API.DataRecord;

namespace BlaiseAutoCompleteCases.Interfaces.Services
{
    public interface ICompleteCaseService
    {
        void CompleteCase(IDataRecord dataRecord, string instrumentName, string serverPark);
    }
}