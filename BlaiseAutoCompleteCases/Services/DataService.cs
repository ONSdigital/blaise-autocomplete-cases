using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Interfaces;
using BlaiseAutoCompleteCases.Helpers;
using BlaiseAutoCompleteCases.Interfaces.PersonData;
using BlaiseAutoCompleteCases.Interfaces.Services;
using log4net;
using StatNeth.Blaise.API.DataRecord;


namespace BlaiseAutoCompleteCases.Services
{
    public class DataService : IDataService
    {
        private readonly ILog _logger;
        private readonly IBlaiseApi _blaiseApi;
        private readonly IPersonOutcome _personOutCome;

        public DataService(ILog logger, IBlaiseApi blaiseApi)
        {
            _logger = logger;
            _blaiseApi = blaiseApi;
        }

        public void GetDataAndUpdate(IDataRecord dataRecord, string instrumentName, string serverParkName)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            var dictionary = _personOutCome.getPersonOutcomeData();

            //_logger.Info($"Populating data for Blaise record - Serial_number: {dataRecord.Keys[0].KeyValue}");
            _blaiseApi.UpdateDataRecord(dataRecord, GetMiDictionary(), instrumentName, serverParkName);
        }

        private static Dictionary<string, string> GetMiDictionary()
        {
            var dictionary = new Dictionary<string, string>
            {
                {"QID.HHold", "1"},
                {"QHAdmin.HOut", "310"},
                {"QHAdmin.IntNum", "1001"},
                {"Mode", "1"},
                {"QDataBag.PostCode", "XX999XX"},
                {"QHousehold.QHHold.Person[1].Sex", "1"},
                {"QHousehold.QHHold.Person[1].tmpDoB", "1/1/1980"},
                {"QHousehold.QHHold.Person[1].DVAge", "40"}
            };
            return dictionary;
        }
    }
}