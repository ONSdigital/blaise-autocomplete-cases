using System.Collections.Generic;

namespace BlaiseAutoCompleteCases.Models
{
    public class ValidationModel
    {
        public ValidationModel()
        {
            qid_fields = new Dictionary<string, dynamic>();
            data_fields = new Dictionary<string, string>();
        }

        public string primary_key { get; set; }
        public string rules_id { get; set; }
        public Dictionary<string, dynamic> qid_fields { get; set; }
        public Dictionary<string, string> data_fields { get; set; }
    }
}
