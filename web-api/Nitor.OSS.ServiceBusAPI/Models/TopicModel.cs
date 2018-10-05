
namespace Nitor.OSS.ServiceBusAPI.Models
{
    class TopicModel
    {
        public string Name { get; set; }
        public string MaxTopicSize { get; set; }
        public string MessageTimeToLive { get; set; }
        public bool DuplicateDetection { get; set; }
        public string DuplicateDetectionTimeWindowInSeconds { get; set; }
        public bool EnablePartitioning { get; set; }
    }
}
