# blaise-case-auto-complete

This service updates fields of Blaise cases that is receives via a Pub/Sub topic. This can be used to automate populate/complete of cases for testing purposes. The service can update fields of a specified case, or a number of random cases depending upon the message received.

For a specific case you need to specify the instrument name, the server park, and the primary key value. The number of cases property is then ignored.

For non specific cases, you simply need to specify the server park and the number of cases you would like to populate. The records processed are random in terms of which ones are chosen in the blaise database.

Example message to complete an OPN telephone interview for a specific case:

```
{
  "server_park":"tel",
  "instrument_name":"OPN2004A",
  "primary_key":"1000011",
  "NumberOfCases":0,
  "payload":
    {
      "Processed":"2",
      "Completed":"1",
      "Mode":"1",
      "QHAdmin.IntNum":"1024",
      "QHAdmin.IntDone":"1",
      "QHAdmin.HOut":"110",
      "QOutro.Comments":"AUTO-COMPLETE"
    }
}
```

Example message to complete 10 random OPN telephone interview cases:

```
{
  "server_park":"tel",
  "instrument_name":"OPN2004A",
  "NumberOfCases":10,
  "payload":
    {
      "Processed":"2",
      "Completed":"1",
      "Mode":"1",
      "QHAdmin.IntNum":"1024",
      "QHAdmin.IntDone":"1",
      "QHAdmin.HOut":"110",
      "QOutro.Comments":"AUTO-COMPLETE"
    }
}
```

Example message to complete 10 random OPN web interview cases:

```
{
  "server_park":"tel",
  "instrument_name":"OPN2004A",
  "NumberOfCases":10,
  "payload":
    {
      "Processed":"2",
      "Completed":"1",
      "Mode":"3",
      "WebFormStatus":"1",
      "WebHOut":"110",
      "QHAdmin.IntNum":"1024",
      "QHAdmin.IntDone":"1",
      "QHAdmin.HOut":"110",
      "QOutro.Comments":"AUTO-COMPLETE"
    }
}
```

# Local debug

To run the service locally please uncomment the line "System.Threading.Thread.Sleep(1200000)" in the InitialiseService.cs in the BlaiseCaseAutoComplete project. This ensures that there is a delay after the Pub/Sub subscription is set up, allowing time for a message to be put on the topic.
