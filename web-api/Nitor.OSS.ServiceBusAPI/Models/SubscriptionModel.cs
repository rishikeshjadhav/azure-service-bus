
namespace Nitor.OSS.ServiceBusAPI.Models
{
    public class SubscriptionModel
    {
        public string Name { get; set; }
        public string TopicName { get; set; }
        public string DefaultMessageTime { get; set; }
        public string LockDuration { get; set; }
        public string MaxDeliveryCount { get; set; }
        public bool MoveExpiredToDLQ { get; set; }
        public bool EnableDeadLetteringOnFilterEvaluationExceptions { get; set; }
        public bool EnableSessions { get; set; }
        public bool AreFiltersRequired { get; set; }
        public string SQLFilterExpression { get; set; }
    }
}
