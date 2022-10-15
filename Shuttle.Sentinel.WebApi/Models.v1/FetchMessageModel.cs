namespace Shuttle.Sentinel.WebApi.Models.v1
{
    public class FetchMessageModel
    {
        public string QueueUri { get; set; }
        public int Count { get; set; } 
    }
}