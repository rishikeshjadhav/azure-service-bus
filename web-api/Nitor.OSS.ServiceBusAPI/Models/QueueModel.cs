
namespace Nitor.OSS.ServiceBusAPI.Models
{
    public class QueueModel
    {
        public string Name { get; set; }
        public string MaxQueueSize { get; set; }
        public string MessageTimeToLive { get; set; }
        public string LockDurationTime { get; set; }
        public bool DuplicateDetection { get; set; }
        public string DuplicateDetectionTimeWindowInSeconds { get; set; }
        public bool DeadLettering { get; set; }
        public bool Session { get; set; }
        public bool EnablePartitioning { get; set; }
    }
}
