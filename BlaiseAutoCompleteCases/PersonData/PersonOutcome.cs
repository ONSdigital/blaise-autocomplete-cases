using BlaiseAutoCompleteCases.Interfaces.PersonData;
using System.Collections.Generic;

namespace BlaiseAutoCompleteCases.PersonData
{
    public class PersonOutcome : IPersonOutcome
    {
        public Dictionary<string, string> getPersonOutcomeData()
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
