# blaise-autocomplete-cases
Windows service that will auto-complete cases in Bliase.  The records processed are random in terms of which ones are chosen in the blaise database. The number of cases that are updated is entered by specifying a number in "NumberOfCasesToComplete".  The survey in "surveyname" is the survey that you want to update accross all server parks.

    Triggerd by:
    
      Message on PubSub topic: autocomplete-cases-topic and subscription: autocomplete-cases-subscription

    PubSub Topic Message content example:

      { "surveyname":"opn2004a", "NumberOfCasesToComplete":"2" }

    Data

      The initial version of this C# service will add data for a 'Good' outcome, along with MI data to each record specified by the above request on the pubsub Topic.

      E.g.            {"QID.HHold", "1"},
                      {"QHAdmin.HOut", "110"},
                      {"QHAdmin.IntNum", "1001"},
                      {"Mode", "1"},
                      {"Processed", "2"},
                      {"QDataBag.PostCode", "XX999XX"},
                      {"QHousehold.QHHold.Person[1].Sex", "1"},
                      {"QHousehold.QHHold.Person[1].tmpDoB", "1/1/1980"},
                      {"QHousehold.QHHold.Person[1].DVAge", "40"}


# Local debug:

To run the service locally please uncomment this line "System.Threading.Thread.Sleep(1200000)" in the InitialiseService.cs in the BlaiseCaseAutoComplete project.  This ensure that there is a delay after the GCP subscription is set up - allowing time for a message to be put onthe topic.  



