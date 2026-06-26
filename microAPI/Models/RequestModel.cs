namespace microAPI.Models
{
    public class RequestModel
    {
        public string Name {get; set;} = string.Empty;
        public string Method {get; set;} = "GET";
        public string Url {get; set;} = string.Empty;
        public string RequestBody {get; set;} = string.Empty;
        public string ResponseBody {get; set;} = string.Empty;
        public string StatusCode {get; set;} = string.Empty;
    }
}