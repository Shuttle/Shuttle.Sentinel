namespace Shuttle.Sentinel.WebApi
{
    public class FetchMessageModel
    {
        public string QueueUri { get; set; }
        public int Count { get; set; } 
    }
}