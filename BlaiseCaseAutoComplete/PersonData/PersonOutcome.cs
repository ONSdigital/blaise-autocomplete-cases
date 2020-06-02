using System.Collections.Generic;
using BlaiseCaseAutoComplete.Interfaces.PersonData;

namespace BlaiseCaseAutoComplete.PersonData
{
    public class PersonOutcome : IPersonOutcome
    {
        public Dictionary<string, string> GetPersonOutcomeData_Good()
        {
            var dictionary = new Dictionary<string, string>
            {
                {"QID.HHold", "1"},
                {"QHAdmin.HOut", "110"},
                {"QHAdmin.IntNum", "1001"},
                {"Mode", "1"},
                {"Processed", "2"},
                {"QDataBag.PostCode", "XX999XX"},
                {"QHousehold.QHHold.Person[1].Sex", "1"},
                {"QHousehold.QHHold.Person[1].tmpDoB", "1/1/1980"},
                {"QHousehold.QHHold.Person[1].DVAge", "40"}
            };
            return dictionary;
        }
    }
}
