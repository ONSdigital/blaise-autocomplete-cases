using StatNeth.Blaise.API.DataRecord;

namespace BlaiseAutoCompleteCases.Interfaces.Services
{
    public interface IDataService
    {
        void GetDataAndUpdate(IDataRecord dataRecord, string instrumentName, string serverPark);
    }
}