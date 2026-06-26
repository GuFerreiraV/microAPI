using System.Collections.Generic;


namespace microAPI.Models
{
    public class CollectionModel
    {   
        public string Name {get; set;} = string.Empty;
        public List<RequestModel> Requests {get; set;} = new();
    }
}