using StatNeth.Blaise.API.DataRecord;

namespace BlaiseCaseAutoComplete.Interfaces.Services
{
    public interface ICompleteCaseService
    {
        void CompleteCase(IDataRecord dataRecord, string instrumentName, string serverPark);
    }
}