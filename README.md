# blaise-autocomplete-cases

Windows service that will auto-complete cases in Bliase. It can complete a specified case, or a number of random cases depending upon the message received

For a specific case you need to specify the instrument name, the server park, and the primary key value. The number of cases property is then ignored.

For non specific cases, you simply need to specify the server park and the number of cases you would like to complete. The records processed are random in terms of which ones are chosen in the 
blaise database. 

You must specify a payload for both methods, which will represent any fields you wish to update

      E.g.           
		"QID.HHold": "1",
        "QHAdmin.HOut": "110",
        "QHAdmin.IntNum": "1001",
        "Mode": "1",
        "Processed": "2",
        "QDataBag.PostCode": "XX999XX",
        "QHousehold.QHHold.Person[1].Sex": "1",
        "QHousehold.QHHold.Person[1].tmpDoB": "1/1/1980",
        "QHousehold.QHHold.Person[1].DVAge": "40"
	

# Triggerd by:
    
      Message on PubSub topic: autocomplete-cases-topic and subscription: autocomplete-cases-subscription

# PubSub Topic Message content example:

specific case : 
{ 	
	"instrument_name": "OPN2004A",
	"server_park": "tel",
	"primary_key": "9000001",
	"NumberOfCases": 0,
	"payload":  
	{
		"QID.HHold": "1",
        "QHAdmin.HOut": "110",
        "QHAdmin.IntNum": "1001",
        "Mode": "1",
        "Processed": "2",
        "QDataBag.PostCode": "XX999XX",
        "QHousehold.QHHold.Person[1].Sex": "1",
        "QHousehold.QHHold.Person[1].tmpDoB": "1/1/1980",
        "QHousehold.QHHold.Person[1].DVAge": "40"
		}
}


non specific multiple cases : 
    { 
    "instrument_name":"opn2004a", 
	"server_park": "tel",
    "NumberOfCases":"2" 
	"payload":  
	{
		"QID.HHold": "1",
        "QHAdmin.HOut": "110",
        "QHAdmin.IntNum": "1001",
        "Mode": "1",
        "Processed": "2",
        "QDataBag.PostCode": "XX999XX",
        "QHousehold.QHHold.Person[1].Sex": "1",
        "QHousehold.QHHold.Person[1].tmpDoB": "1/1/1980",
        "QHousehold.QHHold.Person[1].DVAge": "40"
		}
    }


# Local debug:

To run the service locally please uncomment this line "System.Threading.Thread.Sleep(1200000)" in the InitialiseService.cs in the BlaiseCaseAutoComplete project.  This ensure that there is a delay after the GCP subscription is set up - allowing time for a message to be put onthe topic.  



